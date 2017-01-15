using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearingUIController : MonoBehaviour {
    public GameObject elementPrefab;
    public float yStep = 200;
    public float yBegin = 400;

    int elementCount = 0;

    public void AddElement(GameController.Record record) {
        var instance = Instantiate(elementPrefab, transform);
        instance.transform.localPosition = new Vector3(20, yBegin + (elementCount) * yStep);
        instance.transform.localScale = new Vector3(1.05f, 1.05f, 1);

        instance.transform.FindChild("Basic").GetComponent<Text>().text = record.patient.patientName;

        var action = " 在";
        action += record.healOption.endingText2;
        action += "后";
        action += record.ending;
        instance.transform.FindChild("Action").GetComponent<Text>().text = action;

        var rep = "";
        rep += " 健康" + WrapDelta(record.consequence.healthChange);
        rep += " 声望" + WrapDelta(record.consequence.reputationChange);
        rep += " 金钱" + WrapDelta(record.consequence.moneyChange);
        rep += " 道德" + WrapDelta(record.consequence.moralChange);

        instance.transform.FindChild("ValueChange").GetComponent<Text>().text = rep;
        ++elementCount;
    }

    string WrapDelta(int delta) {
        return delta >= 0 ? ("+" + delta ) : ("-" + delta);
    }

    public void ClearElements() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        elementCount = 0;
    }



}
