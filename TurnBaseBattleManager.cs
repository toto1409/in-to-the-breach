using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnBaseBattleManager : MonoBehaviour
{
    public static TurnBaseBattleManager Inst = null;

    // 턴 시스템 단계 정의
    public enum BattleStates
    {
        PLAYERPOSITIONSET,  // 플레이어 캐릭터 배치 단계
        ENEMYTURN, // 적군 턴
        PLAYERTURN, // 플레이어 턴
        ENDBATTLE // 전투 종료
    }

    // 사용될 스크립트 변수들
    PlayerPositionSet playerPositionSet;
    ClickManager clickManager;
    Spawn spawn;

    public List<GameObject> playerList;
    public List<GameObject> enemyList;
    public List<GameObject> objectList;
    public BattleStates currentState; // 현재 단계를 저장할 변수
    public int RemainTurn;
    public Text turnTitleText;
    public SpriteRenderer turnTitlebar;
    public bool turnTitleOn;
    public float turnTitleAlpha;
    public int time;
    public bool addListCharacterCheak = true;
    public bool addListEnemyCheak = true;
    public bool addListObjectCheak = true;
    public bool turnTitleFirst = true;
    public bool turnTitlePrint = true;
    public bool turnTitleWait = false;
    public bool blockMouseClick; // Move 중일때 blockMouseClick 변수가 true이면 다른 객체는 마우스 클릭 선택이 되지 않음.
    public bool isFirstEnemy = true;
    public bool nextEnemyFlag = true;
    public bool enemyAttack = false;
    public bool fireDamege = true;
    public bool enemyMove = false;
    public int nextEnemyIndex;
    public bool enemySpawn = true;
    public bool playerTurnChange = true;
    GameObject remainTurnNum;
    public int enegy;
    Animator enegyAnimator;
    public bool gameWin, gameLose;
    public float startTime;
    public Sound s;
    public AudioSource bgm;


    private void Awake()
    {
        //DataBase.playerMech[0] = 3; // ##수정필요## 앞 씬에서 선택하도록
        //DataBase.playerMech[1] = 4; // ##수정필요## 앞 씬에서 선택하도록
        //DataBase.playerMech[2] = 5; // ##수정필요## 앞 씬에서 선택하도록
        //DataBase.playMapID = 2; // ##수정필요## 앞 씬에서 선택하도록

        Inst = this;
    }

    void Start()
    {
        currentState = BattleStates.PLAYERPOSITIONSET; // 게임 시작시 플레이어 캐릭터 배치 단계부터 시작
        playerPositionSet = GetComponent<PlayerPositionSet>();
        clickManager = GetComponent<ClickManager>();
        spawn = Spawn.GetInst();

        RemainTurn = 5;
        blockMouseClick = false;
        remainTurnNum = GameObject.Find("Num");
        enegy = 7;
        enegyAnimator = GameObject.Find("startingGameUI").transform.GetChild(0).GetChild(1).GetComponent<Animator>();
        startTime = Time.time;
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
        bgm = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        GameReset();
    }

    private void Update()
    {
        if(currentState != BattleStates.ENDBATTLE && currentState != BattleStates.PLAYERPOSITIONSET)
        {
            enegyAnimator.SetInteger("Enegy", enegy);
        }

        switch (currentState)
        {
            case BattleStates.PLAYERPOSITIONSET:
                {
                    AddEnemyToList(); // 리스트에 몬스터들 등록
                    AddObstacleToList(); // 리스트에 맵 오브젝트들 등록
                    playerPositionSet.SelectablePlayerPosView(); // 플레이어 캐릭터 배치 범위 표시
                    playerPositionSet.DeployPlayer();  // 플레이어 캐릭터를 배치
                }
                break;
            case BattleStates.ENEMYTURN:
                {
                    AddPlayerToList(); // 리스트에 캐릭터들 등록
                    PrintTurntitle(); // 턴 타이틀 출력 ("적군 턴")
                    TurnTitleBarFade();

                    if (turnTitleWait)
                    {
                        EnemyTurnProgress(); // 몬스터 한마리씩 행동 진행
                    }
                }
                break;
            case BattleStates.PLAYERTURN:
                {
                    PrintTurntitle(); // 턴 타이틀 출력 ("플레이어 턴")
                    TurnTitleBarFade();

                    if (blockMouseClick == false && MapControl.isMouseIn)
                    {
                        clickManager.ClickCheck(); // 마우스 클릭 체크
                    }

                    ProfileMiddleOffCheck();
                    PlayerInputCheak();
                }
                break;
            case BattleStates.ENDBATTLE:
                if (gameWin)
                {
                    gameWin = false;
                    GameObject.Find("GameUICanvas").gameObject.SetActive(false);
                    StartCoroutine(GameWinToolTipPrint());
                }
                else if (gameLose) 
                {
                    gameLose = false;
                    GameObject.Find("GameUICanvas").gameObject.SetActive(false);
                    StartCoroutine(GameLoseAnimation());
                }
                break;
        }

        if (currentState != BattleStates.PLAYERPOSITIONSET && currentState != BattleStates.ENDBATTLE)
        {
            if (RemainTurn == 0 || enemyList.Count == 0) // 이겼을 경우
            {
                gameWin = true;
                currentState = BattleStates.ENDBATTLE;
            }
            else if(playerList.Count == 0 || enegy == 0) // 졌을 경우
            {
                gameLose = true;
                currentState = BattleStates.ENDBATTLE;
            }
        }
    }

    public static TurnBaseBattleManager GetInst()
    {
        return Inst;
    }

    void AddPlayerToList()
    {
        if (addListCharacterCheak)
        {
            playerList.AddRange(GameObject.FindGameObjectsWithTag("Character"));
            addListCharacterCheak = false;
        }
    }

    void AddEnemyToList()
    {
        if (addListEnemyCheak)
        {
            enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            addListEnemyCheak = false;
        }
    }

    void AddObstacleToList()
    {
        if (addListObjectCheak)
        {
            objectList.AddRange(GameObject.FindGameObjectsWithTag("Building"));
            addListObjectCheak = false;
        }
    }

    void PrintTurntitle()
    {
        if (turnTitlePrint)
        {
            time = (int)Time.time;
            Turntitle();
            turnTitlePrint = false;
        }

        if (time + 2 == (int)(Time.time))
        {
            turnTitleOn = false;

            if(BurnForestCheck())
            {
                Invoke("EnemyTurnProgressDelay", 1f);
            }
            else
            {
                turnTitleWait = true;
            }
        }
    }

    void Turntitle()
    {
        if (turnTitleFirst)
        {
            turnTitleText.text = "임무 개시";
            turnTitleFirst = false;
            bgm.Play();
            s.SoundPlay("EffectSound/ui_battle_mission_start");
        }
        else
        {
            switch (currentState)
            {
                case BattleStates.ENEMYTURN:
                    turnTitleText.text = "적군 턴";
                    s.SoundPlay("EffectSound/ui_battle_enemy_turn", 1f);
                    break;
                case BattleStates.PLAYERTURN:
                    turnTitleText.text = "플레이어 턴";
                    s.SoundPlay("EffectSound/ui_battle_end_turn_notification");
                    break;
            }
        }

        turnTitleOn = true;
    }

    void TurnTitleBarFade()
    {
        if(turnTitleOn)
        {
            if (turnTitleAlpha < 1f)
            {
                turnTitleAlpha += 0.1f;
                turnTitlebar.color = new Color(turnTitlebar.color.r, turnTitlebar.color.g, turnTitlebar.color.b, turnTitleAlpha);
                turnTitleText.color = new Color(turnTitleText.color.r, turnTitleText.color.g, turnTitleText.color.b, turnTitleAlpha);
            }
        }
        else
        {
            if (turnTitleAlpha > 0f)
            {
                turnTitleAlpha -= 0.1f;
                turnTitlebar.color = new Color(turnTitlebar.color.r, turnTitlebar.color.g, turnTitlebar.color.b, turnTitleAlpha);
                turnTitleText.color = new Color(turnTitleText.color.r, turnTitleText.color.g, turnTitleText.color.b, turnTitleAlpha);
            }
        }
    }

    void PlayerInputCheak()
    {
        if (Input.GetKeyDown("space")) // 스페이스 바
        {
            Functions f = Functions.GetInst();
            f.MoveRangClear();
            if (clickManager.player != null)
            {
                clickManager.player.ClickOn = false;
            }

            turnTitleWait = false;
            turnTitlePrint = true;
            currentState = BattleStates.ENEMYTURN;

            foreach (GameObject player in playerList)
            {
                player.GetComponent<Player>().MoveAvailable = true;
                player.GetComponent<Player>().Mode = Unit.MODE.None;
            }
        }
    }

    public void PlayerEndTurn()
    {
        Functions f = Functions.GetInst();
        f.MoveRangClear();
        if (clickManager.player != null)
        {
            clickManager.player.ClickOn = false;
        }

        turnTitleWait = false;
        turnTitlePrint = true;
        currentState = BattleStates.ENEMYTURN;

        foreach (GameObject player in playerList)
        {
            player.GetComponent<Player>().MoveAvailable = true;
            player.GetComponent<Player>().Mode = Unit.MODE.None;
        }
    }

    void EnemyTurnProgressDelay()
    {
        turnTitleWait = true;
    }

    void EnemyTurnProgress()
    {
        if(fireDamege)
        {
            fireDamege = false;

            if(BurnForestCheck())
            {
                FireDamege();
                Invoke("FireDamegeDelay", 1.5f);
            }
            else
            {
                enemyAttack = true;
            }
        }

        if (enemyAttack)
        {
            if (nextEnemyFlag)
            {
                nextEnemyFlag = false;

                if (GetNextAttackEnemyIndex() != -1)
                {
                    if (enemyList[nextEnemyIndex].GetComponent<EnemyAI>().AttackLockOn)
                    {
                        Invoke("NextEnemyAttakDelay", 2f);
                    }
                    else
                    {
                        enemyList[nextEnemyIndex].GetComponent<Enemy>().Mode = Enemy.MODE.Attack;
                        nextEnemyFlag = true;
                    }
                }
                else
                {
                    enemyAttack = false;
                    nextEnemyFlag = true;

                    if (RemainTurn < 5 && enemySpawn)
                    {
                        spawn.EnemyCount(); // 출현 예정 지점에서 몬스터 출현
                        enemySpawn = false;
                        Invoke("EnemyMoveStartDelay", 2f);
                    }
                    else
                    {
                        enemyMove = true;
                    }
                }
            }
        }

        if (enemyMove)
        {
            if (nextEnemyFlag)
            {
                nextEnemyFlag = false;
                Invoke("NextenemyMove", 2f);
            }
        }

        if (!enemyAttack && !enemyMove)
        {
            EnemyTurnEndCheck();
        }
    }

    bool BurnForestCheck()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (MapControl.MapTileArray[i, j].GetComponent<MapTile>().isBurnForest)
                {
                    if (MapControl.MapObjectArray[i, j] != null)
                    {
                        if (MapControl.MapObjectArray[i, j].tag == "Character" || MapControl.MapObjectArray[i, j].tag == "Enemy")
                        {
                            if (MapControl.MapObjectArray[i, j].GetComponent<Unit>().health > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    void FireDamege()
    {
        s.SoundPlay("EffectSound/prop_forest_fire");

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(MapControl.MapTileArray[i,j].GetComponent<MapTile>().isBurnForest)
                {
                    if (MapControl.MapObjectArray[i, j] != null)
                    {
                        if (MapControl.MapObjectArray[i, j].tag == "Character" || MapControl.MapObjectArray[i, j].tag == "Enemy")
                        {
                            GameObject gameObject = MapControl.MapObjectArray[i, j];
                            Unit unit = gameObject.GetComponent<Unit>();

                            if (unit.health > 0)
                            {
                                unit.health -= 1;

                                GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
                                GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
                                tooltip.GetComponent<ToolTip>().mode = "FireDamege";
                                tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
                                tooltip.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f);
                                tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                            }
                        }
                    }
                }
            }
        }
    }

    void FireDamegeDelay()
    {
        enemyAttack = true;
    }

    int GetNextAttackEnemyIndex()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Enemy enemy = enemyList[i].GetComponent<Enemy>();

            if (enemy.Mode != Enemy.MODE.Attack)
            {
                nextEnemyIndex = i;
                return nextEnemyIndex;
            }
        }

        nextEnemyIndex = -1;
        return nextEnemyIndex;
    }

    void NextEnemyAttakDelay()
    {
        nextEnemyFlag = true;
        enemyList[nextEnemyIndex].GetComponent<Enemy>().Mode = Enemy.MODE.Attack;
    }

    void EnemyMoveStartDelay()
    {
        enemyMove = true;
    }

    void NextenemyMove()
    {
        nextEnemyFlag = true;

        for (int i = 0; i < enemyList.Count; i++)
        {
            Enemy enemy = enemyList[i].GetComponent<Enemy>();

            if (enemy.Mode == Enemy.MODE.Move)
            {
                print("아직 덜 움직임 앞에 얘");
                return;
            }

            if (enemy.Mode == Enemy.MODE.Attack)
            {
                enemy.disableActive = false;
                enemy.Mode = Enemy.MODE.Move;
                //print(enemyList[i].name + " 움직임");
                return;
            }
        }

        enemyMove = false;
    }

    void EnemyTurnEndCheck()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy.GetComponent<Enemy>().Mode != Unit.MODE.Done) { return; }
        }

        if (playerTurnChange)
        {
            playerTurnChange = false;

            Functions f = Functions.GetInst();
            f.MoveRangClear();

            spawn.PointCount(); // 적 출현 위치 지정
            Invoke("PlayTurnChange", 2f);
        }
    }

    void PlayTurnChange()
    {
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().Mode = Unit.MODE.None;
            enemy.GetComponent<Enemy>().MoveAvailable = true;
            enemy.GetComponent<Enemy>().disableActive = false;
        }

        fireDamege = true;
        enemyAttack = false;
        enemyMove = false;

        turnTitlePrint = true;
        turnTitleWait = false;
        enemySpawn = true;
        playerTurnChange = true;

        RemainTurn--;
        for (int i = 0; i <= 5; i++)
        {
            remainTurnNum.transform.GetChild(i).gameObject.SetActive(false);
        }
        remainTurnNum.transform.GetChild(RemainTurn).gameObject.SetActive(true);

        currentState = BattleStates.PLAYERTURN;
    }

    public void GameReset()
    {
        Functions f = Functions.GetInst();
        f.EnemyTargetedClear();
    }

    void ProfileMiddleOffCheck()
    {
        if(Input.GetMouseButtonDown(1))
        {
            UIControl.GetInst().proflieMiddlePick = false;
            ProfileInfo.GetInst().ClearProfile();
            for (int i = 0; i < 3; i++)
            {
                GameObject.Find("startingGameUI").transform.GetChild(2).GetChild(1).GetChild(i).GetComponent<hirightControl>().player.ClickOn = false;
                GameObject.Find("startingGameUI").transform.GetChild(2).GetChild(1).GetChild(i).GetComponent<hirightControl>().ClearProInfo();
            }
        }
    }

    IEnumerator GameWinToolTipPrint()
    {
        bgm.Stop();
        s.SoundPlay("BackgroundSound/title_ending", 1f);
        for (int i = 0; i < objectList.Count; i++) 
        {
            GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
            GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
            tooltip.GetComponent<ToolTip>().mode = "EndToolTip";
            tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
            tooltip.transform.position = new Vector2(objectList[i].transform.position.x, objectList[i].transform.position.y + 0.3f);
            tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            yield return new WaitForSeconds(1f);
        }
        s.SoundPlay("BackgroundSound/victory", 1f);
        yield return new WaitForSeconds(1.5f);
        UIControl.GetInst().GameWin();
    }

    IEnumerator GameLoseAnimation()
    {
        bgm.Stop();
        s.SoundPlay("BackgroundSound/title_ending", 1f);
        for (int k = 0; k < 4; k++)
        {
            for (int num = 0; num < 8; num++) 
            {
                int i = Random.Range(0, 8);
                int j = Random.Range(0, 8);

                if (MapControl.MapObjectArray[i, j] == null && MapControl.MapTileArray[i, j].GetComponent<MapTile>().Water == false)
                {
                    spawn.Spawns(i, j);
                    s.SoundPlay("EffectSound/ui_battle_enemy_emerge_warning_rubble_01",0.3f);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        s.SoundPlay("BackgroundSound/gameover", 1f);
        yield return new WaitForSeconds(1.5f);
        UIControl.GetInst().GameLose();
    }
}
