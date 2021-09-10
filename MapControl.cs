using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <변경점>
//  맵 생성할 때 타일에 있는 Forest도 생성하도록 업데이트

public class MapControl : MonoBehaviour
{
    // 셀 관련
    public static Vector3[,] Cell = new Vector3[8, 8];              // 셀 시작 좌표값 배열
    private float Cell_Xsize = 1.0f;                                // 셀 가로 픽셀값
    private float Cell_Ysize = 0.74f;                               // 셀 세로 픽셀값
    public static int Crt_X, Crt_Y;                                 // 셀 인덱스 값 변수
    public Vector2[,] Cell_Index = new Vector2[8, 8];               // 셀 x,y 인덱스 저장 배열.
    public Vector3[,] Cell_Center_Pos = new Vector3[8, 8];          // 셀의 땅바닥부분의 가운데 좌표 x,y,z

    // 맵 타일 관련
    public static GameObject[,] MapTileArray = new GameObject[8, 8];     // 맵 타일의 레퍼런스를 저장하는 배열
    public static GameObject[,] MapObjectArray = new GameObject[8, 8];   // 맵 위에 있는 오브젝트의 레퍼런스를 저장하는 배열
    public int[,] MapTileNameArray = new int[8, 8];                      // 맵의 타일 종류를 저장하는 스트링 배열
    public int[,] MapObjectNameArray = new int[8, 8];                    // 맵의 오브젝트 종류를 저장하는 스트링 배열

    // 마우스 관련
    public static Vector2 mouse_pos = new Vector2(0, 0);     // 마우스 좌표값
    public Transform crt_mouse_pos_img;               // 현재 마우스위치 셀 표시 이미지 변수
    public static bool isMouseIn = false;             // 마우스가 맵안에 있는지 확인하는 변수

    public static bool[,] MoveState = new bool[8, 8];
    public static bool[,] MoveState2 = new bool[8, 8];
    public static int[,] MoveState3 = new int[8, 8];
    public static bool[,] AttackState = new bool[8, 8];
    public static bool[,] EnemyTargeted = new bool[8, 8];

    TurnBaseBattleManager tm;
    public int MapID;

    public static Vector2 EnemyPosBackUp;
    public static Vector2 PlayerPosBackUp;

    private void Awake()
    {
        tm = TurnBaseBattleManager.GetInst();

        // 오브젝트 변수에 오브젝트 할당
        crt_mouse_pos_img = GameObject.Find("UI").transform.Find("mouseover").transform;

        // 셀 초기 위치 설정
        InitializeCellPos();

        // 맵 타일 설정
        MapID = DataBase.playMapID;
        InitializeMapTile(MapID);

        // 맵 타일 배치
        SetMapTile();
    }

    private void Update()
    {
        // 마우스가 맵 안에 있는지 확인
        CheakMouseInsideMap();

        if (tm.currentState != TurnBaseBattleManager.BattleStates.PLAYERPOSITIONSET)
        {
            // 맵 타일 색 변환 체크
            MapTileColorCheak();
        }
    }


    // 마우스가 맵 안에 있는지 확인
    void CheakMouseInsideMap()
    {
        if (isMouseIn)
        {
            crt_mouse_pos_img.gameObject.SetActive(true);

            // 현재 셀 위치에 이미지를 표시시키는 함수
            CurrentCellPos();
        }
        else
        {
            crt_mouse_pos_img.gameObject.SetActive(false);
        }
    }

    // 현재 셀 위치에 이미지를 표시시키는 함수
    void CurrentCellPos()
    {
        Crt_X = (int)((mouse_pos.x / (Cell_Xsize / 2) + mouse_pos.y / (Cell_Ysize / 2)) / -2);
        Crt_Y = (int)((mouse_pos.y / (Cell_Ysize / 2) - (mouse_pos.x / (Cell_Xsize / 2))) / -2);

        // 타일 인덱스 최대값을 벗어나지 않도록 체크하여 벗어날 경우 최대값으로 설정함
        if (Crt_X > 7) { Crt_X = 7; }
        if (Crt_Y > 7) { Crt_Y = 7; }

        Vector3 temp = new Vector3(Cell[Crt_X, Crt_Y].x, Cell[Crt_X, Crt_Y].y, Cell[Crt_X, Crt_Y].z - 0.1f);

        // 맵 타일의 상태가 Water일 경우 마우스 이미지 위치를 조절함
        if (MapTileArray[Crt_X, Crt_Y].GetComponent<MapTile>().Water == true)
        {
            temp.y -= 0.132f;
        }

        crt_mouse_pos_img.position = temp;
    }

