using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public class hirightControl : MonoBehaviour
{
    public TurnBaseBattleManager tm;
    public Functions f;
    private int MechNumber;
    public int slot;
    public Player player;
    private bool initialize = true;
    public Sound s;

    private void Start()
    {
        tm = TurnBaseBattleManager.GetInst();
        f = Functions.GetInst();
        MechNumber = DataBase.playerMech[slot];
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
    }

    private void Update()
    {
        if (player)
        {
            if (player.health <= 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                GetComponent<hirightControl>().enabled = false;
            }
        }

        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN)
        {
            if (initialize)
            {
                player = GameObject.Find("Player" + slot).GetComponent<Player>();
                initialize = false;
            }

            if (UIControl.GetInst().proflieMiddlePick && player.ClickOn)
            {
                if (player.onWater)
                {
                    ProfileInfo.GetInst().OnWaterPlayer();
                }
                else
                {
                    ProfileInfo.GetInst().OutOfWaterPlayer();
                }
            }

            if (player.ClickOn)
            {
                this.transform.GetChild(0).gameObject.SetActive(true); // 하이라이트 온
                this.transform.GetChild(1).gameObject.SetActive(true); // Press 온
            }
            else
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(false);
            }

            if (player.MouseOn)
            {
                this.transform.GetChild(0).gameObject.SetActive(true); // 하이라이트 온
            }
            else
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                s.SoundPlay("EffectSound/ui_map_window_close");
                ClearProInfo();
                player.ClickOn = false;
                player.ClearMovePath();
                player.EndMouseOver();
                if (player.Mode == Unit.MODE.Move)
                {
                    player.Mode = Unit.MODE.None;
                }
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(false);
                UIControl.GetInst().proflieMiddlePick = false;
            }
        }
    }

    void OnMouseEnter()
    {
        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN && ProfileInfo.GetInst().clickFlag == false)
        {
            if (UIControl.GetInst().proflieMiddlePick == false)
            {
                if(player.onWater)
                {
                    ProfileInfo.GetInst().OnWaterPlayer();
                }
                else
                {
                    ProfileInfo.GetInst().OutOfWaterPlayer();
                }

                this.transform.GetChild(0).gameObject.SetActive(true); // 하이라이트 온
                ClearProInfo();
                GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.PLAYER).GetChild(MechNumber).gameObject.SetActive(true); // id에 맞는 프로필 이미지 출력
                GameObject.Find("weponUI").transform.GetChild(MechNumber).gameObject.SetActive(true); // 플레이어 id에 맞는 무기 버튼 출력
                GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(true); // 리페어 버튼 출력
                GameObject.Find("ProfileInfoBottom").transform.GetChild(2).gameObject.SetActive(true); // 플레이어 무기 인터페이스 출력
                s.SoundPlay("EffectSound/ui_general_highlight_button");

                player.MouseOn = true;
                UIControl.GetInst().proflieMiddleMouseOn = true;
            }
        }
    }

    void OnMouseExit()
    {
        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN)
        {
            if (ProfileInfo.GetInst().clickFlag == false)
            {
                ClearProInfo();
                this.transform.GetChild(0).gameObject.SetActive(false); // 하이라이트 아웃

                player.MouseOn = false;
                UIControl.GetInst().proflieMiddleMouseOn = false;
            }
        }
    }

    void OnMouseDown()
    {
        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN)
        {
            UIControl.GetInst().proflieMiddlePick = false;

            foreach (GameObject player in tm.playerList)
            {
                player.GetComponent<Player>().ClickOn = false;
            }
            foreach (GameObject enemy in tm.enemyList)
            {
                enemy.GetComponent<Enemy>().ClickOn = false;
            }
            ClearProInfo();

            player.ClickOn = true;
            player.Mode = Unit.MODE.Move;
            ClickManager.GetInst().player = player;

            GameObject tmp = GameObject.Find("PlayerHiriht");
            for (int i = 0; i < 3; i++)
            {
                tmp.transform.GetChild(i).GetChild(1).gameObject.SetActive(false); // Press 이미지 클리어
            }
            this.transform.GetChild(1).gameObject.SetActive(true); // 자신의 Press 이미지 출력

            GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.PLAYER).GetChild(MechNumber).gameObject.SetActive(true); // id에 맞는 프로필 이미지 출력
            GameObject.Find("weponUI").transform.GetChild(MechNumber).gameObject.SetActive(true); // 플레이어 id에 맞는 무기 버튼 출력
            GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(true); // 리페어 버튼 출력
            GameObject.Find("ProfileInfoBottom").transform.GetChild(2).gameObject.SetActive(true); // 플레이어 무기 인터페이스 출력
            s.SoundPlay("EffectSound/ui_map_window_open");

            UIControl.GetInst().proflieMiddlePick = true;
            ProfileInfo.GetInst().holdInterface = true;
        }
    }

    public void ClearProInfo()
    {
        if (UIControl.GetInst().proflieMiddlePick == false)
        {
            int playerCount, enemyCount, weaponCount;

            playerCount = GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.PLAYER).GetChildCount();
            enemyCount = GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.ENEMY).GetChildCount();
            weaponCount = GameObject.Find("weponUI").transform.GetChildCount();

            GameObject.Find("ProfileInfoBottom").transform.GetChild(2).gameObject.SetActive(false);
            GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 0; i < playerCount; i++)
            {
                GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.PLAYER).GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < enemyCount; i++)
            {
                GameObject.Find("ProfileInfoBottom").transform.GetChild(ProfileInfo.ENEMY).GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < weaponCount; i++)
            {
                GameObject.Find("weponUI").transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}