using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartUI : MonoBehaviour
{
    public RectTransform rectTransform;  // UI 창의 RECT 컴포넌트를 받아올변수
    public Text title; // 제목을 저장할 변수
    public Text text; // 내용을 저장할 변수
    public GameObject mechImage;


    void Update()
    {
        if(PlayerPositionSet.GetInst().isDeployEnd && !PlayerPositionSet.GetInst().isReDeploy)
        {
            mechImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Player_Arrow");
            mechImage.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
            mechImage.GetComponent<RectTransform>().localScale = new Vector3(45f, 45f, 1f);
            title.text = "배치 완료";
            text.text = "승인하기 전에 위치를 변경할 수 있습니다";
        }
        else
        {
            mechImage.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            mechImage.GetComponent<RectTransform>().localScale = new Vector3(70f, 70f, 1f);

            switch (PlayerPositionSet.GetInst().curDeployCharNum)
            {
                case 0:
                    SetText(DataBase.playerMech[0]);
                    mechImage.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[0] + 1) + "/Mech_" + (DataBase.playerMech[0] + 1));
                    break;
                case 1:
                    SetText(DataBase.playerMech[1]);
                    mechImage.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[1] + 1) + "/Mech_" + (DataBase.playerMech[1] + 1));
                    break;
                case 2:
                    SetText(DataBase.playerMech[2]);
                    mechImage.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[2] + 1) + "/Mech_" + (DataBase.playerMech[2] + 1));
                    break;
            }
        }

        // UI 창의 크기를 Text 내용에 맞춰서 최적화
        rectTransform.sizeDelta = new Vector2(title.preferredWidth + 75f, title.preferredHeight + 37f);

        // UI 최소 가로 크기 지정
        if (rectTransform.sizeDelta.x < 230f)
        {
            rectTransform.sizeDelta = new Vector2(230f, rectTransform.sizeDelta.y);
        }
        // UI 최소 세로 크기 지정
        if (rectTransform.sizeDelta.y < 56f)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 56f);
        }
    }

    void SetText(int _num)
    {
        switch(_num)
        {
            case Player.Combat_Mech:
                title.text = "컴뱃 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
            case Player.Artillery_Mech:
                title.text = "자주포 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
            case Player.Cannon_Mech:
                title.text = "캐논 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
            case 3:
                title.text = "방패 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
            case 4:
                title.text = "D.VA 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
            case 5:
                title.text = "K-9 메크 배치";
                text.text = "노란 배치 지역에서 위치를 선택해주세요";
                break;
        }
    }
}