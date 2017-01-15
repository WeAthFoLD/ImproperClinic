using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour {
    public enum Ending {
        Heal, Worsen, Die
    }

    [System.Serializable]
    public struct Answer {
        public string content;
    }

    [System.Serializable]
    public struct QA {
        public Question question;
        public Answer answer;
    }

    [System.Serializable]
    public class QAItem : WeightedItem<QA> {
        public QA qa;
        public float _weight = 1;
        public float weight { get { return _weight; }}

        public QA val { get { return qa; } }
    }

    [System.Serializable]
    public struct AnswerItem {
        ///
        ///<summary>Question index in question category.</summary>
        ///
        public int questionIndex;
        public Answer answer;
    }

    [System.Serializable]
    public struct ResponseItem {
        public string selection;
        public string response;
    }

    [System.Serializable]
    public struct ConsequenceGroup {
        public Consequence consequence;
        public Ending ending;
    }

    [System.Serializable]
    public struct ConsequenceGroupItem {
        public string id;
        public ConsequenceGroup grp;
    }

    public string patientName = "冬马小三";

    public Disease disease;

    public List<Disease> relatedDisease;

    [Multiline]
    public string diagnosticMessage;

    public List<QAItem> QAs;

    public List<ResponseItem> responses;

    public List<ConsequenceGroupItem> consequences;

    public ConsequenceGroup defaultConsequence;

    public Sprite sprite;

    public string GetResponse(Disease disease) {
        foreach (var it in responses) {
            return it.response;
        }
        return "Invalid reponse";
    }

    public ConsequenceGroup GetConsGrp(string id) {
        foreach (var it in consequences) {
            if (it.id == id) {
                return it.grp;
            }
        }
        return defaultConsequence;
    }

}
