using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{

    public static Functions Inst = null;
    public GameObject ProfileMenuBottom;
    public Sound s;

    int NowPos = 0;

    int cnt, pos; // 좌표정보 배열의 길이
    Vector2[] StartCoordinates = new Vector2[255]; // 시작좌표
    int[] ShortestDistance = new int[255]; // 최단거리 길이


    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        ProfileMenuBottom = GameObject.Find("ProfileInfoBottom");
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
    }

    public static Functions GetInst()
    {
        return Inst;
    }


    void CoordinateInformation(Vector2 _StartCoordinates, int _ShortestDistance) // 좌표정보 저장함수
    {
        StartCoordinates[cnt] = _StartCoordinates;
        ShortestDistance[cnt] = _ShortestDistance;

        cnt++;
    }

    int cnt2, pos2; // 좌표정보 배열의 길이
    Vector2[] StartCoordinates2 = new Vector2[100]; // 시작좌표
    int[] ShortestDistance2 = new int[100]; // 최단거리 길이

    void CoordinateInformation2(Vector2 _StartCoordinates, int _ShortestDistance) // 좌표정보 저장함수
    {
        StartCoordinates2[cnt2] = _StartCoordinates;
        ShortestDistance2[cnt2] = _ShortestDistance;

        cnt2++;
    }

    public bool IsObjOn()//누른위치에 오브젝트가 있나? 없으면 return true
    {
        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null && MapControl.isMouseIn == true)
            return true;
        else
            return false;
    }

    bool CheckBeforeMove() //다른데 클릭시 움직이기전 움직임이 가능한지. 가능하면 true리턴
    {
        if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] == null)
        {
            if (MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] == true)
            {
                return true;
            }

        }
        return false;
    }

    // 근접 공격 범위 출력
    public void CloseAttackRange(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j)
                    {
                        MapControl.AttackState[i, j] = true;
                    }
                }
            }
        }

        if (a <= 0)
        {
            return;
        }

        Vector2 upTile = new Vector2(tileX, tileY - 1);
        Vector2 downTile = new Vector2(tileX, tileY + 1);
        Vector2 leftTile = new Vector2(tileX - 1, tileY);
        Vector2 rightTile = new Vector2(tileX + 1, tileY);
        a = 1;
        if (tileY > 0)
        {
            CloseAttackRange(a - 1, upTile, false);
        }
        if (tileY < 7)
        {
            CloseAttackRange(a - 1, downTile, false);
        }
        if (tileX > 0)
        {
            CloseAttackRange(a - 1, leftTile, false);
        }
        if (tileX < 7)
        {
            CloseAttackRange(a - 1, rightTile, false);
        }
    }

    // 박격포 공격 범위 표시
    // f.MortarAttackRange(10,  a, true); 과 같이 사용
    public void MortarAttackRange(int a, Vector2 tile, bool isFirst)
    {

        if (isFirst)
        {
            Vector2 c = new Vector2(tile.x, tile.y);
            MortarAttackRange2(10, c, true);
            MortarAttackRange3(10, c, true);
            MortarAttackRange4(10, c, true);
        }

        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && a < 9)
                    {
                        MapControl.AttackState[i, j] = true;
                    }
                }
            }
        }

        if (a <= 0)
        {

            return;
        }

        Vector2 upTile = new Vector2(tileX, tileY - 1);

        if (tileY > 0 && tileY < 7)
        {
            MortarAttackRange(a - 1, upTile, false);
        }
    }
    void MortarAttackRange2(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && a < 9)
                    {
                        MapControl.AttackState[i, j] = true;
                    }
                }
            }
        }

        if (a <= 0)
        {

            return;
        }

        Vector2 downTile = new Vector2(tileX, tileY + 1);

        if (tileY < 7)
        {
            MortarAttackRange2(a - 1, downTile, false);
        }
    }


    void MortarAttackRange3(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && a < 9)
                    {
                        MapControl.AttackState[i, j] = true;
                    }
                }
            }
        }

        if (a <= 0)
        {
            return;
        }

        Vector2 leftTile = new Vector2(tileX - 1, tileY);

        if (tileX > 0)
        {
            MortarAttackRange3(a - 1, leftTile, false);
        }

    }
    void MortarAttackRange4(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && a < 9)
                    {
                        MapControl.AttackState[i, j] = true;
                    }
                }
            }
        }

        if (a <= 0)
        {
            return;
        }

        Vector2 rightTile = new Vector2(tileX + 1, tileY);

        if (tileX < 7)
        {
            MortarAttackRange4(a - 1, rightTile, false);
        }

    }

    // 직사포 범위 출력
    // f.ProjectileAttackRange(10,  a, true); 과 같이 사용
    public void ProjectileAttackRange(int a, Vector2 tile, bool isFirst)
    {

        if (isFirst)
        {
            Vector2 c = new Vector2(tile.x, tile.y);
            Projectile2(10, c, true);
            Projectile3(10, c, true);
            Projectile4(10, c, true);
        }

        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j)
                    {
                        MapControl.AttackState[i, j] = true;
                        if (MapControl.MapObjectArray[tileX, tileY] == true)
                        {
                            return;
                        }
                    }
                }
            }
        }

        if (a <= 0)
        {

            return;
        }

        Vector2 upTile = new Vector2(tileX, tileY - 1);

        if (tileY > 0 && tileY < 7)
        {
            ProjectileAttackRange(a - 1, upTile, false);
        }
    }
    void Projectile2(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j)
                    {
                        MapControl.AttackState[i, j] = true;
                        if (MapControl.MapObjectArray[tileX, tileY] == true)
                        {
                            return;
                        }
                    }
                }
            }
        }

        if (a <= 0)
        {

            return;
        }

        Vector2 downTile = new Vector2(tileX, tileY + 1);

        if (tileY < 7)
        {
            Projectile2(a - 1, downTile, false);
        }
    }


    void Projectile3(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j)
                    {
                        MapControl.AttackState[i, j] = true;
                        if (MapControl.MapObjectArray[tileX, tileY] == true)
                        {
                            return;
                        }
                    }
                }
            }
        }

        if (a <= 0)
        {
            return;
        }

        Vector2 leftTile = new Vector2(tileX - 1, tileY);

        if (tileX > 0)
        {
            Projectile3(a - 1, leftTile, false);
        }

    }
    void Projectile4(int a, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j)
                    {
                        MapControl.AttackState[i, j] = true;
                        if (MapControl.MapObjectArray[tileX, tileY] == true)
                        {
                            return;
                        }
                    }
                }
            }
        }

        if (a <= 0)
        {
            return;
        }

        Vector2 rightTile = new Vector2(tileX + 1, tileY);

        if (tileX < 7)
        {
            Projectile4(a - 1, rightTile, false);
        }

    }

    //사용법 ->  f.EnemyMoveRange(1, Vector2 , true) -> 범위 1 넣어 주면 됩니다.
    // 적 사각형 이동 범위
    public void EnemyMoveRange2(int move, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && MapControl.MapTileArray[tileX, tileY].GetComponent<MapTile>().Moveable == true)
                    {
                        MapControl.MoveState[i, j] = true;
                        MapControl.MoveState2[i, j] = true;
                        MapControl.MoveState3[i, j] = 0;
                    }
                }
            }
        }

        if (move <= 0)
        {
            return;
        }

        Vector2 upTile = new Vector2(tileX, tileY - 1);
        Vector2 downTile = new Vector2(tileX, tileY + 1);
        Vector2 leftTile = new Vector2(tileX - 1, tileY);
        Vector2 rightTile = new Vector2(tileX + 1, tileY);

        Vector2 URTile = new Vector2(tileX - 1, tileY - 1);
        Vector2 DRTile = new Vector2(tileX + 1, tileY + 1);
        Vector2 ULTile = new Vector2(tileX - 1, tileY + 1);
        Vector2 DLTile = new Vector2(tileX + 1, tileY - 1);

        if (tileY > 0 && MapControl.MapObjectArray[tileX, tileY - 1] != true)
        {
            EnemyMoveRange2(move - 1, upTile, false);
        }
        if (tileY < 7 && MapControl.MapObjectArray[tileX, tileY + 1] != true)
        {
            EnemyMoveRange2(move - 1, downTile, false);
        }
        if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY] != true)
        {
            EnemyMoveRange2(move - 1, leftTile, false);
        }
        if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY] != true)
        {
            EnemyMoveRange2(move - 1, rightTile, false);
        }

        if (tileY > 0 && MapControl.MapObjectArray[tileX - 1, tileY - 1] != true)
        {
            EnemyMoveRange2(move - 1, URTile, false);
        }
        if (tileY < 7 && MapControl.MapObjectArray[tileX + 1, tileY + 1] != true)
        {
            EnemyMoveRange2(move - 1, DRTile, false);
        }
        if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY + 1] != true)
        {
            EnemyMoveRange2(move - 1, ULTile, false);
        }
        if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY - 1] != true)
        {
            EnemyMoveRange2(move - 1, DLTile, false);
        }
    }
    // 이동 범위 표시 함수
    public void MoveRange(int move, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;

        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && MapControl.MapTileArray[tileX, tileY].GetComponent<MapTile>().Moveable == true)
                    {
                        MapControl.MoveState[i, j] = true;
                    }
                    MapControl.MoveState2[tileX, tileY] = true;
                    MapControl.MoveState3[tileX, tileY] = 0;
                }
            }
        }

        if (move <= 0)
        {
            return;
        }

        Vector2 upTile = new Vector2(tileX, tileY - 1);
        Vector2 downTile = new Vector2(tileX, tileY + 1);
        Vector2 leftTile = new Vector2(tileX - 1, tileY);
        Vector2 rightTile = new Vector2(tileX + 1, tileY);

        if (tileY > 0 && MapControl.MapObjectArray[tileX, tileY - 1] != true)
        {
            MoveRange(move - 1, upTile, false);
        }
        if (tileY < 7 && MapControl.MapObjectArray[tileX, tileY + 1] != true)
        {
            MoveRange(move - 1, downTile, false);
        }
        if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY] != true)
        {
            MoveRange(move - 1, leftTile, false);
        }
        if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY] != true)
        {
            MoveRange(move - 1, rightTile, false);
        }

        foreach (GameObject a in TurnBaseBattleManager.GetInst().playerList)
        {
            if (tileY > 0 && MapControl.MapObjectArray[tileX, tileY - 1] == a)
            {
                MoveRange(move - 1, upTile, false);
            }
            if (tileY < 7 && MapControl.MapObjectArray[tileX, tileY + 1] == a)
            {
                MoveRange(move - 1, downTile, false);
            }
            if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY] == a)
            {
                MoveRange(move - 1, leftTile, false);
            }
            if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY] == a)
            {
                MoveRange(move - 1, rightTile, false);
            }
        }
    }

    public void EnemyMoveRange(int move, Vector2 tile, bool isFirst)
    {
        int tileX = (int)tile.x;
        int tileY = (int)tile.y;
        if (!isFirst)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tileX == i && tileY == j && MapControl.MapTileArray[tileX, tileY].GetComponent<MapTile>().Moveable == true)
                    {
                        MapControl.MoveState[i, j] = true;
                    }
                    MapControl.MoveState2[tileX, tileY] = true;
                    MapControl.MoveState3[tileX, tileY] = 0;
                }
            }
        }
        if (move <= 0)
        {
            return;
        }
        Vector2 upTile = new Vector2(tileX, tileY - 1);
        Vector2 downTile = new Vector2(tileX, tileY + 1);
        Vector2 leftTile = new Vector2(tileX - 1, tileY);
        Vector2 rightTile = new Vector2(tileX + 1, tileY);
        if (tileY > 0 && MapControl.MapObjectArray[tileX, tileY - 1] != true)
        {
            EnemyMoveRange(move - 1, upTile, false);
        }
        if (tileY < 7 && MapControl.MapObjectArray[tileX, tileY + 1] != true)
        {
            EnemyMoveRange(move - 1, downTile, false);
        }
        if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY] != true)
        {
            EnemyMoveRange(move - 1, leftTile, false);
        }
        if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY] != true)
        {
            EnemyMoveRange(move - 1, rightTile, false);
        }
        foreach (GameObject a in TurnBaseBattleManager.GetInst().enemyList)
        {
            if (tileY > 0 && MapControl.MapObjectArray[tileX, tileY - 1] == a)
            {
                EnemyMoveRange(move - 1, upTile, false);
            }
            if (tileY < 7 && MapControl.MapObjectArray[tileX, tileY + 1] == a)
            {
                EnemyMoveRange(move - 1, downTile, false);
            }
            if (tileX > 0 && MapControl.MapObjectArray[tileX - 1, tileY] == a)
            {
                EnemyMoveRange(move - 1, leftTile, false);
            }
            if (tileX < 7 && MapControl.MapObjectArray[tileX + 1, tileY] == a)
            {
                EnemyMoveRange(move - 1, rightTile, false);
            }
        }
    }

    // 몬스터 타겟 지점 표시 클리어
    public void EnemyTargetedClear()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                MapControl.EnemyTargeted[i, j] = false;
            }
        }
    }

    // 공격 범위 표시 클리어
    public void AttackRangeClear()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                MapControl.AttackState[i, j] = false;
            }
        }
    }

    //이동 범위 표시 클리어
    public void MoveRangClear()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                MapControl.MoveState[i, j] = false;
                MapControl.MoveState2[i, j] = false;
                MapControl.MoveState3[i, j] = 0;
            }
        }
    }

    //넘겨준 객체를 복사해서 40%인 투명도로 만든다. 복사객체를 리턴
    public GameObject CreateMovingMouseOver(GameObject _obj)
    {
        GameObject obj;
        obj = MonoBehaviour.Instantiate(_obj) as GameObject;
        Color TempColor = new Color(1f, 1f, 1f, 0.4f);
        obj.transform.GetComponentInChildren<SpriteRenderer>().color = TempColor;
        obj.tag = "CharacterMouseOver";
        obj.transform.GetChild(1).gameObject.SetActive(false);
        obj.transform.GetChild(2).gameObject.SetActive(false);
        obj.GetComponent<Player>().enabled = false;
        return obj;
    }
    public Player player;
    // 움직일 오브젝트와 이동할 좌표 인덱스 받아서 x, y에 이동시킴. 이동이 됬다면 true, 아니라면 false
    public bool Move(GameObject obj, int x, int y)
    {
        if (IsObjOn() == false && MapControl.MoveState[x, y] == true)
        {
            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
            tm.blockMouseClick = true;
            StartCoroutine(MMove(obj));

            if(obj.tag == "Character")
            {
                switch (obj.GetComponent<Player>().playerID)
                {
                    case 0:
                        s.SoundPlay("EffectSound/mech_prime_punch_move");
                        break;
                    case 1:
                        s.SoundPlay("EffectSound/mech_brute_tank_move");
                        break;
                    case 2:
                        s.SoundPlay("EffectSound/mech_distance_artillery_move");
                        break;
                    case 3:
                        s.SoundPlay("EffectSound/mech_prime_punch_move");
                        break;
                    case 4:
                        s.SoundPlay("EffectSound/mech_distance_artillery_move");
                        break;
                    case 5:
                        s.SoundPlay("EffectSound/mech_brute_tank_move");
                        break;
                }
            }

            MapControl.MapObjectArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y] = null;
            Clear();
            return true;
        }
        return false;
    }

    IEnumerator MMove(GameObject obj)
    {
        Player player = obj.GetComponent<Player>();

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (pos2 == NowPos)
            {
                MapControl.MapObjectArray[(int)MapControl.PlayerPosBackUp.x, (int)MapControl.PlayerPosBackUp.y] = obj;
                TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
                tm.blockMouseClick = false;
                NowPos = 0;
                break;
            }

            obj.transform.position = MapControl.Cell[(int)StartCoordinates2[(pos2 - 1) - NowPos].x, (int)StartCoordinates2[(pos2 - 1) - NowPos].y];

            MapControl.PlayerPosBackUp.x = (int)StartCoordinates2[(pos2 - 1) - NowPos].x;
            MapControl.PlayerPosBackUp.y = (int)StartCoordinates2[(pos2 - 1) - NowPos].y;
            NowPos++;
        }
    }

    // 움직일 오브젝트와 이동할 좌표 인덱스 받아서 x, y에 이동시킴. 이동이 됬다면 true, 아니라면 false
    public bool MoveEnemy(GameObject obj, int x, int y)
    {
        StartCoroutine(EnemyMMove(obj));
        MapControl.MapObjectArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y] = null;
        if (obj.tag =="Enemy")
        {
            switch (obj.GetComponent<Enemy>().enemyID)
            {
                case 0:
                    s.SoundPlay("EffectSound/enemy_hornet_1_move_01");
                    break;
                case 1:
                    s.SoundPlay("EffectSound/enemy_scorpion_1_move_01");
                    break;
                case 2:
                    s.SoundPlay("EffectSound/enemy_beetle_1_move_01");
                    break;
                case 3:
                    s.SoundPlay("EffectSound/enemy_leaper_1_move_01");
                    break;
                case 4:
                    s.SoundPlay("EffectSound/enemy_blobber_1_move_01");
                    break;
            }
        }

        return true;
    }

    IEnumerator EnemyMMove(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();

        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (pos2 == NowPos)
            {
                MapControl.MapObjectArray[(int)MapControl.EnemyPosBackUp.x, (int)MapControl.EnemyPosBackUp.y] = obj;
                obj.GetComponent<Enemy>().Mode = Unit.MODE.MoveEnd;
                NowPos = 0;
                break;
            }

            obj.transform.position = MapControl.Cell[(int)StartCoordinates2[(pos2 - 1) - NowPos].x, (int)StartCoordinates2[(pos2 - 1) - NowPos].y];
            MapControl.EnemyPosBackUp.x = (int)StartCoordinates2[(pos2 - 1) - NowPos].x;
            MapControl.EnemyPosBackUp.y = (int)StartCoordinates2[(pos2 - 1) - NowPos].y;
            NowPos++;
        }
    }

    public void EnemyBFS(Vector2 _first, bool isFirst, int _x, int _y)
    {
        Vector2 a = new Vector2(_x, _y);

        if (isFirst)
        {
            cnt = 0;
            pos = 0;
        }

        CoordinateInformation(_first, 1);

        while (pos < cnt && (StartCoordinates[pos].x != a.x || StartCoordinates[pos].y != a.y))
        {

            MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y] = false;


            if (StartCoordinates[pos].y > 0 && MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y - 1] == true)
            {
                Vector2 upTile = new Vector2(StartCoordinates[pos].x, StartCoordinates[pos].y - 1);
                CoordinateInformation(upTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y - 1] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].y < 7 && MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y + 1] == true)
            {
                Vector2 downTile = new Vector2(StartCoordinates[pos].x, StartCoordinates[pos].y + 1);
                CoordinateInformation(downTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y + 1] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].x > 0 && MapControl.MoveState2[(int)StartCoordinates[pos].x - 1, (int)StartCoordinates[pos].y] == true)
            {
                Vector2 leftTile = new Vector2(StartCoordinates[pos].x - 1, StartCoordinates[pos].y);
                CoordinateInformation(leftTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x - 1, (int)StartCoordinates[pos].y] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].x < 7 && MapControl.MoveState2[(int)StartCoordinates[pos].x + 1, (int)StartCoordinates[pos].y] == true)
            {
                Vector2 rightTile = new Vector2(StartCoordinates[pos].x + 1, StartCoordinates[pos].y);
                CoordinateInformation(rightTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x + 1, (int)StartCoordinates[pos].y] = ShortestDistance[pos] + 1;
            }

            pos++;

        }
        EnemyBFS2(a, StartCoordinates[0], true);

    }

    void EnemyBFS2(Vector2 _first, Vector2 _first2, bool isFirst)
    {
        Clear();

        if (isFirst)
        {
            cnt2 = 0;
            pos2 = 0;
        }

        CoordinateInformation2(_first, 1);

        while (pos2 < cnt2 && (StartCoordinates2[pos2].x != _first2.x || StartCoordinates2[pos2].y != _first2.y))
        {

            MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y] = 0;


            if (StartCoordinates2[pos2].y > 0 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y - 1] == ShortestDistance[pos] - 1)
            {
                Vector2 upTile = new Vector2(StartCoordinates2[pos2].x, StartCoordinates2[pos2].y - 1);
                CoordinateInformation2(upTile, ShortestDistance2[pos2] + 1);
            }
            else if (StartCoordinates2[pos2].y < 7 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y + 1] == ShortestDistance[pos] - 1)
            {
                Vector2 downTile = new Vector2(StartCoordinates2[pos2].x, StartCoordinates2[pos2].y + 1);
                CoordinateInformation2(downTile, ShortestDistance2[pos2] + 1);
            }
            else if (StartCoordinates2[pos2].x > 0 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x - 1, (int)StartCoordinates2[pos2].y] == ShortestDistance[pos] - 1)
            {
                Vector2 leftTile = new Vector2(StartCoordinates2[pos2].x - 1, StartCoordinates2[pos2].y);
                CoordinateInformation2(leftTile, ShortestDistance2[pos2] + 1);

            }
            else if (StartCoordinates2[pos2].x < 7 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x + 1, (int)StartCoordinates2[pos2].y] == ShortestDistance[pos] - 1)
            {
                Vector2 rightTile = new Vector2(StartCoordinates2[pos2].x + 1, StartCoordinates2[pos2].y);
                CoordinateInformation2(rightTile, ShortestDistance2[pos2] + 1);
            }

            ShortestDistance[pos] -= 1;

            pos2++;
        }

    }

    public void BFS(Vector2 _first, bool isFirst)
    {

        Vector2 a = new Vector2(MapControl.Crt_X, MapControl.Crt_Y);

        if (isFirst)
        {
            cnt = 0;
            pos = 0;
        }

        CoordinateInformation(_first, 1);

        while (pos < cnt && (StartCoordinates[pos].x != a.x || StartCoordinates[pos].y != a.y))
        {

            MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y] = false;


            if (StartCoordinates[pos].y > 0 && MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y - 1] == true)
            {
                Vector2 upTile = new Vector2(StartCoordinates[pos].x, StartCoordinates[pos].y - 1);
                CoordinateInformation(upTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y - 1] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].y < 7 && MapControl.MoveState2[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y + 1] == true)
            {
                Vector2 downTile = new Vector2(StartCoordinates[pos].x, StartCoordinates[pos].y + 1);
                CoordinateInformation(downTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x, (int)StartCoordinates[pos].y + 1] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].x > 0 && MapControl.MoveState2[(int)StartCoordinates[pos].x - 1, (int)StartCoordinates[pos].y] == true)
            {
                Vector2 leftTile = new Vector2(StartCoordinates[pos].x - 1, StartCoordinates[pos].y);
                CoordinateInformation(leftTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x - 1, (int)StartCoordinates[pos].y] = ShortestDistance[pos] + 1;
            }
            if (StartCoordinates[pos].x < 7 && MapControl.MoveState2[(int)StartCoordinates[pos].x + 1, (int)StartCoordinates[pos].y] == true)
            {
                Vector2 rightTile = new Vector2(StartCoordinates[pos].x + 1, StartCoordinates[pos].y);
                CoordinateInformation(rightTile, ShortestDistance[pos] + 1);
                MapControl.MoveState3[(int)StartCoordinates[pos].x + 1, (int)StartCoordinates[pos].y] = ShortestDistance[pos] + 1;
            }

            pos++;

        }
        BFS2(a, StartCoordinates[0], true);

    }

    void BFS2(Vector2 _first, Vector2 _first2, bool isFirst)
    {
        Clear();

        if (isFirst)
        {
            cnt2 = 0;
            pos2 = 0;
        }

        CoordinateInformation2(_first, 1);

        while (pos2 < cnt2 && (StartCoordinates2[pos2].x != _first2.x || StartCoordinates2[pos2].y != _first2.y))
        {

            MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y] = 0;


            if (StartCoordinates2[pos2].y > 0 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y - 1] == ShortestDistance[pos] - 1)
            {
                Vector2 upTile = new Vector2(StartCoordinates2[pos2].x, StartCoordinates2[pos2].y - 1);
                CoordinateInformation2(upTile, ShortestDistance2[pos2] + 1);
            }
            else if (StartCoordinates2[pos2].y < 7 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x, (int)StartCoordinates2[pos2].y + 1] == ShortestDistance[pos] - 1)
            {
                Vector2 downTile = new Vector2(StartCoordinates2[pos2].x, StartCoordinates2[pos2].y + 1);
                CoordinateInformation2(downTile, ShortestDistance2[pos2] + 1);
            }
            else if (StartCoordinates2[pos2].x > 0 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x - 1, (int)StartCoordinates2[pos2].y] == ShortestDistance[pos] - 1)
            {
                Vector2 leftTile = new Vector2(StartCoordinates2[pos2].x - 1, StartCoordinates2[pos2].y);
                CoordinateInformation2(leftTile, ShortestDistance2[pos2] + 1);

            }
            else if (StartCoordinates2[pos2].x < 7 && MapControl.MoveState3[(int)StartCoordinates2[pos2].x + 1, (int)StartCoordinates2[pos2].y] == ShortestDistance[pos] - 1)
            {
                Vector2 rightTile = new Vector2(StartCoordinates2[pos2].x + 1, StartCoordinates2[pos2].y);
                CoordinateInformation2(rightTile, ShortestDistance2[pos2] + 1);
            }

            ShortestDistance[pos] -= 1;


            if (pos2 < cnt2)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (MapControl.MoveState[i, j] == true && MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != true)
                        {
                            MoveDirection(i, j);
                        }
                    }
                }
            }
            pos2++;
        }

    }

    void MoveDirection(int i, int j)
    {
        int count = pos2 + 1;

        while (count != 0)
        {
            if (count == pos2 - 1)
            {
                if ((StartCoordinates[0].y > StartCoordinates2[1].y && StartCoordinates[0].x < StartCoordinates2[1].x))
                {
                    if ((StartCoordinates[0].y > StartCoordinates2[2].y))
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                    }
                }
                else if ((StartCoordinates[0].y < StartCoordinates2[1].y && StartCoordinates[0].x < StartCoordinates2[1].x))
                {
                    if ((StartCoordinates[0].y < StartCoordinates2[2].y))
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                    }
                }
                if ((StartCoordinates[0].y < StartCoordinates2[1].y && StartCoordinates[0].x > StartCoordinates2[1].x))
                {
                    if ((StartCoordinates[0].y < StartCoordinates2[2].y))
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                    }
                }
                else if ((StartCoordinates[0].y > StartCoordinates2[1].y && StartCoordinates[0].x > StartCoordinates2[1].x))
                {
                    if ((StartCoordinates[0].y > StartCoordinates2[2].y))
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                    }

                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                    }
                }
                else if (StartCoordinates[0].y == StartCoordinates2[1].y)
                {
                    MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                }
                else if (StartCoordinates[0].x == StartCoordinates2[1].x)
                {
                    MapControl.MapTileArray[(int)StartCoordinates2[2].x, (int)StartCoordinates2[2].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                }

            }

            else if (count == pos2)
            {
                if ((StartCoordinates[0].y > StartCoordinates2[0].y && StartCoordinates[0].x < StartCoordinates2[0].x))
                {
                    if (StartCoordinates[0].y - 2 == StartCoordinates2[0].y && StartCoordinates2[2].x == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y == StartCoordinates2[0].y && StartCoordinates2[2].x + 2 == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates[0].y > StartCoordinates2[1].y && StartCoordinates[0].x == StartCoordinates2[1].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y > StartCoordinates2[0].y && StartCoordinates2[2].x < StartCoordinates2[0].x)
                    {
                        if (StartCoordinates2[1].y == StartCoordinates2[0].y)
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                        }
                        else
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                    }
                }
                else if (StartCoordinates[0].y < StartCoordinates2[0].y && StartCoordinates[0].x < StartCoordinates2[0].x)
                {
                    if (StartCoordinates[0].y + 2 == StartCoordinates2[0].y && StartCoordinates2[2].x == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y == StartCoordinates2[0].y && StartCoordinates2[2].x + 2 == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates[0].y < StartCoordinates2[1].y && StartCoordinates[0].x == StartCoordinates2[1].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y < StartCoordinates2[0].y && StartCoordinates2[2].x < StartCoordinates2[0].x)
                    {
                        if (StartCoordinates2[1].y == StartCoordinates2[0].y)
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                        }
                        else
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                    }
                }
                else if (StartCoordinates[0].y < StartCoordinates2[0].y && StartCoordinates[0].x > StartCoordinates2[0].x)
                {
                    if (StartCoordinates[0].y + 2 == StartCoordinates2[0].y && StartCoordinates2[2].x == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y == StartCoordinates2[0].y && StartCoordinates2[2].x - 2 == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates[0].y < StartCoordinates2[1].y && StartCoordinates[0].x == StartCoordinates2[1].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y < StartCoordinates2[0].y && StartCoordinates2[2].x > StartCoordinates2[0].x)
                    {
                        if (StartCoordinates2[1].y == StartCoordinates2[0].y)
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UL).gameObject.SetActive(true);
                        }
                        else
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DR).gameObject.SetActive(true);
                    }
                }
                else if (StartCoordinates[0].y > StartCoordinates2[0].y && StartCoordinates[0].x > StartCoordinates2[0].x)
                {
                    if (StartCoordinates[0].y - 2 == StartCoordinates2[0].y && StartCoordinates2[2].x == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y == StartCoordinates2[0].y && StartCoordinates2[2].x - 2 == StartCoordinates2[0].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates[0].y > StartCoordinates2[1].y && StartCoordinates[0].x == StartCoordinates2[1].x)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[2].y > StartCoordinates2[0].y && StartCoordinates2[2].x > StartCoordinates2[0].x)
                    {
                        if (StartCoordinates2[1].y == StartCoordinates2[0].y)
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_UR).gameObject.SetActive(true);
                        }
                        else
                        {
                            MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Corner_DL).gameObject.SetActive(true);
                    }
                }
                else if (StartCoordinates[0].y == StartCoordinates2[0].y)
                {
                    MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_UD).gameObject.SetActive(true);
                }
                else if (StartCoordinates[0].x == StartCoordinates2[0].x)
                {
                    MapControl.MapTileArray[(int)StartCoordinates2[1].x, (int)StartCoordinates2[1].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_LR).gameObject.SetActive(true);
                }
            }
            else if (count == pos2 + 1)
            {

                if (pos2 == 1)
                {
                    if (StartCoordinates2[0].y < StartCoordinates2[1].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Left).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].y > StartCoordinates2[1].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Right).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].x < StartCoordinates2[1].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Up).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].x > StartCoordinates2[1].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Down).gameObject.SetActive(true);
                    }

                }
                else if (pos2 == 0)
                {
                    if (StartCoordinates2[0].y < StartCoordinates[0].y && StartCoordinates2[0].x == StartCoordinates[0].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Left).gameObject.SetActive(true);
                        MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Left).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].y > StartCoordinates[0].y && StartCoordinates2[0].x == StartCoordinates[0].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Right).gameObject.SetActive(true);
                        MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Right).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].x < StartCoordinates[0].x && StartCoordinates2[0].y == StartCoordinates[0].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Up).gameObject.SetActive(true);
                        MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Up).gameObject.SetActive(true);
                    }
                    else if (StartCoordinates2[0].x > StartCoordinates[0].x && StartCoordinates2[0].y == StartCoordinates[0].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
                    {
                        MapControl.MapTileArray[(int)StartCoordinates2[0].x, (int)StartCoordinates2[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_End_Down).gameObject.SetActive(true);
                        MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Down).gameObject.SetActive(true);
                    }
                }

            }

            if (StartCoordinates2[pos2].y < StartCoordinates[0].y && StartCoordinates2[pos2].x == StartCoordinates[0].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
            {
                MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Left).gameObject.SetActive(true);
            }
            else if (StartCoordinates2[pos2].y > StartCoordinates[0].y && StartCoordinates2[pos2].x == StartCoordinates[0].x && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
            {
                MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Right).gameObject.SetActive(true);
            }
            else if (StartCoordinates2[pos2].x < StartCoordinates[0].x && StartCoordinates2[pos2].y == StartCoordinates[0].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
            {
                MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Up).gameObject.SetActive(true);
            }
            else if (StartCoordinates2[pos2].x > StartCoordinates[0].x && StartCoordinates2[pos2].y == StartCoordinates[0].y && MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] != false)
            {
                MapControl.MapTileArray[(int)StartCoordinates[0].x, (int)StartCoordinates[0].y].transform.GetChild(MapTile.ROAD).GetChild(MapTile.Road_Down).gameObject.SetActive(true);
            }

            count--;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 14; k++)
                {
                    MapControl.MapTileArray[i, j].transform.GetChild(MapTile.ROAD).GetChild(k).gameObject.SetActive(false);
                }
            }
        }

    }

    public void FindFacePicture(GameObject obj)
    {
        if (obj.tag == "Character" || obj.tag == "Enemy")
        {
            string name = obj.name;
            GameObject fp;
            int i = GetFaceIndex(ProfileMenuBottom, name);
            fp = ProfileMenuBottom.transform.GetChild(i).gameObject;
            obj.GetComponent<Unit>().FacePicture = fp;
        }
    }

    int GetFaceIndex(GameObject obj, string name) //얼굴사진과 오브젝트의 이름이 같으면 FacePicture 폴더안의 ChildCount 를 리턴. 못찾았다면 -1 리턴
    {
        int index;
        index = obj.transform.GetChildCount();

        for (int i = 0; i < index; i++)
        {
            if (obj.transform.GetChild(i).name == name)
            {
                return i;
            }

        }
        return -1;
    }

    public void ShowProfileMenuBottom()
    {
        ProfileMenuBottom.SetActive(true);
    }

    public void ClearProfileMenuBottom()
    {
        ProfileMenuBottom.SetActive(false);
    }

    public void Hit(int x1, int y1, int x2, int y2) //단순히 from의 무기데미지만큼 to에게 체력을 깍는다
    {
        Unit From, To;
        GameObject FromObj, ToObj;
        FromObj = MapControl.MapObjectArray[x1, y1];
        From = FromObj.GetComponentInChildren<Unit>();
        if (MapControl.MapObjectArray[x2, y2] != null)
        {
            ToObj = MapControl.MapObjectArray[x2, y2];
            To = ToObj.GetComponentInChildren<Unit>();
            To.health = To.health - From.weapon.Dmg;

            s.SoundPlay("EffectSound/prop_earthmover_attack_first");
            //Debug.Log(FromObj.name + " damages " + ToObj.name + " for " + From.weapon.Dmg + "!!!");
            //Debug.Log(ToObj.name + "'s hp is : " + To.health);

            if (MapControl.MapTileArray[x2,y2].GetComponent<MapTile>().Forest)
            {
                MapControl.MapTileArray[x2, y2].GetComponent<MapTile>().isBurnForest = true;
                s.SoundPlay("EffectSound/prop_forest_fire");
            }

            return;
        }
        else
        {
            //Debug.Log("hit - 안맞음");

            if (MapControl.MapTileArray[x2, y2].GetComponent<MapTile>().Forest)
            {
                MapControl.MapTileArray[x2, y2].GetComponent<MapTile>().isBurnForest = true;
                s.SoundPlay("EffectSound/prop_forest_fire");
            }

            return;
        }
    }

}
