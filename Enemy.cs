using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public const int ATTACKDIRECTION = 1;

    public float dealaytime = 3f;
    public float dealay;
    public Animator ani;   //애니메이터
    public ClickManager cm;
    public bool fly;
    public int enemyID;
    public float time;
    public bool timeSet;
    public bool activation;
    public bool disableActive;
    public string enemyName;
    public bool deathFlag;
    

    void Start()
    {
        cm = ClickManager.GetInst();
        f = GameObject.Find("GameSystem").GetComponent<Functions>();
        ani = transform.GetChild(0).GetComponentInChildren<Animator>();
        animator2 = this.transform.GetChild(4).GetChild(0).GetComponent<Animator>();
        weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        Mode = MODE.None;
        ClickOn = false;
        MouseOn = false;
        MoveRangeOn = false;
        MoveAvailable = true;
        unitDie = false;
        onWater = false;
        timeSet = true;
        activation = false;

        SetEnemyStat();

        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);

        SetWeaponType(enemyID);
        SetWeaponName(enemyID);

        weapon = weapon.SearchWeaponType(WeaponType);
    }

    private void Update()
    {
        AnimationUpdate();
        HealthChangeCheck();

        // 불 타일에 위치해 있는지 체크
        if (MapControl.MapTileArray[x, y].GetComponent<MapTile>().isBurnForest)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }

        // 물 타일에 위치해 있는지 체크
        if (MapControl.MapTileArray[x, y].GetComponent<MapTile>().Water == true)
        {
            onWater = true;
        }
        else
        {
            onWater = false;
        }

        if (health > 0)
        {
            // 몬스터의 좌표인덱스 값은 실시간으로 새로 받아옴.
            x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
            y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);

            if (ClickOn) // 클릭된 상태의 경우
            {
                ShowOutline(); // 아웃라인 출력
                ShowMovementRange(); // 이동가능 범위 출력
                MoveRangeOn = true;
            }
            else if (MapControl.AttackState[x, y]) // 공격 범위일 경우 오브젝트 아웃라인 출력
            {
                ShowOutline(); // 아웃라인 출력
            }
            else if (activation) // 턴 진행 중인 경우
            {
                ShowOutline(); // 아웃라인 출력
            }
            else
            { 
                EraseOutline(); // 아웃라인 출력 비활성화
                ClearMovementRange(); // 이동가능 범위 출력 비활성화
                MoveRangeOn = false;
            }

            if (MouseOn) // 마우스 포인터가 올려져 있는 상태의 경우
            {
                ShowMovementRange(); // 이동 범위 출력
                MoveRangeOn = true;
            }
            else
            {
                if (!ClickOn) 
                {
                    ClearMovementRange(); // 이동 범위 출력 비활성화
                    MoveRangeOn = false;
                }
            }

            if(TurnBaseBattleManager.GetInst().currentState == TurnBaseBattleManager.BattleStates.ENEMYTURN)
            {
                if (Mode == MODE.Attack && GetComponent<EnemyAI>().AttackLockOn)
                {
                    showHpBar = true;
                    ProfileInfo.GetInst().enemyActive = true;
                    ProfileInfo.GetInst().enemyProfilePrint(this.gameObject, enemyID);
                }
                else if (Mode == MODE.Move)
                {
                    showHpBar = true;
                    ProfileInfo.GetInst().enemyActive = true;
                    ProfileInfo.GetInst().enemyProfilePrint(this.gameObject, enemyID);
                }
                else if (TurnBaseBattleManager.GetInst().currentState == TurnBaseBattleManager.BattleStates.ENEMYTURN && showHpBar)
                {
                    if (!disableActive)
                    {
                        disableActive = true;
                        Invoke("DisableEnemyActive", 1f);
                    }
                }
                else
                {
                    showHpBar = false;
                }
            }

            if (TurnBaseBattleManager.GetInst().currentState != TurnBaseBattleManager.BattleStates.ENEMYTURN) 
            {
                if(MapControl.Crt_X == x && MapControl.Crt_Y == y)// 마우스가 자신의 타일에 있을 때
                {
                    showHpBar = true;
                }
                else
                {
                    showHpBar = false;
                }
            }

            if (showHpBar || healthChange)
            {
                ShowHpBar();
            }
            else
            {
                DisableHpBar();
            }
        }
    }

    public void AnimationUpdate()
    {
        if (health <= 0)
        {
            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
            tm.enemyList.Remove(this.gameObject);
            MapControl.MapObjectArray[x, y] = null;

            if (onWater)
            {
                ani.SetBool("Dive", true);

                DeathDelay();
            }
            else
            {
                ani.SetBool("Dead", true);

                DeathDelay();
            }
        }

        if (onWater && !fly)
        {
            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
            tm.enemyList.Remove(this.gameObject);

            ani.SetBool("Dive", true);

            DeathDelay();
        }

        if (push)
        {
            transform.GetChild(0).GetChild(enemyID).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(4).GetChild(0).gameObject.SetActive(true);

            if (transform.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sprite == null)
            {
                transform.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Enemy/StateImage/" + unitID);
            }

            if ((int)pushVector.x == 0 && (int)pushVector.y == -1) // 왼쪽
            {
                if (pushBack)
                {
                    animator2.SetBool("backLeft", true);
                }
                else
                {
                    animator2.SetBool("pushLeft", true);
                }
            }
            else if ((int)pushVector.x == 0 && (int)pushVector.y == 1) // 오른쪽
            {
                if (pushBack)
                {
                    animator2.SetBool("backRight", true);
                }
                else
                {
                    animator2.SetBool("pushRight", true);
                }
            }
            else if ((int)pushVector.x == -1 && (int)pushVector.y == 0) // 위
            {
                if (pushBack)
                {
                    animator2.SetBool("backTop", true);
                }
                else
                {
                    animator2.SetBool("pushTop", true);
                }
            }
            else if ((int)pushVector.x == 1 && (int)pushVector.y == 0) // 아래
            {
                if (pushBack)
                {
                    animator2.SetBool("backBottom", true);
                }
                else
                {
                    animator2.SetBool("pushBottom", true);
                }
            }
        }
        else
        {
            transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
        }
    }

    void DeathDelay()
    {
        if (timeSet)
        {
            time = Time.time;
            timeSet = false;
            deathFlag = true;
        }

        if (time + 1.5f <= Time.time)
        {
            if (unitDie == false)
            {
                unitDie = true;
            }
            Spawn.GetInst().CountEnemy--;

            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(i).gameObject.SetActive(false);
            }
            MapControl.EnemyTargeted[GetComponent<EnemyAI>().targetX, GetComponent<EnemyAI>().targetY] = false;
            MapControl.MapTileArray[x, y].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            MapControl.MapObjectArray[x, y] = null;

            Destroy(this.gameObject);
        }
    }

    void ShowHpBar()
    {
        if (!healthBarEnable)
        {
            healthBarEnable = true;

            switch (maxHealth)
            {
                case 3:
                    transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                    break;
                case 2:
                    transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                    break;
            }
        }
    }

    void DisableHpBar()
    {
        if (healthBarEnable)
        {
            healthBarEnable = false;

            switch (maxHealth)
            {
                case 3:
                    transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                    break;
                case 2:
                    transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }
    }

    void DisableEnemyActive()
    {
        showHpBar = false;
        ProfileInfo.GetInst().enemyActive = false;
    }

    void SetEnemyStat()
    {
        if (enemyID == Spawn.Firefly)
        {
            maxHealth = 2;
            health = 2;
            movement = 5;
            attackRange = 1;
            fly = true;
            Mode = MODE.Attack;
        }
        if (enemyID == Spawn.Spider)
        {
            maxHealth = 3;
            health = 3;
            movement = 3;
            attackRange = 1;
            fly = false;
            Mode = MODE.Attack;
        }
        if (enemyID == Spawn.Beetle)
        {
            maxHealth = 3;
            health = 3;
            movement = 3;
            attackRange = 5;
            fly = false;
            Mode = MODE.Attack;
        }
        if (enemyID == Spawn.Scarab)
        {
            maxHealth = 2;
            health = 2;
            movement = 3;
            attackRange = 1;
            fly = false;
            Mode = MODE.Attack;
        }
        if (enemyID == Spawn.Squid)
        {
            maxHealth = 2;
            health = 2;
            movement = 2;
            attackRange = 3;
            fly = false;
            Mode = MODE.Attack;
        }
    }

}
