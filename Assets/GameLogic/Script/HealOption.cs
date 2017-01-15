using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HealOption {

    public string text;
    [SerializeField]
    private string endingText;

    [SerializeField]
    private string _consequenceID;

    public string endingText2 {
        get { return endingText == "" ? text : endingText; }
    }

    public string consequenceID {
        get { return _consequenceID == "" ? text : _consequenceID; }
    }

}
