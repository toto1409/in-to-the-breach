using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public static Spawn Inst = null;
    public int x;
    public int y;
    public GameObject Enemy;
    public int[] SpawnImageX = new int[10]; //이전 숫자를 받을 인수
    public int[] SpawnImageY = new int[10];//위와 동일
    public int[] Enemys = new int[6];
    public int CountEnemy = 0;
    int CountStackEnemy = 0;
    public int spawnImageCount = 0;
    //Enemy하위 상수 값
    public const int Image = 0;
    //Enemy-Image관련 상수값 정의
    public const int Firefly = 0;
    public const int Spider = 1;
    public const int Beetle = 2;
    public const int Scarab = 3;
    public const int Squid = 4;
    public int EnemyIdRandom;
    public int StageSelect;
    public Animator ani;
    public int SpawnMin = Firefly;
    public int SpawnMax = Beetle + 1;
    public int StartEnemy;

    private void Awake()
    {
        Inst = this;
        ani = GetComponent<Animator>();
        for (int i = 0; i < 10; i++)
        {
            SetSpawnArrayEmpty(i);
        }
    }

    void SetSpawnArrayEmpty(int i)
    {
        SpawnImageX[i] = SpawnImageY[i] = -1;
    }

    void SpawnArraySortAll()
    {
        for (int i = 0; i < 9; i++)
        {
            if (SpawnImageX[i] == -1)
            {
                SpawnImageX[i] = SpawnImageX[i + 1];
                SpawnImageY[i] = SpawnImageY[i + 1];
            }
        }
    }

    void Start()
    {
        EnemySpawn();
    }

    public static Spawn GetInst()
    {
        return Inst;
    }

    public void EnemyCount()
    {
        int count = 0;
        if (CountEnemy < 12)
        {
            if (TurnBaseBattleManager.GetInst().RemainTurn < 5)
            {
                for (int i = 0; i < spawnImageCount; i++)
                {
                    if (CheckSpawnPointForSpawn(SpawnImageX[i], SpawnImageY[i]) == true)
                    {
                        Spawns(i);
                        count++;
                    }
                    else
                    {
                        BlockSpawnPointDamage(SpawnImageX[i], SpawnImageY[i]);
                    }
                }
                spawnImageCount = spawnImageCount - count;
                SpawnArraySortAll();

            }

        }
        if (12 <= CountEnemy)
        {
            Debug.Log("생성이 불가능합니다.");
        }
    }

    public void PointCount()
    {
        int SpawnNumberRandom = Random.Range(3, 4);//2~3
        for (int s = 0; s < SpawnNumberRandom; s++)
        {
            SetSpawnPoint();
            
        }
    }

    //void NomalEnemy()
    //{
    //    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    //    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
    //    enemy.transform.position = MapControl.Cell[3, 7];
    //    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Firefly).name + "_" + CountStackEnemy;
    //    enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Firefly).gameObject.SetActive(true);
    //    enemy.GetComponent<Enemy>().enemyID = Spawn.Firefly;
    //    MapControl.MapObjectArray[3, 7] = enemy;
    //    MapControl.MapTileArray[3, 7].GetComponent<MapTile>().Moveable = false;
    //    CountEnemy++;
    //    CountStackEnemy++;
    //}

    //void NomalEnemy2()
    //{
    //    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    //    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
    //    enemy.transform.position = MapControl.Cell[6, 7];
    //    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Spider).name + "_" + CountStackEnemy;
    //    enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Spider).gameObject.SetActive(true);
    //    enemy.GetComponent<Enemy>().enemyID = Spawn.Spider;
    //    MapControl.MapObjectArray[6, 7] = enemy;
    //    MapControl.MapTileArray[6, 7].GetComponent<MapTile>().Moveable = false;
    //    CountEnemy++;
    //    CountStackEnemy++;
    //}

    //void NomalEnemy3()
    //{
    //    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    //    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
    //    enemy.transform.position = MapControl.Cell[6, 6];
    //    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Firefly).name + "_" + CountStackEnemy;
    //    enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Firefly).gameObject.SetActive(true);
    //    enemy.GetComponent<Enemy>().enemyID = Spawn.Firefly;
    //    MapControl.MapObjectArray[6, 6] = enemy;
    //    MapControl.MapTileArray[6, 6].GetComponent<MapTile>().Moveable = false;
    //    CountEnemy++;
    //    CountStackEnemy++;
    //}

    //void NomalEnemy4()
    //{
    //    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    //    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
    //    enemy.transform.position = MapControl.Cell[5, 5];
    //    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Beetle).name + "_" + CountStackEnemy;
    //    enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Beetle).gameObject.SetActive(true);
    //    enemy.GetComponent<Enemy>().enemyID = Spawn.Beetle;
    //    MapControl.MapObjectArray[5, 5] = enemy;
    //    MapControl.MapTileArray[5, 5].GetComponent<MapTile>().Moveable = false;
    //    CountEnemy++;
    //    CountStackEnemy++;
    //}

    //void NomalEnemy5()
    //{
    //    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    //    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
    //    enemy.transform.position = MapControl.Cell[2, 6];
    //    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Scarab).name + "_" + CountStackEnemy;
    //    enemy.transform.GetChild(Spawn.Image).GetChild(Spawn.Scarab).gameObject.SetActive(true);
    //    enemy.GetComponent<Enemy>().enemyID = Spawn.Scarab;
    //    MapControl.MapObjectArray[2, 6] = enemy;
    //    MapControl.MapTileArray[2, 6].GetComponent<MapTile>().Moveable = false;
    //    CountEnemy++;
    //    CountStackEnemy++;
    //}

    void EnemySpawn()
    {
        for (int s = 0; s < 3; s++)
        {
            x = Random.Range(1, 7);
            y = Random.Range(5, 8);
            StartEnemy = Random.Range(SpawnMin, SpawnMax);
            Enemys[s] = StartEnemy;
            if (CheckSpawnPointForSpawn(x, y) == true)
            {
                if (CheckSpawnPointForImage(x, y) == true)
                {
                    GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
                    GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
                    enemy.transform.position = MapControl.Cell[x, y];
                    enemy.GetComponent<Enemy>().enemyID = Enemys[s];
                    enemy.GetComponent<Unit>().unitID = Enemys[s];
                    enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(Enemys[s]).name + "_" + CountStackEnemy;
                    enemy.transform.GetChild(Spawn.Image).GetChild(Enemys[s]).gameObject.SetActive(true);
                    enemy.GetComponent<Enemy>().enemyName = SetEnemyName(Enemys[s]);
                    MapControl.MapObjectArray[x, y] = enemy;
                    MapControl.MapTileArray[x, y].GetComponent<MapTile>().Moveable = false;
                    CountEnemy++;
                    CountStackEnemy++;
                }
            }

        }
    }

    public void SetSpawnPoint()
    {
        x = Random.Range(1, 7);
        y = Random.Range(5, 8);
        if (CheckSpawnPointForImage(x, y) == true)
        {
            SpawnImageOn(x, y);

            SpawnImageX[spawnImageCount] = x;
            SpawnImageY[spawnImageCount] = y;
            spawnImageCount++;
        }
    }
    void SpawnImageOn(int x, int y)
    {
        MapControl.MapTileArray[x, y].transform.GetChild(5).GetChild(Image).gameObject.SetActive(true);
    }
    void SpawnImageOff(int x, int y)
    {
        MapControl.MapTileArray[x, y].transform.GetChild(5).GetChild(Image).gameObject.SetActive(false);
    }
    bool CheckSpawnPointForImage(int x, int y)
    {
        bool OkayToSpawn = true;
        if (spawnImageCount == 0)
        {
            return OkayToSpawn;
        }
        for (int i = 0; i < spawnImageCount; i++)
        {
            if (SpawnImageX[i] == x && SpawnImageY[i] == y)
            {
                OkayToSpawn = false;
            }
            else
            {
                if (MapControl.MapTileArray[x, y].GetComponent<MapTile>().Moveable == false
                    || MapControl.MapTileArray[x, y].GetComponent<MapTile>().Water == true
                    || MapControl.MapObjectArray[x, y] != null) // 이동 불가능하며 물인곳
                {
                    OkayToSpawn = false;
                }
            }
        }

        return OkayToSpawn;
    }
    bool CheckSpawnPointForSpawn(int x, int y)
    {

        if (MapControl.MapTileArray[x, y].GetComponent<MapTile>().Moveable == true
            && MapControl.MapTileArray[x, y].GetComponent<MapTile>().Water == false) // 이동 가능하며 물이 아닌곳
        {
            return true;
        }

        return false;
    }

    public void Spawns(int i)
    {

        SpawnImageOff(SpawnImageX[i], SpawnImageY[i]);
        //EnemyIdRandom = Random.Range(Spawn.Firefly, Spawn.Scarab + 1);
        EnemyIdRandom = 3;

        GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
        GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
        enemy.transform.position = MapControl.Cell[SpawnImageX[i], SpawnImageY[i]];
        enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(EnemyIdRandom).name + "_" + CountStackEnemy;
        enemy.transform.GetChild(Spawn.Image).GetChild(EnemyIdRandom).gameObject.SetActive(true);
        enemy.GetComponent<Enemy>().enemyID = EnemyIdRandom;
        enemy.GetComponent<Unit>().unitID = EnemyIdRandom;
        enemy.GetComponent<Enemy>().enemyName = SetEnemyName(EnemyIdRandom);
        CountStackEnemy++;
        CountEnemy++;
        MapControl.MapObjectArray[SpawnImageX[i], SpawnImageY[i]] = enemy;
        MapControl.MapTileArray[SpawnImageX[i], SpawnImageY[i]].GetComponent<MapTile>().Moveable = false;
        TurnBaseBattleManager.GetInst().enemyList.Add(enemy);
    }

    public void Spawns(int i, int j)
    {
        EnemyIdRandom = Random.Range(Spawn.Firefly, Spawn.Scarab + 1);

        GameObject enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
        GameObject enemy = MonoBehaviour.Instantiate(enemyPrefab) as GameObject;
        enemy.transform.position = MapControl.Cell[i, j];
        enemy.name = "Enemy_" + enemy.transform.GetChild(Spawn.Image).GetChild(EnemyIdRandom).name + "_" + CountStackEnemy;
        enemy.transform.GetChild(Spawn.Image).GetChild(EnemyIdRandom).gameObject.SetActive(true);
        enemy.GetComponent<Enemy>().enemyID = EnemyIdRandom;
        enemy.GetComponent<Unit>().unitID = EnemyIdRandom;
        enemy.GetComponent<Enemy>().enemyName = SetEnemyName(EnemyIdRandom);

        MapControl.MapObjectArray[i, j] = enemy;
        MapControl.MapTileArray[i, j].GetComponent<MapTile>().Moveable = false;
        TurnBaseBattleManager.GetInst().enemyList.Add(enemy);
    }

    void SortArray(int i)
    {
        while (SpawnImageX[i + 1] != -1)
        {
            SpawnImageX[i] = SpawnImageX[i + 1];
            SpawnImageY[i] = SpawnImageY[i + 1];
            i++;
        }
    }
    public void BlockSpawnPointDamage(int x, int y)
    {
        GameObject obj = MapControl.MapObjectArray[x, y];
        SpawnImageOff(x, y);
        if (obj.tag == "Character" || obj.tag == "Enemy")
        {
            //Debug.Log("x: " + x + " y: " + y + "출현 봉쇄 데미지 1");
            MapControl.MapObjectArray[x, y].GetComponent<Unit>().health -= 1;

            GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
            GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
            tooltip.GetComponent<ToolTip>().mode = "BlockDamege";
            tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
            tooltip.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 0.3f);
            tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            return;
        }



    }

    string SetEnemyName(int _id)
    {
        string name = "";
        switch (_id)
        {
            case 0:
                name = "말벌";
                break;
            case 1:
                name = "전갈";
                break;
            case 2:
                name = "풍뎅이";
                break;
            case 3:
                name = "굴착 벌레";
                break;
            case 4:
                name = "전투 염력체";
                break;
        }
        return name;
    }
}