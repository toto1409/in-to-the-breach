using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMiddlePlayer : MonoBehaviour {

    public int slot;
    public int PlayerID;

	void Start ()
    {
        PlayerID = DataBase.playerMech[slot];

        GetComponent<SpriteRenderer>().sprite = Resources.Load("charter/anmator/mech_" + (PlayerID + 1)) as Sprite;
        GetComponent<Animator>().runtimeAnimatorController = Resources.Load("charter/anmator/mech_" + (PlayerID + 1) + "_0") as RuntimeAnimatorController;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (PlayerID + 1) + "/Mech_" + (PlayerID + 1));
        transform.GetChild(1).GetComponent<HpBar>().player = GameObject.Find("Player").transform.GetChild(slot).GetComponent<Player>();
    }
}
