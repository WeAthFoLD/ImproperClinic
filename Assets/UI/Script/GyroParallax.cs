using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroParallax : MonoBehaviour {

    Text text;

    public Vector2 rate;

    Vector3 lastEuler;

    void Start() {
        text = GetComponent<Text>();

        lastEuler = Input.acceleration;
        targetPosition = transform.localPosition;
        
        StartCoroutine(DoResample());
    }

    Vector3 targetPosition;

    Vector3 acc;
	
    IEnumerator DoResample() {
        do {
            acc = Input.acceleration;
            yield return new WaitForSeconds(0.1f);
        } while (true);
    }

    void Update () {
        var euler = acc;
        var delta = euler - lastEuler;
        targetPosition += new Vector3(delta.x * rate.x, delta.y * rate.y);
        var l = transform.localPosition; 
        transform.localPosition = new Vector3
            (Mathf.MoveTowards(l.x, targetPosition.x, Time.deltaTime * Mathf.Abs(rate.x) * 2f),
             Mathf.MoveTowards(l.y, targetPosition.y, Time.deltaTime * Mathf.Abs(rate.y) * 2f),
            1);

        lastEuler = euler;
    }
    
}
