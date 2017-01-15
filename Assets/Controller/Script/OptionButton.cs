using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour, IPointerDownHandler {
    public System.Action callback;

    public void OnPointerDown (PointerEventData eventData) {
		if (callback != null) {
            callback();
        }
	}

}


