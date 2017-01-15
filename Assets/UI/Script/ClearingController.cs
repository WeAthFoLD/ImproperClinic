using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClearingController : MonoBehaviour {

    public GameController gc;
    public Text tMoral, tHealth, tReputation, tMoney, tDay;
	
    void Update () {
        tMoral.text = "道 德：" + gc.player.moral;
        tHealth.text = "生 命：" + gc.player.health;
        tReputation.text = "名 声：" + gc.player.reputation;
        tMoney.text = "金 钱：" + gc.player.money;
        tDay.text = "第 " + gc.dayCount + " 天";
    }
}
