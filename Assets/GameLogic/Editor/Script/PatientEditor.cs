using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patient))]
public class PatientEditor : Editor {

    void OnInspectorGUI() {
        base.OnInspectorGUI();

        var patient = (Patient) target;
        
    }
    
}
