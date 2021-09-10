using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionSet : MonoBehaviour
{
    public static PlayerPositionSet Inst = null;

    // PLAYERPOSITIONSET 관련 변수
    public GameObject selectplayerMechUI;                // 선택된 플레이어 캐릭터를 출력할 UI 변수
    public GameObject[] playerMechUI = new GameObject[3];  // 플레이어 캐릭터를 출력할 UI 변수
    public Sprite[] PlayerMechImg = new Sprite[3];     // 출력할 플레이어 캐릭터 스트라이트 변수
    public Sprite[] arrowImg = new Sprite[3];          // 타일 상태를 알려줄 화살표 이미지 변수
    public int curDeployCharNum;                  // 시작 위치를 선택해야하는 캐릭터의 번호
    public bool isDeployEnd = false;               // 3개의 캐릭터 모두 배치하였는지 체크하는 변수
    public bool isReDeploy = false;                // 배치한 캐릭터를 재배치하는지 체크라는 변수
    public int reDeployIndex;                     // 재배치하려는 캐릭터의 인덱스 번호값 변수
    public Vector2[] deployPos = new Vector2[3];        // 배치된 캐릭터의 Cell 인덱스 값을 저장하는 변수
    public Vector2 prevPos = new Vector2();  // selectplayerMechUI의 이전 좌표값을 저장하는 변수
    public bool deployDone = false;
    public bool startBattlePressed;
    TurnBaseBattleManager turnBaseBattleManager;
    public Sound s;


    private void Awake()
    {
        Inst = this;

        selectplayerMechUI = GameObject.Find("UI").transform.Find("SelectPlayerMechImg").transform.gameObject;
        for (int i = 0; i < 3; i++)
        {
            playerMechUI[i] = GameObject.Find("UI").transform.Find("PlayerMechImg" + i).transform.gameObject;
            PlayerMechImg[i] = Resources.Load<Sprite>("Player/Mech_" + (DataBase.playerMech[i] + 1) + "/Mech_" + (DataBase.playerMech[i] + 1));
        }

        arrowImg[0] = Resources.Load<Sprite>("UI/Player_Arrow");
        arrowImg[1] = Resources.Load<Sprite>("UI/Player_Arrow2");
        arrowImg[2] = Resources.Load<Sprite>("UI/Player_Arrow3");
    }

    private void Start()
    {
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
        turnBaseBattleManager = GetComponent<TurnBaseBattleManager>();

        selectplayerMechUI.transform.GetChild(0).
            GetComponent<SpriteRenderer>().sprite = PlayerMechImg[curDeployCharNum];

        for (int i = 0; i < 3; i++)
        {
            playerMechUI[i].transform.GetChild(0).
                GetComponent<SpriteRenderer>().sprite = PlayerMechImg[i];
        }
    }

    public static PlayerPositionSet GetInst()
    {
        return Inst;
    }

    // 플레이어 캐릭터 배치 범위 표시
    public void SelectablePlayerPosView()
    {
        if (!deployDone)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // 재배치 모드 일 경우
                    if (isReDeploy)
                    {
                        // 배치 범위 내에만 표시
                        if (i >= 1 && i <= 6 && j >= 1 && j <= 3)
                        {
                            // 마우스 포인터가 위치하는 타일의 색상 변경
                            if (MapControl.Crt_X == i && MapControl.Crt_Y == j)
                            {
                                ColorChange(i, j, "GREEN");
                            }
                            // 배치 가능한 타일일 경우
                            else if (MapControl.MapTileArray[i, j].GetComponent<MapTile>().Moveable == true ||
                                     MapControl.MapObjectArray[i, j].tag == "Character")
                            {
                                ColorChange(i, j, "YELLOW_T");
                            }
                            else  // 배치 불가능한 타일일 경우 색상 초기화
                            {
                                ColorChange(i, j, "RESET");
                            }
                        }
                        else
                        {
                            if (MapControl.MapObjectArray[i, j] != null && MapControl.MapObjectArray[i, j].tag == "Enemy")
                            {
                                ColorChange(i, j, "ENEMY");
                            }
                            else if (MapControl.Crt_X == i && MapControl.Crt_Y == j)
                            {
                                ColorChange(i, j, "RED");
                            }
                            else
                            {
                                ColorChange(i, j, "RESET");
                            }
                        }
                    }

                    // 배치 완료 상태일 경우
                    else if (isDeployEnd)
                    {
                        if (i >= 1 && i <= 6 && j >= 1 && j <= 3)  // 배치 가능 범위 안에만 표시
                        {
                            // 배치 가능한 타일일 경우
                            if (MapControl.MapTileArray[i, j].GetComponent<MapTile>().Moveable == true ||
                                MapControl.MapObjectArray[i, j].tag == "Character")
                            {
                                ColorChange(i, j, "YELLOW_T");
                            }
                            else // 배치 불가능한 타일일 경우
                            {
                                ColorChange(i, j, "RESET");
                            }
                        }
                        else
                        {
                            if (MapControl.MapObjectArray[i, j] != null && MapControl.MapObjectArray[i, j].tag == "Enemy")
                            {
                                ColorChange(i, j, "ENEMY");
                            }
                            else
                            {
                                ColorChange(i, j, "RESET");
                            }
                        }
                    }

                    // 배치 모드 일 경우
                    else
                    {
                        // 배치 범위 내에만 표시
                        if (i >= 1 && i <= 6 && j >= 1 && j <= 3)
                        {
                            if (MapControl.Crt_X == i && MapControl.Crt_Y == j)
                            {
                                ColorChange(i, j, "YELLOW");
                            }
                            // 배치 가능한 타일일 경우
                            else if (MapControl.MapTileArray[i, j].GetComponent<MapTile>().Moveable == true ||
                                     MapControl.MapObjectArray[i, j].tag == "Character")
                            {
                                ColorChange(i, j, "YELLOW_T");
                            }
                            else
                            {
                                ColorChange(i, j, "RESET");
                            }
                        }
                        else
                        {
                            if (MapControl.Crt_X == i && MapControl.Crt_Y == j)
                            {
                                if (MapControl.MapObjectArray[i, j] != null && MapControl.MapObjectArray[i, j].tag == "Enemy")
                                {
                                    ColorChange(i, j, "ENEMY");
                                }
                                else
                                {
                                    ColorChange(i, j, "RED");
                                }
                            }
                            else
                            {
                                if (MapControl.MapObjectArray[i, j] != null && MapControl.MapObjectArray[i, j].tag == "Enemy")
                                {
                                    ColorChange(i, j, "ENEMY");
                                }
                                else
                                {
                                    ColorChange(i, j, "RESET");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // 플레이어 캐릭터를 배치
    public void DeployPlayer()
    {
        // 맵 안에 마우스가 위치하는지 확인하여 배치유닛 UI를 활성/비활성화 한다.
        if (!deployDone)
        {
            if (MapControl.isMouseIn)
            {
                if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null)
                {
                    if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Enemy")
                    {
                        selectplayerMechUI.gameObject.SetActive(false);
                    }
                }
                else
                {
                    selectplayerMechUI.gameObject.SetActive(true);
                }
            }
            else
            {
                selectplayerMechUI.gameObject.SetActive(false);
                ColorChange(MapControl.Crt_X, MapControl.Crt_Y, "RESET");
            }
        }

        // 배치 영역 안이 아닐 경우 마지막으로 SelectUI가 있던 자리의 오브젝트 레퍼런스 값을 null 로 초기화
        if (IsSelectiveRange(MapControl.Crt_X, MapControl.Crt_Y) == false)
        {
            if (MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] != null)
            {
                if (MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y].tag != "Obstacle" &&
                    MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y].tag != "Character")
                {
                    MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] = null;
                }
            }
        }

        // 3개의 캐릭터가 모두 배치되었을 경우
        if (isDeployEnd)
        {
            //스페이스 바를 누르면 다음 단계로 넘어감
            if (Input.GetButtonDown("Jump") && !startBattlePressed)
            {
                // 배치 단계를 끝내고 전투를 시작함
                startBattlePressed = true;
                StartBattle();
            }

            // 오른쪽 마우스를 클릭하면 재배치를 취소
            if (Input.GetMouseButtonDown(1) && !startBattlePressed)
            {
                CancelReDeploy();
            }

            // 재배치 모드의 경우
            if (isReDeploy)
            {
                for (int i = 0; i < 3; i++)
                {
                    playerMechUI[i].gameObject.transform.position = MapControl.Cell[(int)deployPos[i].x, (int)deployPos[i].y];
                    playerMechUI[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[0];
                }

                // 재배치 가능 범위 안인지 확인
                if (IsSelectiveRange(MapControl.Crt_X, MapControl.Crt_Y))
                {
                    // 타일이 재배치 가능한 상태의 타일 인지 확인
                    if (MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().Moveable == true)
                    {
                        selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 1;
                        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[0];
                        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                        selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];

                        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<Outline>().color = 1;
                        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
                        playerMechUI[reDeployIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 0.3f);

                        if (MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] != null &&
                        MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y].name == "SelectPlayerMechImg")
                        {
                            MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] = null;
                        }

                        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] == null)
                        {
                            MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = selectplayerMechUI;
                            prevPos.x = MapControl.Crt_X;
                            prevPos.y = MapControl.Crt_Y;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            MapControl.MapObjectArray[(int)deployPos[reDeployIndex].x, (int)deployPos[reDeployIndex].y] = null;

                            playerMechUI[reDeployIndex].gameObject.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
                            deployPos[reDeployIndex].x = MapControl.Crt_X;
                            deployPos[reDeployIndex].y = MapControl.Crt_Y;
                            MapControl.MapObjectArray[(int)deployPos[reDeployIndex].x, (int)deployPos[reDeployIndex].y] = playerMechUI[reDeployIndex];

                            CancelReDeploy();
                            s.SoundPlay("EffectSound/ui_battle_preplan_place_mech_01");
                        }

                    }
                    // 이미 타일에 아군 유닛이 배치되있는 경우 서로 위치를 교환함
                    else if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Character")
                    {
                        int index = -1;

                        selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 1;
                        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[2];
                        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                        selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];

                        for (int i = 0; i < 3; i++)
                        {
                            if ((int)deployPos[i].x == MapControl.Crt_X && (int)deployPos[i].y == MapControl.Crt_Y)
                            {
                                index = i;
                            }
                        }

                        playerMechUI[index].gameObject.transform.position = MapControl.Cell[(int)deployPos[reDeployIndex].x, (int)deployPos[reDeployIndex].y];
                        playerMechUI[index].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[2];

                        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
                        playerMechUI[reDeployIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.0f);


                        if (Input.GetMouseButtonDown(0))
                        {
                            playerMechUI[index].gameObject.transform.position = MapControl.Cell[(int)deployPos[reDeployIndex].x, (int)deployPos[reDeployIndex].y];
                            deployPos[index].x = (int)deployPos[reDeployIndex].x;
                            deployPos[index].y = (int)deployPos[reDeployIndex].y;
                            MapControl.MapObjectArray[(int)deployPos[index].x, (int)deployPos[index].y] = playerMechUI[index];

                            MapControl.MapTileArray[(int)deployPos[index].x, (int)deployPos[index].y].GetComponent<MapTile>().Moveable = false;

                            playerMechUI[curDeployCharNum].gameObject.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
                            deployPos[reDeployIndex].x = MapControl.Crt_X;
                            deployPos[reDeployIndex].y = MapControl.Crt_Y;
                            MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = playerMechUI[curDeployCharNum];

                            CancelReDeploy();
                        }

                    }
                    else
                    {
                        // 재배치 모드에서 부적절한 타일 처리
                        ReDeploy_IneligibleTile();
                    }

                }
                else
                {
                    ReDeploy_IneligibleTile();
                }
            }
            // 배치완료 상태에서 재배치 모드가 아닐경우
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 타일에 아군 캐릭터가 위치해 있는지 확인
                    if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null &&
                        MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Character")
                    {
                        reDeployIndex = -1;

                        for (int i = 0; i < 3; i++)
                        {
                            if ((int)deployPos[i].x == MapControl.Crt_X && (int)deployPos[i].y == MapControl.Crt_Y)
                            {
                                reDeployIndex = i;
                                curDeployCharNum = i;
                                MapControl.MapTileArray[(int)deployPos[i].x, (int)deployPos[i].y].GetComponent<MapTile>().Moveable = true;
                                isReDeploy = true;
                            }
                        }

                        if (reDeployIndex == -1)
                        {
                            return;
                        }

                        selectplayerMechUI.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerMechImg[reDeployIndex];
                    }

                }
            }

        }
        // 3개의 캐릭터 모두 배치되지 않았을 경우
        else
        {
            // 배치 가능 범위 안인지 확인
            if (IsSelectiveRange(MapControl.Crt_X, MapControl.Crt_Y))
            {
                for (int i = 0; i < 3; i++)
                {
                    playerMechUI[i].transform.GetChild(0).GetComponent<Outline>().eraseRenderer = false;
                    playerMechUI[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1.0f);
                    playerMechUI[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 1.0f);
                }

                // 타일이 배치 가능한 상태의 타일 인지 확인
                if (MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().Moveable == true)
                {
                    selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 2;
                    selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[0];
                    selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
                    selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];

                    if (MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] != null &&
                        MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y].name == "SelectPlayerMechImg")
                    {
                        MapControl.MapObjectArray[(int)prevPos.x, (int)prevPos.y] = null;
                    }

                    if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] == null)
                    {
                        MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = selectplayerMechUI;
                        prevPos.x = MapControl.Crt_X;
                        prevPos.y = MapControl.Crt_Y;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        s.SoundPlay("EffectSound/ui_battle_preplan_place_mech_01");
                        playerMechUI[curDeployCharNum].gameObject.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
                        playerMechUI[curDeployCharNum].gameObject.SetActive(true);

                        // 맵 오브젝트 참조 배열에 할당
                        MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = playerMechUI[curDeployCharNum];

                        // 타일의 상태를 이동 가능한 상태로 변경
                        MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().Moveable = false;

                        deployPos[curDeployCharNum].x = MapControl.Crt_X;
                        deployPos[curDeployCharNum].y = MapControl.Crt_Y;

                        // 이미 배치가 되어있다면 배치되지 않은 캐릭터 인덱스까지 이동함
                        while (playerMechUI[curDeployCharNum].activeSelf == true)
                        {
                            curDeployCharNum++;

                            // 3개의 캐릭터 모두 배치되었다면 캐릭터 배치 UI를 비활성화 시키고 return
                            if (curDeployCharNum == 3)
                            {
                                isDeployEnd = true;
                                selectplayerMechUI.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                                selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;

                                return;
                            }
                        }

                        // 전부 배치되지 않았으면 다음 배치해야하는 캐릭터의 스프라이트로 변경
                        selectplayerMechUI.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerMechImg[curDeployCharNum];
                    }

                }
                // 이미 타일에 아군 유닛이 배치되있는 경우
                else if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Character")
                {
                    int index = -1;

                    selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 2;
                    selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[2];
                    selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
                    selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];

                    for (int i = 0; i < 3; i++)
                    {
                        if ((int)deployPos[i].x == MapControl.Crt_X && (int)deployPos[i].y == MapControl.Crt_Y)
                        {
                            index = i;
                        }
                    }

                    playerMechUI[index].transform.GetChild(0).GetComponent<Outline>().eraseRenderer = true;
                    playerMechUI[index].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
                    playerMechUI[index].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 0.0f);

                    // 이미 배치되어 있는 아군 오브젝트와 교환함
                    if (Input.GetMouseButtonDown(0))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (playerMechUI[i].activeSelf == true)
                            {
                                if ((int)deployPos[i].x == MapControl.Crt_X && (int)deployPos[i].y == MapControl.Crt_Y)
                                {
                                    playerMechUI[i].gameObject.SetActive(false);
                                    deployPos[i].x = -1;
                                    deployPos[i].y = -1;

                                    playerMechUI[curDeployCharNum].gameObject.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
                                    playerMechUI[curDeployCharNum].gameObject.SetActive(true);
                                    deployPos[curDeployCharNum].x = MapControl.Crt_X;
                                    deployPos[curDeployCharNum].y = MapControl.Crt_Y;

                                    // 맵 오브젝트 참조 배열에 할당
                                    MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = playerMechUI[curDeployCharNum];

                                    curDeployCharNum = i;
                                    selectplayerMechUI.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerMechImg[i];
                                    break;
                                }
                            }
                        }
                    }

                }
                // 배치 가능한 상태의 타일이 아닐 경우
                else
                {
                    // 배치 모드에서 부적절한 타일 처리
                    Deploy_IneligibleTile();
                }

            }
            // 배치 가능한 범위가 아닐 경우
            else
            {
                Deploy_IneligibleTile();
            }
        }

    }

    // 재배치 모드 취소
    void CancelReDeploy()
    {
        selectplayerMechUI.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
        MapControl.MapTileArray[(int)deployPos[reDeployIndex].x, (int)deployPos[reDeployIndex].y].GetComponent<MapTile>().Moveable = false;

        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<Outline>().color = 2;
        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        playerMechUI[reDeployIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 1f);

        for (int i = 0; i < 3; i++)
        {
            playerMechUI[i].gameObject.transform.position = MapControl.Cell[(int)deployPos[i].x, (int)deployPos[i].y];
            playerMechUI[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[0];
        }

        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null)
        {
            if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].name == "SelectPlayerMechImg")
            {
                MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] = null;
            }
        }

        isReDeploy = false;
    }

    // 배치 모드에서 부적절한 타일 처리
    void Deploy_IneligibleTile()
    {
        selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 0;
        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[1];
        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
    }

    // 재배치 모드에서 부적절한 타일 처리
    void ReDeploy_IneligibleTile()
    {
        selectplayerMechUI.transform.GetChild(0).GetComponent<Outline>().color = 0;
        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = arrowImg[1];
        selectplayerMechUI.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        selectplayerMechUI.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];

        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<Outline>().color = 0;
        playerMechUI[reDeployIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
        playerMechUI[reDeployIndex].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.3f);
    }

    bool IsSelectiveRange(int _index1, int _index2)
    {
        if (_index1 >= 1 && _index1 <= 6 && _index2 >= 1 && _index2 <= 3)
        {
            return true;
        }
        return false;
    }

    // 배치 단계를 끝내고 전투를 시작함
    public void StartBattle()
    {
        deployDone = true;

        Destroy(selectplayerMechUI);
        selectplayerMechUI = null;

        for (int i = 0; i < 3; i++)
        {
            Destroy(playerMechUI[i]);
            playerMechUI[i] = null;
        }

        for (int i = 1; i < 7; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                ColorChange(i, j, "RESET");
            }
        }

        GameObject.Find("BattleStartUI").SetActive(false);
        for (int i = 0; i < 3; i++) 
        {
            // 메뉴 버튼, 전력 바, 중간 프로필 인터페이스 출력
            GameObject.Find("startingGameUI").transform.GetChild(i).gameObject.SetActive(true);

            // Mech(Player) 오브젝트 생성
            GameObject playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
            GameObject player = MonoBehaviour.Instantiate(playerPrefab) as GameObject;
            player.transform.parent = GameObject.Find("Player").transform;
            player.transform.position = new Vector3(8f, 0f, 0f);
            player.name = "Player" + i;
            player.transform.GetChild(Player.IMAGE).GetChild(DataBase.playerMech[i]).gameObject.SetActive(true);
            player.GetComponent<Player>().playerID = DataBase.playerMech[i];
            player.GetComponent<Unit>().unitID = DataBase.playerMech[i];
            player.GetComponent<Player>().mechName = SetMechName(DataBase.playerMech[i]);
            player.GetComponent<Player>().SetPlayerStat();
        }

        foreach(GameObject building in TurnBaseBattleManager.GetInst().objectList)
        {
            GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
            GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
            tooltip.GetComponent<ToolTip>().mode = "StartToolTip";
            tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
            tooltip.transform.position = new Vector2(building.transform.position.x, building.transform.position.y + 0.3f);
            tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }

        StartCoroutine(SetPlayer());
    }

    IEnumerator SetPlayer()
    {
        for (int i = 0; i < 3; i++)
        {
            // 생성한 Mech(Player) 오브젝트 배치
            GameObject player = GameObject.Find("Player").transform.GetChild(i).gameObject;
            player.SetActive(true);
            player.transform.GetChild(Player.IMAGE).GetChild(DataBase.playerMech[i]).GetComponent<Animator>().SetBool("isAppear", true);
            player.transform.position = MapControl.Cell[(int)deployPos[i].x, (int)deployPos[i].y];
            MapControl.MapObjectArray[(int)deployPos[i].x, (int)deployPos[i].y] = player;

            // 등장 이펙트 출력
            GameObject appearEffectPrefab = Resources.Load("Prefabs/Effect") as GameObject;
            GameObject appearEffect = MonoBehaviour.Instantiate(appearEffectPrefab) as GameObject;
            appearEffect.transform.position = MapControl.Cell[(int)deployPos[i].x, (int)deployPos[i].y];
            appearEffect.transform.GetChild(0).gameObject.SetActive(true);

            yield return new WaitForSeconds(1.0f);
        }
        turnBaseBattleManager.currentState = TurnBaseBattleManager.BattleStates.ENEMYTURN;
    }

    // <타일 색상 변경>  인덱스1, 인덱스2 번째 타일의 색상을 해당 값으로 바꿉니다.
    void ColorChange(int _index1, int _index2, string _color)
    {
        switch (_color)
        {
            // 기본 색상
            case "YELLOW":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f, 0.4f);
                break;
            case "RED":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 0.4f);
                break;
            case "GREEN":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 0.4f);
                break;

            // 연한 색상
            case "YELLOW_T":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.6f, 0.4f);
                break;

            // 적이 위치한 타일의 색상
            case "ENEMY":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(0.53f, 0.64f, 0.81f, 0.42f);
                break;

            // 색상 초기화
            case "RESET":
                MapControl.MapTileArray[_index1, _index2].transform.GetChild(1).
                    GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                break;
        }

    }

    string SetMechName(int _id)
    {
        string name = "";
        switch(_id)
        {
            case 0:
                name = "컴뱃 메크";
                break;
            case 1:
                name = "자주포 메크";
                break;
            case 2:
                name = "캐논 메크";
                break;
            case 3:
                name = "방패 메크";
                break;
            case 4:
                name = "D.VA 메크";
                break;
            case 5:
                name = "K-9 메크";
                break;
        }
        return name;
    }

}
