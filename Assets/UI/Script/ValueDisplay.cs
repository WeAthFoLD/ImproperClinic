using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour {

    public string text {
        set {
            childText.text = value;
        }
    }

    Text childText;

    void Start() {
        childText = transform.FindChild("Text").GetComponent<Text>();
    }

}
