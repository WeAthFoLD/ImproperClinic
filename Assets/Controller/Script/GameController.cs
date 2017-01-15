using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour {

    [System.Serializable]
    public struct PatientItem: WeightedItem<Patient> {
        public Patient _patient;
        public float _weight;

        public Patient val { get { return _patient; }}

        public float weight { get { return _weight; }}
    }

    struct Option {
        public string content;
        public System.Action callback;
    }

    public struct Record {
        public Patient patient;
        public HealOption healOption;
        public Consequence consequence;
        public string ending;
    }

    public Player playerPrefab;

    public List<PatientItem> patients;

    public GameObject optionList;

    public Text patientText;

    public ValueDisplay vHealth;

    public GameObject diagnosticUI, clearingUI, deathUI;

    public ClearingUIController clearingList;

    public Image characterImage;

    public Player player;

    Patient patient;

    int patientsLeft;

    public int dayCount = 1;

    List<Record> records = new List<Record>();

    public AudioClip
        soundHeal, soundDie, soundDoorOpen, soundDoorClose, soundClick, soundClear;

    AudioSource audioSource;

    Patient NextPatient() {
        return WeightedAverage.RandItem<PatientItem, Patient>(patients);
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        Restart();
    }

    void PlaySound(AudioClip c) {
        audioSource.PlayOneShot(c);
    }

    public void Restart() {
        if (player != null)
            Destroy(player.gameObject);

        player = Instantiate(playerPrefab);

        StartDiagnosing();
    }

    void Update() {
        vHealth.text = player.health.ToString();
    }

    public void StartDiagnosing() {
        patient = NextPatient();
        characterImage.sprite = patient.sprite;
        characterImage.GetComponent<Animator>().Play("Idle");

        diagnosticUI.SetActive(true);
        clearingUI.SetActive(false);
        deathUI.SetActive(false); 

        if (patientsLeft == 0) {
            patientsLeft = 3;
            ++dayCount;
        }

        PlaySound(soundDoorOpen);

        ShowPatientText(patient.diagnosticMessage);

        StartDiagnoseQuestion();
    }

    void StartDiagnoseQuestion() {
        var qaLeft = new List<Patient.QAItem>(patient.QAs);
        ApplyDiagnoseQuestion(qaLeft, false);
    }

    void ApplyDiagnoseQuestion(List<Patient.QAItem> left, bool endAction) {
        var callbacks = new List<Option>();
        var questions = WeightedAverage.SelectN<Patient.QAItem, Patient.QA>(left, 4);
        var options = questions.ConvertAll(q => new Option { 
            content = q.question.content, callback = () => {
                var content = q.answer.content;
                

                ShowPatientText(content);
                if (endAction) {
                    StartDiagnoseDiagnose();
                } else {
                    left.RemoveAll(it => it.val.Equals(q)); 
                    ApplyDiagnoseQuestion(left, true);
                }
            } });
        
        ShowOptionList(options);
    }

    void StartDiagnoseDiagnose() {
        var rawOptions = new List<Disease>();
        rawOptions.Add(patient.disease);

        patient.relatedDisease.Shuffle();
        rawOptions.AddRange(patient.relatedDisease);

        var options = rawOptions.ConvertAll(it => new Option { content = it.displayName, 
            callback = () => {
                StartDiagnoseDecide(it);
            }});
        ShowOptionList(options);
    }

    void StartDiagnoseDecide(Disease selectedDisease) {
        ShowPatientText(patient.GetResponse(selectedDisease));

        var options = new List<Option>();
        options.AddRange(selectedDisease.effectiveOptions.ConvertAll(it => new Option {
            content = it.text,
            callback = () => {
                var grp = patient.GetConsGrp(it.consequenceID);
                StartApplyEffect(grp.consequence, it, grp.ending.Name());
            }
        }));
        options.Shuffle();

        ShowOptionList(options);
    }

    void StartApplyEffect(Consequence cons, HealOption option, string ending) {
        ShowPatientText(ending);

        StartCoroutine(DoApplyEffect(cons, option, ending));
    }

    IEnumerator DoApplyEffect(Consequence cons, HealOption option, string ending) {
        characterImage.GetComponent<Animator>().Play("BlendOut");
        PlaySound(soundDoorClose);

        // hack: hide the options
        var lst = new List<Option>();
        ShowOptionList(lst);
        yield return new WaitForSeconds(2.0f);

        player.money = System.Math.Max(0, player.money + cons.moneyChange);
        player.moral = System.Math.Max(0, player.moral + cons.moralChange);
        player.reputation = System.Math.Max(0, player.reputation + cons.reputationChange);
        player.health = System.Math.Max(0, player.health + cons.healthChange);

        records.Add(new Record { patient = patient, healOption = option, consequence = cons, ending = ending });

        patientsLeft -= 1;
        if (player.health == 0) {
            StartDeath();
            PlaySound(soundDie);
        } else if (patientsLeft == 0) {
            StartClearing();
            PlaySound(soundClear);
        } else {
            StartDiagnosing();
        }
    }


    void StartClearing() {
        diagnosticUI.SetActive(false);
        clearingUI.SetActive(true);
        deathUI.SetActive(false);
        
        clearingList.ClearElements();
        foreach (var it in records) {
            clearingList.AddElement(it);
        }

        records.Clear();
    }

    void StartDeath() {
        deathUI.SetActive(true);
        diagnosticUI.SetActive(false);
        clearingUI.SetActive(false);
    }

    public void HidePatientText() {
        patientText.transform.parent.gameObject.SetActive(false);
        optionList.SetActive(true);
    }

    void ShowPatientText(string text) {
        patientText.transform.parent.gameObject.SetActive(true);
        optionList.SetActive(false);

        patientText.text = text;
    }

    public void PlayClickSound() {
        print("PlayClickSound");
        PlaySound(soundClick);
    }

    void ShowOptionList(List<Option> options) {
        // HidePatientText();

        for (int i = 0; i < 4; ++i) {
            var button = optionList.transform.FindChild("Option" + i).GetComponent<OptionButton>();
            if (i < options.Count) {
                button.gameObject.SetActive(true);
                button.callback = options[i].callback;
                button.GetComponentInChildren<Text>().text = options[i].content;
            } else {
                button.gameObject.SetActive(false);
            }

            
        }
    }
    
}
