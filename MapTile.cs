using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <변경점>
//  타일에 Forest가 존재하는지 체크하는 변수들 추가
//  

// 맵 타일 프리팹에 직접 적용되는 스크립트
public class MapTile : MonoBehaviour
{
    // 맵 타일 관련 상수값을 정의
    public const int IMAGE = 0;
    public const int RGB = 1;
    public const int FOREST = 2;
    public const int UI = 3;
    public const int ROAD = 4;

    // 맵 타일 type 관련 상수값을 정의
    public const int Grass = 0;
    public const int Snow = 1;
    public const int Sand = 2;
    public const int Water_C = 3;
    public const int Water_L = 4;
    public const int Water_LR = 5;
    public const int Water_R = 6;
    public const int Sand_Water_C = 7;
    public const int Sand_Water_L = 8;
    public const int Sand_Water_LR = 9;
    public const int Sand_Water_R = 10;

    // 경로 표시 이미지 관련 상수값을 정의
    public const int Road_Up = 0;
    public const int Road_Down = 1;
    public const int Road_Left = 2;
    public const int Road_Right = 3;
    public const int Road_UD = 4;
    public const int Road_LR = 5;
    public const int Road_Corner_UL = 6;
    public const int Road_Corner_UR = 7;
    public const int Road_Corner_DL = 8;
    public const int Road_Corner_DR = 9;
    public const int Road_End_Up = 10;
    public const int Road_End_Down = 11;
    public const int Road_End_Left = 12;
    public const int Road_End_Right = 13;

    // 맵 타일 Forest 관련 상수값을 정의
    public const int Grass_Forest = 100;
    public const int Snow_Forest = 101;
    public const int Sand_Forest = 102;

    // 맵 타일 UI 관련 상수값을 정의
    public const int TileForest = 0;
    public const int Targeted = 1;

    // 맵 타일 종류
    public string type;

    // 물 타일인지 확인하는 변수
    public bool Water;

    // 타일에 숲이 존재하는지 확인하는 변수
    public bool Forest;

    // 숲이 불타는지 확인하는 변수
    public bool isBurnForest;

    // 이동 가능한 타일인지 확인하는 변수
    public bool Moveable = true;

    public int indexX, indexY;
    public Animator animator;
    public  Sound s;

    private void Start()
    {
        animator = GetComponent<Animator>();
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
    }

    private void Update()
    {
        if (MapControl.MapObjectArray[indexX, indexY] == null ||
            MapControl.MapObjectArray[indexX, indexY].name == "SelectPlayerMechImg")
        {
            this.Moveable = true;
        }
        else
        {
            this.Moveable = false;
        }

        if (Forest == true && MapControl.MapObjectArray[indexX, indexY] != null)
        {
            this.transform.GetChild(MapTile.UI).GetChild(MapTile.TileForest).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(MapTile.UI).GetChild(MapTile.TileForest).gameObject.SetActive(false);
        }

        if(isBurnForest)
        {
            animator.SetBool("isBurnForest", true);
        }
    }

    // 마우스가 충돌체 안에 있는 동안 작동
    void OnMouseOver()
    {
        Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MapControl.mouse_pos.x = temp.x;
        MapControl.mouse_pos.y = temp.y;

        MapControl.isMouseIn = true;
    }

    // 마우스가 충돌체 안에 들어올때 작동
    private void OnMouseEnter()
    {
        TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();

        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN)
        {
            s.SoundPlay("EffectSound/ui_battle_highlight_terrain");
            ClickManager cm = ClickManager.GetInst();

            if (cm.obj == null && UIControl.GetInst().proflieMiddlePick == false)
            {
                if (MapControl.MapObjectArray[indexX, indexY] != null &&
                    MapControl.MapObjectArray[indexX, indexY].tag == "Character")
                {
                    MapControl.MapObjectArray[indexX, indexY].GetComponent<Player>().MouseOn = true;
                }

                if (MapControl.MapObjectArray[indexX, indexY] != null &&
                   MapControl.MapObjectArray[indexX, indexY].tag == "Enemy")
                {
                    MapControl.MapObjectArray[indexX, indexY].GetComponent<Enemy>().MouseOn = true;
                }
            }
        }
    }

    // 마우스가 충돌체에서 나갈때 작동
    void OnMouseExit()
    {
        MapControl.isMouseIn = false;

        TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();

        if (tm.currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN)
        {
            if (MapControl.MapObjectArray[indexX, indexY] != null &&
                    MapControl.MapObjectArray[indexX, indexY].tag == "Character")
            {
                MapControl.MapObjectArray[indexX, indexY].GetComponent<Player>().MouseOn = false;
            }

            if (MapControl.MapObjectArray[indexX, indexY] != null &&
               MapControl.MapObjectArray[indexX, indexY].tag == "Enemy")
            {
                MapControl.MapObjectArray[indexX, indexY].GetComponent<Enemy>().MouseOn = false;
            }
        }
    }

    // 마우스가 충돌체에서 클릭을 했을때 작동
    private void OnMouseDown()
    {
        if (TurnBaseBattleManager.GetInst().currentState == TurnBaseBattleManager.BattleStates.PLAYERTURN &&
            ClickManager.GetInst().attackOn == false)
        {
            ClickManager cm = ClickManager.GetInst();

            if (cm.player != null && cm.player.Mode == Unit.MODE.Move) // 클릭한 캐릭터가 존재하고 Move 모드일 경우
            {
                if (MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] == true)  // 이동 가능한 타일 일 때
                {
                    Functions f = Functions.GetInst();

                    f.Move(cm.player.gameObject, MapControl.Crt_X, MapControl.Crt_Y); // 이동
                    cm.player.MoveAvailable = false; // 이동 한 상태로 설정
                    f.MoveRangClear(); // 이동 범위 표시 off
                    cm.player.EndMouseOver(); // 투명한 캐릭터 UI off
                    cm.player.Mode = Unit.MODE.MoveEnd; // 캐릭터를 Attack 모드로 전환
                }
                else
                {
                    cm.player.Mode = Unit.MODE.None;
                }
            }
        }
    }
}
