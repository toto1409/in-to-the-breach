using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfo : MonoBehaviour
{

    public static ProfileInfo Inst = null;
    public const int PLAYER = 0;
    public const int ENEMY = 1;

    public string MechName;
    public int id;
    public int movement;
    public bool Holding;
    public TurnBaseBattleManager tm;
    public ClickManager cm;
    public bool clickFlag = false;
    public bool holdInterface = false;
    public bool enemyActive;
    public GameObject unitStatus;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        tm = TurnBaseBattleManager.GetInst();
        cm = ClickManager.GetInst();
    }

    void Update()
    {
        if (tm.currentState != TurnBaseBattleManager.BattleStates.ENEMYTURN &&
           tm.currentState != TurnBaseBattleManager.BattleStates.PLAYERPOSITIONSET)
        {
            if (ClickManager.GetInst().obj && ClickManager.GetInst().obj.tag == "Character")
            {
                if (ClickManager.GetInst().obj.GetComponent<Player>().onWater)
                {
                    this.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                }
            }

            if (!clickFlag && UIControl.GetInst().proflieMiddlePick == false) // 마우스 오버 했을 때 프로필 출력
            {
                if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null)
                {
                    ClearProfile(); // 유닛 프로필 사진 출력 클리어

                    GameObject temp = MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y]; // 타일에 클릭한 오브젝트 객체를 참조

                    if (temp.tag == "Character")
                    {
                        if (temp.GetComponent<Player>().onWater)
                        {
                            this.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                        }

                        id = temp.GetComponent<Player>().playerID; // 캐릭터 id값 가져옴
                        movement = temp.GetComponent<Unit>().movement;

                        transform.GetChild(ProfileInfo.PLAYER).GetChild(id).gameObject.SetActive(true); // id에 해당하는 프로필 이미지 출력
                        GameObject.Find("weponUI").transform.GetChild(id).gameObject.SetActive(true); // id에 해당하는 무기 이미지 출력
                        GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(true); // 리페어 이미지 출력
                        this.transform.GetChild(2).gameObject.SetActive(true); // 플레이어 프로필 인터페이스 출력
                        this.transform.GetChild(3).gameObject.SetActive(false);
                        // 유닛 스테이터스 출력
                        unitStatus.SetActive(true);
                        unitStatus.transform.GetChild(0).GetComponent<Text>().text = temp.GetComponent<Player>().mechName;
                        unitStatus.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (id + 1) + "/Mech_" + (id + 1));
                        unitStatus.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Number/Highlight/" + movement + "_Highlight");
                        unitStatus.transform.GetChild(3).gameObject.SetActive(true);
                        unitStatus.transform.GetChild(4).gameObject.SetActive(false);
                    }
                    else if (temp.tag == "Enemy")
                    {
                        id = temp.GetComponent<Enemy>().enemyID; // 캐릭터 id값 가져옴
                        movement = temp.GetComponent<Unit>().movement;

                        transform.GetChild(ProfileInfo.ENEMY).GetChild(id).gameObject.SetActive(true); // id에 해당하는 프로필 이미지 출력
                        this.transform.GetChild(3).gameObject.SetActive(true); // 플레이어 프로필 인터페이스 출력
                        this.transform.GetChild(2).gameObject.SetActive(false);
                        // 유닛 스테이터스 출력
                        unitStatus.SetActive(true);
                        unitStatus.transform.GetChild(0).GetComponent<Text>().text = temp.GetComponent<Enemy>().enemyName;
                        unitStatus.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Enemy/StateImage/" + id);
                        unitStatus.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Number/Highlight/" + movement + "_Highlight");
                        unitStatus.transform.GetChild(3).gameObject.SetActive(false);
                        if(temp.GetComponent<Enemy>().fly)
                        {
                            unitStatus.transform.GetChild(4).gameObject.SetActive(true);
                        }
                        else
                        {
                            unitStatus.transform.GetChild(4).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (UIControl.GetInst().proflieMiddleMouseOn == false)
                    {
                        this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                        ClearProfile(); // 유닛 프로필 사진 출력 클리어
                        this.transform.GetChild(2).gameObject.SetActive(false);
                        this.transform.GetChild(3).gameObject.SetActive(false);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) && MapControl.isMouseIn && ClickManager.GetInst().attackOn == false) // 왼쪽 클릭
            {
                if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null &&
                    MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Character") // 캐릭터일 경우
                {
                    holdInterface = true;
                    clickFlag = true;
                    ClearProfile(); // 유닛 프로필 사진 출력 클리어

                    foreach (GameObject player in tm.playerList) // 클릭상태 해제
                    {
                        player.GetComponent<Player>().ClickOn = false;
                    }

                    GameObject temp = MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y]; // 타일에 클릭한 오브젝트 객체를 참조

                    id = temp.GetComponent<Player>().playerID; // 캐릭터 id값 가져옴
                    movement = temp.GetComponent<Player>().movement;

                    transform.GetChild(ProfileInfo.PLAYER).GetChild(id).gameObject.SetActive(true); // id에 해당하는 프로필 이미지 출력
                    GameObject.Find("weponUI").transform.GetChild(id).gameObject.SetActive(true); // id에 해당하는 무기 이미지 출력
                    GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(true); // 리페어 이미지 출력

                    temp.GetComponent<Unit>().ClickOn = true;
                    this.transform.GetChild(2).gameObject.SetActive(true); // 플레이어 프로필 인터페이스 출력
                    this.transform.GetChild(3).gameObject.SetActive(false); // 몬스터 프로필 인터페이스 비활성화

                    // 유닛 스테이터스 출력
                    unitStatus.SetActive(true);
                    unitStatus.transform.GetChild(0).GetComponent<Text>().text = temp.GetComponent<Player>().mechName;
                    unitStatus.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (id + 1) + "/Mech_" + (id + 1));
                    unitStatus.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Number/Highlight/" + movement + "_Highlight");
                    unitStatus.transform.GetChild(3).gameObject.SetActive(true);
                    unitStatus.transform.GetChild(4).gameObject.SetActive(false);
                }
                else if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null &&
                         MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Enemy") // 적군일 경우
                {
                    holdInterface = false;
                    clickFlag = true;
                    ClearProfile();

                    foreach (GameObject enemy in tm.enemyList) // 클릭상태 해제
                    {
                        enemy.GetComponent<Enemy>().ClickOn = false;
                    }

                    GameObject temp = MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y];

                    id = temp.GetComponent<Enemy>().enemyID;
                    movement = temp.GetComponent<Enemy>().movement;

                    transform.GetChild(ProfileInfo.ENEMY).GetChild(id).gameObject.SetActive(true); // id에 해당하는 프로필 이미지 출력

                    temp.GetComponent<Unit>().ClickOn = true;
                    this.transform.GetChild(3).gameObject.SetActive(true); // 몬스터 프로필 인터페이스 출력
                    this.transform.GetChild(2).gameObject.SetActive(false); // 플레이어 프로필 인터페이스 출력 비활성화

                    // 유닛 스테이터스 출력
                    unitStatus.SetActive(true);
                    unitStatus.transform.GetChild(0).GetComponent<Text>().text = temp.GetComponent<Enemy>().enemyName;
                    unitStatus.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Enemy/StateImage/" + id);
                    unitStatus.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Number/Highlight/" + movement + "_Highlight");
                    unitStatus.transform.GetChild(3).gameObject.SetActive(false);
                    if (temp.GetComponent<Enemy>().fly)
                    {
                        unitStatus.transform.GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        unitStatus.transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (!holdInterface)
                {
                    if (MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] == false) // 이동 범위 표시가 되어있지 않은 타일 일때
                    {
                        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] == null) // 클릭한 타일이 비어있을 경우
                        {
                            ClearProfile();
                            this.transform.GetChild(2).gameObject.SetActive(false); // 플레이어 프로필 인터페이스 출력 비활성화
                        }
                    }
                    else
                    {
                        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] == null) // 클릭한 타일이 비어있을 경우
                        {
                            if (cm.obj != null && cm.obj.tag == "Enemy")
                            {
                                ClearProfile();
                                this.transform.GetChild(2).gameObject.SetActive(false); // 플레이어 프로필 인터페이스 출력 비활성화
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) // 오른쪽 클릭
            {
                ClearProfile();
                this.transform.GetChild(2).gameObject.SetActive(false); // 플레이어 프로필 인터페이스 출력 비활성화
                clickFlag = false;
            }
        }
        else
        {
            if(!enemyActive)
            {
                ClearProfile(); // 유닛 프로필 사진 출력 클리어
                this.transform.GetChild(2).gameObject.SetActive(false);
                this.transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    public static ProfileInfo GetInst()
    {
        return Inst;
    }

    public void ClearProfile() // 유닛 프로필 사진 출력 클리어
    {
        if (UIControl.GetInst().proflieMiddlePick == false)
        {
            int playerCount, enemyCount, weaponCount;

            playerCount = transform.GetChild(ProfileInfo.PLAYER).GetChildCount();
            enemyCount = transform.GetChild(ProfileInfo.ENEMY).GetChildCount();
            weaponCount = GameObject.Find("weponUI").transform.GetChildCount();

            GameObject.Find("WeponButton").transform.GetChild(0).gameObject.SetActive(false);
            unitStatus.SetActive(false);

            for (int i = 0; i < playerCount; i++)
            {
                transform.GetChild(ProfileInfo.PLAYER).GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < enemyCount; i++)
            {
                transform.GetChild(ProfileInfo.ENEMY).GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < weaponCount; i++)
            {
                GameObject.Find("weponUI").transform.GetChild(i).gameObject.SetActive(false);
            }

            this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void enemyProfilePrint(GameObject _unit, int _id)
    {
        transform.GetChild(ProfileInfo.ENEMY).GetChild(_id).gameObject.SetActive(true); // id에 해당하는 프로필 이미지 출력
        this.transform.GetChild(3).gameObject.SetActive(true); // 몬스터 프로필 인터페이스 출력
        this.transform.GetChild(2).gameObject.SetActive(false); // 플레이어 프로필 인터페이스 출력 비활성화

        // 유닛 스테이터스 출력
        int movement = _unit.GetComponent<Unit>().movement;
        unitStatus.SetActive(true);
        unitStatus.transform.GetChild(0).GetComponent<Text>().text = _unit.GetComponent<Enemy>().enemyName;
        unitStatus.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Enemy/StateImage/" + _id);
        unitStatus.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/Number/Highlight/" + movement + "_Highlight");
        unitStatus.transform.GetChild(3).gameObject.SetActive(false);
        if (_unit.GetComponent<Enemy>().fly)
        {
            unitStatus.transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            unitStatus.transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    public void ClearPlayerInterFace()
    {
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void OnWaterPlayer()
    {
        this.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
    }

    public void OutOfWaterPlayer()
    {
        this.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    }
}
