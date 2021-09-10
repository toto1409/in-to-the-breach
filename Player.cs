using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public const int Available = 1;

    public const int Combat_Mech = 0;
    public const int Artillery_Mech = 1;
    public const int Cannon_Mech = 2;

    public GameObject MouseOver;
    public int WeaponDmg;
    public int playerID;
    public string mechName;
    public Sound s;


    void Start()
    {
        f = GameObject.Find("GameSystem").GetComponent<Functions>();
        weapon = GameObject.Find("Weapon").GetComponent<Weapon>();
        animator = this.transform.GetChild(Unit.IMAGE).GetChild(playerID).GetComponent<Animator>();
        animator2 = this.transform.GetChild(4).GetChild(0).GetComponent<Animator>();
        Mode = MODE.None;

        ClickOn = false;
        MouseOn = false;
        MoveRangeOn = true;
        MoveAvailable = true;
        unitDie = false;
        onWater = false;
        enemySetMeTarget = 0;
        s = GameObject.Find("GameSystem").GetComponent<Sound>();

        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);

        SetWeaponType(playerID);
        SetWeaponName(playerID);

        weapon = weapon.SearchWeaponType(WeaponType);
    }

    void Update()
    {
        // 캐릭터의 좌표인덱스 값을 실시간으로 갱신.
        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);

        // 캐릭터 애니메이션 체크
        AnimationCheak();
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
            if (Input.GetMouseButtonDown(0))
            {
                if (Mode == Unit.MODE.Attack)
                {
                    if(MapControl.AttackState[MapControl.Crt_X,MapControl.Crt_Y])
                    {
                        weapon.Attack(x, y, MapControl.Crt_X, MapControl.Crt_Y);
                        Mode = Unit.MODE.Done;
                        MoveAvailable = false;
                    }
                    else
                    {
                        if (MoveAvailable)
                        {
                            Mode = Unit.MODE.None;
                        }
                        else
                        {
                            Mode = Unit.MODE.MoveEnd;
                        }
                    }
                    ClickManager.GetInst().attackOn = false;
                    f.AttackRangeClear();
                    ProfileInfo.GetInst().ClearProfile();
                    GameObject.Find("Profile_Player_Interface").gameObject.SetActive(false);
                }
            }

            if (ClickOn) // 클릭된 상태의 경우
            {
                showHpBar = true;
                ShowOutline(); // 아웃라인 출력

                if (MoveAvailable) // 이번 턴에 이동하지 않아 이동 가능할 경우
                {
                    ShowMovementRange(); // 이동 범위 출력
                    MoveRangeOn = true;
                    AttackRangeOn = true;
                }
            }
            else if (MapControl.AttackState[x, y]) // 공격 범위일 경우 오브젝트 아웃라인 출력
            {
                gameObject.GetComponentInChildren<Outline>().color = 0;
                ShowOutline(); // 아웃라인 출력
            }
            else
            {
                showHpBar = false;
                gameObject.GetComponentInChildren<Outline>().color = 1;
                EraseOutline(); // 아웃라인 출력 비활성화
                ClearMovementRange(); // 이동 범위 출력 비활성화
                MoveRangeOn = false;
                AttackRangeOn = false;
            }

            if (MouseOn) // 마우스 포인터가 올려져 있는 상태의 경우
            {
                showHpBar = true;

                if (MoveAvailable) // 이번 턴에 이동하지 않아 이동 가능할 경우
                {
                    ShowMovementRange(); // 이동 범위 출력
                    MoveRangeOn = true;
                }
            }
            else
            {
                if (ClickOn == false)
                {
                    showHpBar = false;
                    ClearMovementRange(); // 이동 범위 출력 비활성화
                    MoveRangeOn = false;
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

            switch (Mode)
            {
                case MODE.None:
                    AttackRangeOn = false;
                    transform.GetChild(Player.Available).gameObject.SetActive(true);
                    break;

                case MODE.Move:
                    {
                        AttackRangeOn = false;

                        if (MoveRangeOn) // 이동 범위가 표시되어 있는 경우
                        {
                            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
                            if (tm.blockMouseClick == false)
                            {
                                Vector2 a = new Vector2(x, y);
                                f.BFS(a, true);
                            }
                            ShowMouseOver(); // 이동 예상 좌표에 투명한 캐릭터UI 출력
                        }
                    }
                    break;

                case MODE.MoveEnd:
                    AttackRangeOn = false;
                    break;

                case MODE.Attack:
                    {
                        ClickManager.GetInst().attackOn = true;
                        ClearMovePath(); // 이동 경로 출력 비활성화
                        ClearMovementRange(); // 이동 범위 출력 비활성화
                        EndMouseOver();
                        MoveRangeOn = false;
                        Vector2 pos = new Vector2(x, y);
                        ShowWeaponRange(WeaponType, pos);
                        AttackRangeOn = true;
                    }
                    break;

                case MODE.Done:
                    transform.GetChild(Player.Available).gameObject.SetActive(false);
                    break;
            }
        }

        if (!unitDie && health <= 0)
        {
            switch (playerID)
            {
                case 0:
                    s.SoundPlay("EffectSound/mech_prime_punch_death");
                    break;
                case 1:
                    s.SoundPlay("EffectSound/mech_distance_artillery_death");
                    break;
                case 2:
                    s.SoundPlay("EffectSound/mech_brute_tank_death");
                    break;
                case 3:
                    s.SoundPlay("EffectSound/mech_prime_punch_death");
                    break;
                case 4:
                    s.SoundPlay("EffectSound/mech_prime_punch_death");
                    break;
                case 5:
                    s.SoundPlay("EffectSound/mech_brute_tank_death");
                    break;
            }

            ClickOn = false;
            ProfileInfo.GetInst().clickFlag = false;
            ProfileInfo.GetInst().holdInterface = false;
            UIControl.GetInst().proflieMiddlePick = false;
            ProfileInfo.GetInst().ClearPlayerInterFace();
            EraseOutline();
            ClearMovementRange();
            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
            tm.playerList.Remove(this.gameObject);
            unitDie = true;
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(Player.Available).gameObject.SetActive(false);
        }
    }

    // 이동시에 투명한 Player 이미지를 마우스오버 시킴
    public void ShowMouseOver()
    {
        if (gameObject.tag == "Character")
        {
            if (MouseOver == null)
            {
                MouseOver = f.CreateMovingMouseOver(gameObject);
            }
            else if (MouseOver != null)
            {
                if (MapControl.isMouseIn == true &&
                    MapControl.MoveState[MapControl.Crt_X, MapControl.Crt_Y] == true)
                {
                    MouseOver.SetActive(true);
                    MouseOver.transform.position = MapControl.Cell[MapControl.Crt_X, MapControl.Crt_Y];
                }
                else
                {
                    MouseOver.SetActive(false);
                }
            }
        }
    }

    void ShowHpBar()
    {
        if(!healthBarEnable)
        {
            healthBarEnable = true;

            GameObject available = transform.GetChild(1).gameObject;
            available.transform.position = new Vector2(available.transform.position.x, available.transform.position.y + 0.3f);

            switch (maxHealth)
            {
                case 1:
                    transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                    break;
                case 2:
                    transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                    break;
                case 3:
                    transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                    break;
                case 5:
                    transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
                    break;
            }
        }
    }

    void DisableHpBar()
    {
        if(healthBarEnable)
        {
            healthBarEnable = false;

            GameObject available = transform.GetChild(1).gameObject;
            available.transform.position = new Vector2(available.transform.position.x, available.transform.position.y - 0.3f);

            switch (maxHealth)
            {
                case 1:
                    transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                    break;
                case 2:
                    transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                    break;
                case 3:
                    transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
                    break;
                case 5:
                    transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void EndMouseOver()
    {
        if (MouseOver != null)
        {
            MouseOver.SetActive(false);
        }
    }

    public void ClearMovePath()
    {
        f.Clear();
    }

    public void AnimationCheak()
    {
        if (onWater)
        {
            animator.SetBool("onWater", true);
        }
        else
        {
            animator.SetBool("onWater", false);
        }

        if (unitDie)
        {
            animator.SetBool("isDie", true);
        }

        if (push)
        {
            transform.GetChild(0).GetChild(playerID).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(4).GetChild(0).gameObject.SetActive(true);

            if (transform.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sprite == null)
            {
                transform.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Player/Mech_" + (playerID + 1) + "/Mech_" + (playerID + 1));
            }

            if ((int)pushVector.x == 0 && (int)pushVector.y == -1) // 왼쪽
            {
                if(pushBack)
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

    void ShowWeaponRange(string _weaponType, Vector2 _pos)
    {
        switch (_weaponType)
        {
            case "Projectile":
                f.ProjectileAttackRange(10, _pos, true);
                break;
            case "Parabola":
                f.MortarAttackRange(10, _pos, true);
                break;
            case "Melee":
                f.CloseAttackRange(1, _pos, true);
                break;
        }

    }

    public void SetPlayerStat()
    {
        if (playerID == Player.Combat_Mech)
        {
            maxHealth = 3;
            health = 3;
            movement = 3;
            attackRange = 3;
        }
        if (playerID == Player.Cannon_Mech)
        {
            maxHealth = 2;
            health = 2;
            movement = 3;
            attackRange = 3;
        }
        if (playerID == Player.Artillery_Mech)
        {
            maxHealth = 2;
            health = 2;
            movement = 3;
            attackRange = 3;
        }
        if (playerID == 3) // 방패 메크
        {
            maxHealth = 5;
            health = 5;
            movement = 2;
            attackRange = 3;
        }
        if (playerID == 4) // D.VA 메크
        {
            maxHealth = 5;
            health = 5;
            movement = 2;
            attackRange = 3;
        }
        if (playerID == 5) // K-9 메크
        {
            maxHealth = 3;
            health = 3;
            movement = 2;
            attackRange = 3;
        }

    }
}