    // 각 셀의 초기 좌표값을 배열에 저장시키는 함수
    void InitializeCellPos()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Cell[i, j].x = (-i + j) * (Cell_Xsize / 2);
                Cell[i, j].y = (-i - j) * (Cell_Ysize / 2);
                // z값을 순서대로 설정하여 랜더링 순서를 정해줌
                Cell[i, j].z = ((7 - i) * 10) + (7 - j);
                //cell마다 인덱스값 넣기
                Cell_Index[i, j] = new Vector2(i, j);
                // 셀의 센터 넣기.
                Cell_Center_Pos[i, j] = new Vector3(Cell[i, j].x, Cell[i, j].y - 0.365f, Cell[i, j].z);
            }
        }
    }

    // 해당하는 맵 타일의 프리팹을 생성하여 배치시키는 함수
    void SetMapTile()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject tilePrefab = Resources.Load("Prefabs/MapTile") as GameObject; // 프리팹 리소스를 불러옴
                GameObject tile = MonoBehaviour.Instantiate(tilePrefab) as GameObject; // 불러온 리소스를 토대로 프리팹을 생성
                tile.transform.parent = GameObject.Find("Map").transform; // 생성된 프리팹을 부모 오브젝트에 넣음
                tile.transform.position = Cell[i, j]; // 생성된 프리팹의 위치 설정
                tile.name = "MapTile_" + i + "_" + j; // 생성된 프리팹의 이름 변경
                tile.transform.GetChild(MapTile.IMAGE).GetChild(MapTileNameArray[i, j] % 100).gameObject.SetActive(true);
                // 맵 타일 객체의 string type 값에 타일 종류의 이름을 저장
                tile.GetComponent<MapTile>().type = tile.transform.GetChild(MapTile.IMAGE).GetChild(MapTileNameArray[i, j] % 100).name;

                // 맵 이미지의 태그가 Water 일 경우 맵 타일의 상태값을 Water로 설정
                if (tile.transform.GetChild(MapTile.IMAGE).GetChild(MapTileNameArray[i, j] % 100).tag == "Water")
                {
                    tile.GetComponent<MapTile>().Water = true;
                    tile.transform.GetChild(MapTile.RGB).position = new Vector3(tile.transform.GetChild(MapTile.RGB).position.x,
                        tile.transform.GetChild(MapTile.RGB).position.y - 0.132f, tile.transform.GetChild(MapTile.RGB).position.z + 0.2f);
                    if (MapTileNameArray[i, j] >= 7)
                    {
                        tile.GetComponent<MapTile>().type = "SandWater";
                    }
                    else
                    {
                        tile.GetComponent<MapTile>().type = "Water";
                    }
                }

                // 맵 타일에 Forest가 존재할 경우
                if (MapTileNameArray[i, j] >= 100)
                {
                    tile.transform.GetComponent<MapTile>().Forest = true;
                    tile.transform.GetChild(MapTile.FOREST).GetChild(MapTileNameArray[i, j] % 100).gameObject.SetActive(true);
                }

                tile.GetComponent<MapTile>().indexX = i; // 타일 객체 자신의 좌표 인덱스 값을 저장함
                tile.GetComponent<MapTile>().indexY = j;
                MapTileArray[i, j] = tile;  // 맵타일의 레퍼런스를 배열에 저장해둠

                MoveState[i, j] = false;
                MoveState2[i, j] = false;
                MoveState3[i, j] = 0;
                AttackState[i, j] = false;

                if (MapObjectNameArray[i, j] != MapObject.Empty)  // 맵 타일 오브젝트 값이 Empty(비어있음)이 아닐 경우 
                {
                    GameObject tileObjectPrefab = Resources.Load("Prefabs/MapObject") as GameObject; // 프리팹 리소스를 불러옴
                    GameObject tileObject = MonoBehaviour.Instantiate(tileObjectPrefab) as GameObject; // 불러온 리소스를 토대로 프리팹을 생성
                    tileObject.transform.parent = GameObject.Find("MapObject").transform; // 생성된 프리팹을 부모 오브젝트에 넣음
                    tileObject.transform.position = Cell[i, j]; // 생성된 프리팹의 위치 설정

                    // 맵 오브젝트 객체의 string type 값에 오브젝트 종류의 이름을 저장
                    tileObject.GetComponent<MapObject>().typeID = MapObjectNameArray[i, j];
                    tileObject.GetComponent<MapObject>().type = tileObject.transform.GetChild(MapObject.IMAGE).GetChild(MapObjectNameArray[i, j]).name;
                    tileObject.name = tileObject.GetComponent<MapObject>().type + "_" + i + "_" + j; // 생성된 프리팹의 이름 변경

                    // 해당하는 오브젝트 이미지를 표시
                    tileObject.transform.GetChild(MapObject.IMAGE).GetChild(MapObjectNameArray[i, j]).gameObject.SetActive(true);

                    // 오브젝트가 위치하는 맵타일을 이동불가상태로 설정
                    tile.GetComponent<MapTile>().Moveable = false;

                    MapObjectArray[i, j] = tileObject;
                }
            }
        }
    }

    void MapTileColorCheak()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // 오브젝트 체크
                if (MapObjectArray[i, j] != null)
                {
                    if (MapObjectArray[i, j].tag == "Character") // 캐릭터 위치 표시
                    {
                        MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.7f, 0.5f, 0.5f);
                    }
                    else if (MapObjectArray[i, j].tag == "Enemy") // 몬스터 위치 표시
                    {
                        MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.53f, 0.64f, 0.81f, 0.42f);
                    }
                    else
                    {
                        MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    }
                }
                else if (MoveState[i, j] == true) // 이동가능 범위 표시
                {
                    MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.0f, 3.0f, 0.0f, 0.4f);
                }
                else
                {
                    MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }

                // 공격 범위 표시
                if (AttackState[i, j] == true)
                {
                    MapTileArray[i, j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.3f, 0.0f, 0.4f);
                }

                // 몬스터 타겟 지점 표시
                if (EnemyTargeted[i, j] == true)
                {
                    MapTileArray[i, j].transform.GetChild(MapTile.UI).GetChild(MapTile.Targeted).gameObject.SetActive(true);

                    Vector3 temp = new Vector3(Cell[i, j].x, Cell[i, j].y - 0.374f,
                        MapTileArray[i, j].transform.GetChild(MapTile.UI).GetChild(MapTile.Targeted).position.z);

                    if (MapTileArray[i, j].GetComponent<MapTile>().Water)
                    {
                        temp.y -= 0.132f;
                        MapTileArray[i, j].transform.GetChild(MapTile.UI).GetChild(MapTile.Targeted).position = temp;
                    }
                    else
                    {
                        MapTileArray[i, j].transform.GetChild(MapTile.UI).GetChild(MapTile.Targeted).position = temp;
                    }
                }
                else
                {
                    MapTileArray[i, j].transform.GetChild(MapTile.UI).GetChild(MapTile.Targeted).gameObject.SetActive(false);
                }
            }
        }
    }

    // 맵 타일 설정
    void InitializeMapTile(int _id)
    {
        switch(_id)
        {
            case 0: //*   << 초원 맵 >>  *//
                MapTileNameArray[0, 0] = MapTile.Grass; MapObjectNameArray[0, 0] = MapObject.Empty;
                MapTileNameArray[0, 1] = MapTile.Grass; MapObjectNameArray[0, 1] = MapObject.Empty;
                MapTileNameArray[0, 2] = MapTile.Grass; MapObjectNameArray[0, 2] = MapObject.Empty;
                MapTileNameArray[0, 3] = MapTile.Grass_Forest; MapObjectNameArray[0, 3] = MapObject.Empty;
                MapTileNameArray[0, 4] = MapTile.Grass; MapObjectNameArray[0, 4] = MapObject.Empty;
                MapTileNameArray[0, 5] = MapTile.Grass; MapObjectNameArray[0, 5] = MapObject.Empty;
                MapTileNameArray[0, 6] = MapTile.Grass; MapObjectNameArray[0, 6] = MapObject.Empty;
                MapTileNameArray[0, 7] = MapTile.Grass_Forest; MapObjectNameArray[0, 7] = MapObject.Empty;

                MapTileNameArray[1, 0] = MapTile.Grass; MapObjectNameArray[1, 0] = MapObject.Empty;
                MapTileNameArray[1, 1] = MapTile.Water_LR; MapObjectNameArray[1, 1] = MapObject.Empty;
                MapTileNameArray[1, 2] = MapTile.Grass; MapObjectNameArray[1, 2] = MapObject.GrassMountain;
                MapTileNameArray[1, 3] = MapTile.Water_LR; MapObjectNameArray[1, 3] = MapObject.Empty;
                MapTileNameArray[1, 4] = MapTile.Water_R; MapObjectNameArray[1, 4] = MapObject.Empty;
                MapTileNameArray[1, 5] = MapTile.Grass; MapObjectNameArray[1, 5] = MapObject.Empty;
                MapTileNameArray[1, 6] = MapTile.Grass; MapObjectNameArray[1, 6] = MapObject.Empty;
                MapTileNameArray[1, 7] = MapTile.Grass_Forest; MapObjectNameArray[1, 7] = MapObject.Empty;

                MapTileNameArray[2, 0] = MapTile.Grass; MapObjectNameArray[2, 0] = MapObject.Empty;
                MapTileNameArray[2, 1] = MapTile.Grass_Forest; MapObjectNameArray[2, 1] = MapObject.Empty;
                MapTileNameArray[2, 2] = MapTile.Grass; MapObjectNameArray[2, 2] = MapObject.Building_1;
                MapTileNameArray[2, 3] = MapTile.Grass_Forest; MapObjectNameArray[2, 3] = MapObject.Empty;
                MapTileNameArray[2, 4] = MapTile.Grass; MapObjectNameArray[2, 4] = MapObject.Building_1;
                MapTileNameArray[2, 5] = MapTile.Grass; MapObjectNameArray[2, 5] = MapObject.Empty;
                MapTileNameArray[2, 6] = MapTile.Grass; MapObjectNameArray[2, 6] = MapObject.Empty;
                MapTileNameArray[2, 7] = MapTile.Grass_Forest; MapObjectNameArray[2, 7] = MapObject.Empty;

                MapTileNameArray[3, 0] = MapTile.Grass; MapObjectNameArray[3, 0] = MapObject.Empty;
                MapTileNameArray[3, 1] = MapTile.Grass; MapObjectNameArray[3, 1] = MapObject.Empty;
                MapTileNameArray[3, 2] = MapTile.Grass; MapObjectNameArray[3, 2] = MapObject.Empty;
                MapTileNameArray[3, 3] = MapTile.Grass; MapObjectNameArray[3, 3] = MapObject.GrassMountain;
                MapTileNameArray[3, 4] = MapTile.Grass; MapObjectNameArray[3, 4] = MapObject.GrassMountain;
                MapTileNameArray[3, 5] = MapTile.Grass; MapObjectNameArray[3, 5] = MapObject.Empty;
                MapTileNameArray[3, 6] = MapTile.Grass; MapObjectNameArray[3, 6] = MapObject.Empty;
                MapTileNameArray[3, 7] = MapTile.Grass_Forest; MapObjectNameArray[3, 7] = MapObject.Empty;

                MapTileNameArray[4, 0] = MapTile.Grass; MapObjectNameArray[4, 0] = MapObject.Building_0;
                MapTileNameArray[4, 1] = MapTile.Grass; MapObjectNameArray[4, 1] = MapObject.Building_0;
                MapTileNameArray[4, 2] = MapTile.Grass; MapObjectNameArray[4, 2] = MapObject.Empty;
                MapTileNameArray[4, 3] = MapTile.Grass; MapObjectNameArray[4, 3] = MapObject.Empty;
                MapTileNameArray[4, 4] = MapTile.Grass_Forest; MapObjectNameArray[4, 4] = MapObject.Empty;
                MapTileNameArray[4, 5] = MapTile.Grass_Forest; MapObjectNameArray[4, 5] = MapObject.Empty;
                MapTileNameArray[4, 6] = MapTile.Grass; MapObjectNameArray[4, 6] = MapObject.Empty;
                MapTileNameArray[4, 7] = MapTile.Grass; MapObjectNameArray[4, 7] = MapObject.GrassMountain;

                MapTileNameArray[5, 0] = MapTile.Grass; MapObjectNameArray[5, 0] = MapObject.Building_0;
                MapTileNameArray[5, 1] = MapTile.Grass; MapObjectNameArray[5, 1] = MapObject.Building_1;
                MapTileNameArray[5, 2] = MapTile.Grass; MapObjectNameArray[5, 2] = MapObject.Empty;
                MapTileNameArray[5, 3] = MapTile.Grass; MapObjectNameArray[5, 3] = MapObject.Empty;
                MapTileNameArray[5, 4] = MapTile.Grass; MapObjectNameArray[5, 4] = MapObject.Empty;
                MapTileNameArray[5, 5] = MapTile.Grass_Forest; MapObjectNameArray[5, 5] = MapObject.Empty;
                MapTileNameArray[5, 6] = MapTile.Grass; MapObjectNameArray[5, 6] = MapObject.Empty;
                MapTileNameArray[5, 7] = MapTile.Grass; MapObjectNameArray[5, 7] = MapObject.Empty;

                MapTileNameArray[6, 0] = MapTile.Grass; MapObjectNameArray[6, 0] = MapObject.Empty;
                MapTileNameArray[6, 1] = MapTile.Grass; MapObjectNameArray[6, 1] = MapObject.Empty;
                MapTileNameArray[6, 2] = MapTile.Grass; MapObjectNameArray[6, 2] = MapObject.Empty;
                MapTileNameArray[6, 3] = MapTile.Grass; MapObjectNameArray[6, 3] = MapObject.Building_1;
                MapTileNameArray[6, 4] = MapTile.Grass; MapObjectNameArray[6, 4] = MapObject.Empty;
                MapTileNameArray[6, 5] = MapTile.Grass; MapObjectNameArray[6, 5] = MapObject.Building_1;
                MapTileNameArray[6, 6] = MapTile.Grass; MapObjectNameArray[6, 6] = MapObject.Empty;
                MapTileNameArray[6, 7] = MapTile.Grass_Forest; MapObjectNameArray[6, 7] = MapObject.Empty;

                MapTileNameArray[7, 0] = MapTile.Grass; MapObjectNameArray[7, 0] = MapObject.GrassMountain;
                MapTileNameArray[7, 1] = MapTile.Grass; MapObjectNameArray[7, 1] = MapObject.GrassMountain;
                MapTileNameArray[7, 2] = MapTile.Grass; MapObjectNameArray[7, 2] = MapObject.Empty;
                MapTileNameArray[7, 3] = MapTile.Water_LR; MapObjectNameArray[7, 3] = MapObject.Empty;
                MapTileNameArray[7, 4] = MapTile.Water_R; MapObjectNameArray[7, 4] = MapObject.Empty;
                MapTileNameArray[7, 5] = MapTile.Water_R; MapObjectNameArray[7, 5] = MapObject.Empty;
                MapTileNameArray[7, 6] = MapTile.Water_R; MapObjectNameArray[7, 6] = MapObject.Empty;
                MapTileNameArray[7, 7] = MapTile.Water_R; MapObjectNameArray[7, 7] = MapObject.Empty;
                break;

            case 1: //*   << 사막 맵 >>  *//
                MapTileNameArray[0, 0] = MapTile.Sand; MapObjectNameArray[0, 0] = MapObject.SandMountain;
                MapTileNameArray[0, 1] = MapTile.Sand; MapObjectNameArray[0, 1] = MapObject.SandMountain;
                MapTileNameArray[0, 2] = MapTile.Sand; MapObjectNameArray[0, 2] = MapObject.SandMountain;
                MapTileNameArray[0, 3] = MapTile.Sand; MapObjectNameArray[0, 3] = MapObject.SandMountain;
                MapTileNameArray[0, 4] = MapTile.Sand; MapObjectNameArray[0, 4] = MapObject.SandMountain;
                MapTileNameArray[0, 5] = MapTile.Sand; MapObjectNameArray[0, 5] = MapObject.SandMountain;
                MapTileNameArray[0, 6] = MapTile.Sand; MapObjectNameArray[0, 6] = MapObject.SandMountain;
                MapTileNameArray[0, 7] = MapTile.Sand; MapObjectNameArray[0, 7] = MapObject.SandMountain;

                MapTileNameArray[1, 0] = MapTile.Sand; MapObjectNameArray[1, 0] = MapObject.SandMountain;
                MapTileNameArray[1, 1] = MapTile.Sand; MapObjectNameArray[1, 1] = MapObject.SandMountain;
                MapTileNameArray[1, 2] = MapTile.Sand; MapObjectNameArray[1, 2] = MapObject.Building_0;
                MapTileNameArray[1, 3] = MapTile.Sand; MapObjectNameArray[1, 3] = MapObject.Building_1;
                MapTileNameArray[1, 4] = MapTile.Sand; MapObjectNameArray[1, 4] = MapObject.Building_1;
                MapTileNameArray[1, 5] = MapTile.Sand; MapObjectNameArray[1, 5] = MapObject.SandMountain;
                MapTileNameArray[1, 6] = MapTile.Sand; MapObjectNameArray[1, 6] = MapObject.Building_1;
                MapTileNameArray[1, 7] = MapTile.Sand; MapObjectNameArray[1, 7] = MapObject.SandMountain;

                MapTileNameArray[2, 0] = MapTile.Sand_Forest; MapObjectNameArray[2, 0] = MapObject.Empty;
                MapTileNameArray[2, 1] = MapTile.Sand; MapObjectNameArray[2, 1] = MapObject.Empty;
                MapTileNameArray[2, 2] = MapTile.Sand; MapObjectNameArray[2, 2] = MapObject.Empty;
                MapTileNameArray[2, 3] = MapTile.Sand; MapObjectNameArray[2, 3] = MapObject.Empty;
                MapTileNameArray[2, 4] = MapTile.Sand; MapObjectNameArray[2, 4] = MapObject.Empty;
                MapTileNameArray[2, 5] = MapTile.Sand; MapObjectNameArray[2, 5] = MapObject.Empty;
                MapTileNameArray[2, 6] = MapTile.Sand_Forest; MapObjectNameArray[2, 6] = MapObject.Empty;
                MapTileNameArray[2, 7] = MapTile.Sand; MapObjectNameArray[2, 7] = MapObject.Empty;

                MapTileNameArray[3, 0] = MapTile.Sand; MapObjectNameArray[3, 0] = MapObject.Empty;
                MapTileNameArray[3, 1] = MapTile.Sand; MapObjectNameArray[3, 1] = MapObject.Building_0;
                MapTileNameArray[3, 2] = MapTile.Sand; MapObjectNameArray[3, 2] = MapObject.Empty;
                MapTileNameArray[3, 3] = MapTile.Sand; MapObjectNameArray[3, 3] = MapObject.Empty;
                MapTileNameArray[3, 4] = MapTile.Sand_Forest; MapObjectNameArray[3, 4] = MapObject.Empty;
                MapTileNameArray[3, 5] = MapTile.Sand; MapObjectNameArray[3, 5] = MapObject.Empty;
                MapTileNameArray[3, 6] = MapTile.Sand; MapObjectNameArray[3, 6] = MapObject.Empty;
                MapTileNameArray[3, 7] = MapTile.Sand; MapObjectNameArray[3, 7] = MapObject.Empty;

                MapTileNameArray[4, 0] = MapTile.Sand; MapObjectNameArray[4, 0] = MapObject.Empty;
                MapTileNameArray[4, 1] = MapTile.Sand; MapObjectNameArray[4, 1] = MapObject.Building_0;
                MapTileNameArray[4, 2] = MapTile.Sand; MapObjectNameArray[4, 2] = MapObject.Empty;
                MapTileNameArray[4, 3] = MapTile.Sand_Forest; MapObjectNameArray[4, 3] = MapObject.Empty;
                MapTileNameArray[4, 4] = MapTile.Sand; MapObjectNameArray[4, 4] = MapObject.Building_0;
                MapTileNameArray[4, 5] = MapTile.Sand; MapObjectNameArray[4, 5] = MapObject.Empty;
                MapTileNameArray[4, 6] = MapTile.Sand; MapObjectNameArray[4, 6] = MapObject.Empty;
                MapTileNameArray[4, 7] = MapTile.Sand; MapObjectNameArray[4, 7] = MapObject.SandMountain;

                MapTileNameArray[5, 0] = MapTile.Sand; MapObjectNameArray[5, 0] = MapObject.Empty;
                MapTileNameArray[5, 1] = MapTile.Sand; MapObjectNameArray[5, 1] = MapObject.Empty;
                MapTileNameArray[5, 2] = MapTile.Sand; MapObjectNameArray[5, 2] = MapObject.Empty;
                MapTileNameArray[5, 3] = MapTile.Sand; MapObjectNameArray[5, 3] = MapObject.Empty;
                MapTileNameArray[5, 4] = MapTile.Sand; MapObjectNameArray[5, 4] = MapObject.Empty;
                MapTileNameArray[5, 5] = MapTile.Sand; MapObjectNameArray[5, 5] = MapObject.Empty;
                MapTileNameArray[5, 6] = MapTile.Sand; MapObjectNameArray[5, 6] = MapObject.Empty;
                MapTileNameArray[5, 7] = MapTile.Sand; MapObjectNameArray[5, 7] = MapObject.Empty;

                MapTileNameArray[6, 0] = MapTile.Sand_Water_LR; MapObjectNameArray[6, 0] = MapObject.Empty;
                MapTileNameArray[6, 1] = MapTile.Sand; MapObjectNameArray[6, 1] = MapObject.Building_0;
                MapTileNameArray[6, 2] = MapTile.Sand; MapObjectNameArray[6, 2] = MapObject.SandMountain;
                MapTileNameArray[6, 3] = MapTile.Sand; MapObjectNameArray[6, 3] = MapObject.Building_1;
                MapTileNameArray[6, 4] = MapTile.Sand; MapObjectNameArray[6, 4] = MapObject.Empty;
                MapTileNameArray[6, 5] = MapTile.Sand; MapObjectNameArray[6, 5] = MapObject.Building_0;
                MapTileNameArray[6, 6] = MapTile.Sand_Forest; MapObjectNameArray[6, 6] = MapObject.Empty;
                MapTileNameArray[6, 7] = MapTile.Sand_Forest; MapObjectNameArray[6, 7] = MapObject.Empty;

                MapTileNameArray[7, 0] = MapTile.Sand_Water_L; MapObjectNameArray[7, 0] = MapObject.Empty;
                MapTileNameArray[7, 1] = MapTile.Sand_Water_R; MapObjectNameArray[7, 1] = MapObject.Empty;
                MapTileNameArray[7, 2] = MapTile.Sand_Water_R; MapObjectNameArray[7, 2] = MapObject.Empty;
                MapTileNameArray[7, 3] = MapTile.Sand_Water_R; MapObjectNameArray[7, 3] = MapObject.Empty;
                MapTileNameArray[7, 4] = MapTile.Sand_Water_R; MapObjectNameArray[7, 4] = MapObject.Empty;
                MapTileNameArray[7, 5] = MapTile.Sand_Water_R; MapObjectNameArray[7, 5] = MapObject.Empty;
                MapTileNameArray[7, 6] = MapTile.Sand; MapObjectNameArray[7, 6] = MapObject.Empty;
                MapTileNameArray[7, 7] = MapTile.Sand; MapObjectNameArray[7, 7] = MapObject.Empty;
                break;

            case 2: //*   << 눈 맵 >>  *//
                MapTileNameArray[0, 0] = MapTile.Snow; MapObjectNameArray[0, 0] = MapObject.SnowMountain;
                MapTileNameArray[0, 1] = MapTile.Snow; MapObjectNameArray[0, 1] = MapObject.SnowMountain;
                MapTileNameArray[0, 2] = MapTile.Snow; MapObjectNameArray[0, 2] = MapObject.Empty;
                MapTileNameArray[0, 3] = MapTile.Snow; MapObjectNameArray[0, 3] = MapObject.Empty;
                MapTileNameArray[0, 4] = MapTile.Snow; MapObjectNameArray[0, 4] = MapObject.Building_1;
                MapTileNameArray[0, 5] = MapTile.Snow_Forest; MapObjectNameArray[0, 5] = MapObject.Empty;
                MapTileNameArray[0, 6] = MapTile.Snow; MapObjectNameArray[0, 6] = MapObject.Empty;
                MapTileNameArray[0, 7] = MapTile.Water_LR; MapObjectNameArray[0, 7] = MapObject.Empty;

                MapTileNameArray[1, 0] = MapTile.Snow; MapObjectNameArray[1, 0] = MapObject.SnowMountain;
                MapTileNameArray[1, 1] = MapTile.Snow; MapObjectNameArray[1, 1] = MapObject.Empty;
                MapTileNameArray[1, 2] = MapTile.Snow; MapObjectNameArray[1, 2] = MapObject.Empty;
                MapTileNameArray[1, 3] = MapTile.Snow_Forest; MapObjectNameArray[1, 3] = MapObject.Empty;
                MapTileNameArray[1, 4] = MapTile.Snow; MapObjectNameArray[1, 4] = MapObject.Building_1;
                MapTileNameArray[1, 5] = MapTile.Snow; MapObjectNameArray[1, 5] = MapObject.Empty;
                MapTileNameArray[1, 6] = MapTile.Snow; MapObjectNameArray[1, 6] = MapObject.Empty;
                MapTileNameArray[1, 7] = MapTile.Snow; MapObjectNameArray[1, 7] = MapObject.SnowMountain;

                MapTileNameArray[2, 0] = MapTile.Snow; MapObjectNameArray[2, 0] = MapObject.Building_0;
                MapTileNameArray[2, 1] = MapTile.Snow; MapObjectNameArray[2, 1] = MapObject.Empty;
                MapTileNameArray[2, 2] = MapTile.Snow; MapObjectNameArray[2, 2] = MapObject.Empty;
                MapTileNameArray[2, 3] = MapTile.Snow; MapObjectNameArray[2, 3] = MapObject.Empty;
                MapTileNameArray[2, 4] = MapTile.Snow; MapObjectNameArray[2, 4] = MapObject.Empty;
                MapTileNameArray[2, 5] = MapTile.Snow; MapObjectNameArray[2, 5] = MapObject.Empty;
                MapTileNameArray[2, 6] = MapTile.Snow; MapObjectNameArray[2, 6] = MapObject.Empty;
                MapTileNameArray[2, 7] = MapTile.Snow_Forest; MapObjectNameArray[2, 7] = MapObject.Empty;

                MapTileNameArray[3, 0] = MapTile.Snow; MapObjectNameArray[3, 0] = MapObject.Building_0;
                MapTileNameArray[3, 1] = MapTile.Snow_Forest; MapObjectNameArray[3, 1] = MapObject.Empty;
                MapTileNameArray[3, 2] = MapTile.Snow; MapObjectNameArray[3, 2] = MapObject.Empty;
                MapTileNameArray[3, 3] = MapTile.Snow; MapObjectNameArray[3, 3] = MapObject.Building_1;
                MapTileNameArray[3, 4] = MapTile.Snow; MapObjectNameArray[3, 4] = MapObject.Empty;
                MapTileNameArray[3, 5] = MapTile.Water_LR; MapObjectNameArray[3, 5] = MapObject.Empty;
                MapTileNameArray[3, 6] = MapTile.Snow; MapObjectNameArray[3, 6] = MapObject.Empty;
                MapTileNameArray[3, 7] = MapTile.Snow; MapObjectNameArray[3, 7] = MapObject.Empty;

                MapTileNameArray[4, 0] = MapTile.Snow; MapObjectNameArray[4, 0] = MapObject.Building_0;
                MapTileNameArray[4, 1] = MapTile.Snow; MapObjectNameArray[4, 1] = MapObject.Empty;
                MapTileNameArray[4, 2] = MapTile.Snow; MapObjectNameArray[4, 2] = MapObject.Empty;
                MapTileNameArray[4, 3] = MapTile.Snow; MapObjectNameArray[4, 3] = MapObject.Empty;
                MapTileNameArray[4, 4] = MapTile.Snow; MapObjectNameArray[4, 4] = MapObject.Empty;
                MapTileNameArray[4, 5] = MapTile.Water_L; MapObjectNameArray[4, 5] = MapObject.Empty;
                MapTileNameArray[4, 6] = MapTile.Snow; MapObjectNameArray[4, 6] = MapObject.Empty;
                MapTileNameArray[4, 7] = MapTile.Snow_Forest; MapObjectNameArray[4, 7] = MapObject.Empty;

                MapTileNameArray[5, 0] = MapTile.Snow; MapObjectNameArray[5, 0] = MapObject.Empty;
                MapTileNameArray[5, 1] = MapTile.Snow; MapObjectNameArray[5, 1] = MapObject.Empty;
                MapTileNameArray[5, 2] = MapTile.Snow; MapObjectNameArray[5, 2] = MapObject.Empty;
                MapTileNameArray[5, 3] = MapTile.Snow; MapObjectNameArray[5, 3] = MapObject.Empty;
                MapTileNameArray[5, 4] = MapTile.Snow_Forest; MapObjectNameArray[5, 4] = MapObject.Empty;
                MapTileNameArray[5, 5] = MapTile.Snow; MapObjectNameArray[5, 5] = MapObject.Empty;
                MapTileNameArray[5, 6] = MapTile.Snow; MapObjectNameArray[5, 6] = MapObject.Empty;
                MapTileNameArray[5, 7] = MapTile.Snow; MapObjectNameArray[5, 7] = MapObject.Empty;

                MapTileNameArray[6, 0] = MapTile.Water_LR; MapObjectNameArray[6, 0] = MapObject.Empty;
                MapTileNameArray[6, 1] = MapTile.Snow; MapObjectNameArray[6, 1] = MapObject.Empty;
                MapTileNameArray[6, 2] = MapTile.Snow; MapObjectNameArray[6, 2] = MapObject.Building_0;
                MapTileNameArray[6, 3] = MapTile.Snow; MapObjectNameArray[6, 3] = MapObject.Building_1;
                MapTileNameArray[6, 4] = MapTile.Snow; MapObjectNameArray[6, 4] = MapObject.Empty;
                MapTileNameArray[6, 5] = MapTile.Snow; MapObjectNameArray[6, 5] = MapObject.Empty;
                MapTileNameArray[6, 6] = MapTile.Snow; MapObjectNameArray[6, 6] = MapObject.Empty;
                MapTileNameArray[6, 7] = MapTile.Snow; MapObjectNameArray[6, 7] = MapObject.SnowMountain;

                MapTileNameArray[7, 0] = MapTile.Snow; MapObjectNameArray[7, 0] = MapObject.Empty;
                MapTileNameArray[7, 1] = MapTile.Snow_Forest; MapObjectNameArray[7, 1] = MapObject.Empty;
                MapTileNameArray[7, 2] = MapTile.Snow; MapObjectNameArray[7, 2] = MapObject.Empty;
                MapTileNameArray[7, 3] = MapTile.Snow; MapObjectNameArray[7, 3] = MapObject.Empty;
                MapTileNameArray[7, 4] = MapTile.Snow_Forest; MapObjectNameArray[7, 4] = MapObject.Empty;
                MapTileNameArray[7, 5] = MapTile.Snow; MapObjectNameArray[7, 5] = MapObject.Empty;
                MapTileNameArray[7, 6] = MapTile.Snow_Forest; MapObjectNameArray[7, 6] = MapObject.Empty;
                MapTileNameArray[7, 7] = MapTile.Snow; MapObjectNameArray[7, 7] = MapObject.SnowMountain;
                break;
        }
        
    }
}