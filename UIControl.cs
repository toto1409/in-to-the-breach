using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl Inst = null;
    TurnBaseBattleManager tm;
    PlayerPositionSet pps;
    GameObject battleStarUI;
    GameObject startingGameUI;
    GameObject titleCanvas;
    GameObject backGround;
    public GameObject confirmButton;
    public float backGroundAlpha;
    public bool proflieMiddlePick = false;
    public bool proflieMiddleMouseOn = false;
    public bool check = true;

    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        tm = TurnBaseBattleManager.GetInst();
        pps = PlayerPositionSet.GetInst();
        battleStarUI = GameObject.Find("BattleStartUI");
        startingGameUI = GameObject.Find("startingGameUI");
        titleCanvas = GameObject.Find("TitleCanvas");
        backGround = GameObject.Find("Background");
    }

    void Update()
    {
        switch (tm.currentState)
        {
            case TurnBaseBattleManager.BattleStates.PLAYERPOSITIONSET:
                {
                    // 배치 버튼 활성화/ 비활성화
                    if (pps.isDeployEnd && !pps.isReDeploy)
                    {
                        confirmButton.SetActive(true);
                    }
                    else
                    {
                        confirmButton.SetActive(false);
                    }
                }
                break;

            case TurnBaseBattleManager.BattleStates.ENEMYTURN:
                {
                    SpriteRenderer bg = backGround.transform.GetChild(1).GetComponent<SpriteRenderer>();
                    if (backGroundAlpha < 1f)
                    {
                        backGroundAlpha += 0.1f;
                        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, backGroundAlpha);
                    }

                    startingGameUI.transform.GetChild(4).gameObject.SetActive(true); // 적턴 진행 중 문구
                }
                break;

            case TurnBaseBattleManager.BattleStates.PLAYERTURN:
                {
                    SpriteRenderer bg = backGround.transform.GetChild(1).GetComponent<SpriteRenderer>();
                    if (backGroundAlpha > 0f)
                    {
                        backGroundAlpha -= 0.1f;
                        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, backGroundAlpha);
                    }

                    startingGameUI.transform.GetChild(3).gameObject.SetActive(true); // 플레이어 턴 종료 버튼
                    startingGameUI.transform.GetChild(4).gameObject.SetActive(false); // 적턴 진행 중 문구
                    //startingGameUI.transform.GetChild(5).gameObject.SetActive(true); // undo
                    //startingGameUI.transform.GetChild(6).gameObject.SetActive(true); // 턴 리셋
                }
                break;
        }
    }

    public static UIControl GetInst()
    {
        return Inst;
    }

    public void GameWin()
    {
        GameObject winUI = GameObject.Find("TitleCanvas").transform.GetChild(1).gameObject;
        GameObject profile = winUI.transform.GetChild(2).gameObject;
        int cityzen = 0;
        int time = (int)Time.time - (int)TurnBaseBattleManager.GetInst().startTime;

        foreach (GameObject Object in TurnBaseBattleManager.GetInst().objectList)
        {
            if (Object.name == "Building_0")
            {
                cityzen += 50;
            }
            else if (Object.name == "Building_1")
            {
                if (Object.GetComponent<Building>().health == 2)
                {
                    cityzen += 100;
                }
                else if (Object.GetComponent<Building>().health == 1)
                {
                    cityzen += 50;
                }
            }
        }
        winUI.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<Text>().text = cityzen.ToString();
        winUI.transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<Text>().text = time / 3600 + "시간 " + (time % 3600) / 60 + "분 " + ((time % 3600) % 60) % 60 + "초";

        // 플레이어 1
        profile.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("charter/anmator/mech_" + (DataBase.playerMech[0] + 1));
        profile.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[0] + 1) + "/Mech_" + (DataBase.playerMech[0] + 1));
        if (GameObject.Find("Player0").GetComponent<Player>().health == 0)
        {
            profile.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "무력화됨";
            profile.transform.GetChild(0).GetChild(2).GetComponent<Text>().color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            profile.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "생존";
            profile.transform.GetChild(0).GetChild(2).GetComponent<Text>().color = new Color(0f, 1f, 0f, 1f);
        }

        // 플레이어 2
        profile.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("charter/anmator/mech_" + (DataBase.playerMech[1] + 1));
        profile.transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[1] + 1) + "/Mech_" + (DataBase.playerMech[1] + 1));
        if (GameObject.Find("Player1").GetComponent<Player>().health == 0)
        {
            profile.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "무력화됨";
            profile.transform.GetChild(1).GetChild(2).GetComponent<Text>().color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            profile.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = "생존";
            profile.transform.GetChild(1).GetChild(2).GetComponent<Text>().color = new Color(0f, 1f, 0f, 1f);
        }

        // 플레이어 3
        profile.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("charter/anmator/mech_" + (DataBase.playerMech[2] + 1));
        profile.transform.GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[2] + 1) + "/Mech_" + (DataBase.playerMech[2] + 1));
        if (GameObject.Find("Player2").GetComponent<Player>().health == 0)
        {
            profile.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "무력화됨";
            profile.transform.GetChild(2).GetChild(2).GetComponent<Text>().color = new Color(1f, 0f, 0f, 1f);
        }
        else
        {
            profile.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "생존";
            profile.transform.GetChild(2).GetChild(2).GetComponent<Text>().color = new Color(0f, 1f, 0f, 1f);
        }

        winUI.SetActive(true);
    }

    public void GameLose()
    {
        GameObject loseUI = GameObject.Find("TitleCanvas").transform.GetChild(2).gameObject;
        int cityzen = 0;
        int time = (int)Time.time - (int)TurnBaseBattleManager.GetInst().startTime; 

        foreach (GameObject Object in TurnBaseBattleManager.GetInst().objectList)
        {
            if (Object.name == "Building_0")
            {
                cityzen += 50;
            }
            else if (Object.name == "Building_1")
            {
                cityzen += 100;
            }
        }
        loseUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = cityzen.ToString();
        loseUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = time / 3600 + "시간 " + (time % 3600) / 60 + "분 " + ((time % 3600) % 60) % 60 + "초";

        loseUI.SetActive(true);
    }

}