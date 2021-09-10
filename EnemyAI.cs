using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public TurnBaseBattleManager tbm;
    public Enemy enemy;
    Functions f;
    public GameObject target;
    public int targetDistance;
    public int mvPointX, mvPointY;
    public bool AttackLockOn;
    public int time;
    public bool targetNumCheck;
    public int targetX, targetY;
    public int DistanceX, DistanceY;
    public bool printAttackDirection;
    public Sound s;

    private void Start()
    {
        tbm = TurnBaseBattleManager.GetInst();
        enemy = this.GetComponent<Enemy>();
        f = Functions.GetInst();
        targetDistance = 999;
        AttackLockOn = false;
        targetNumCheck = true;
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
    }

    void Update()
    {

        // 죽었을 경우 처리
        if (enemy.health <= 0)
        {
            AttackDirectionDisable();
        }

        if (AttackLockOn)
        {
            if (enemy.WeaponType == "Projectile")
            {
                // 직사포의 경우 타겟이 위치에 있는지 매번 체크
                ProjectileTargetCheck();
            }
            else
            {
                MapControl.EnemyTargeted[targetX, targetY] = false;
                targetX = enemy.x + DistanceX;
                targetY = enemy.y + DistanceY;
            }

            // 자신의 공격 타겟 지점을 계산하여 표시
            targetX = enemy.x + DistanceX;
            targetY = enemy.y + DistanceY;
            if(enemy.deathFlag)
            {
                MapControl.EnemyTargeted[targetX, targetY] = false;

                for (int i = 0; i < 4; i++)
                {
                    enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                MapControl.EnemyTargeted[targetX, targetY] = true;

                // 공격 방향 표시
                if (!printAttackDirection)
                {
                    s.SoundPlay("EffectSound/ui_map_sell", 1f);
                    if (targetX < enemy.x) // 상
                    {
                        enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(0).gameObject.SetActive(true);
                    }
                    else if (targetX > enemy.x) // 하
                    {
                        enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(1).gameObject.SetActive(true);
                    }
                    else if (targetY < enemy.y) // 좌
                    {
                        enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(2).gameObject.SetActive(true);
                    }
                    else if (targetY > enemy.y) // 우
                    {
                        enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(3).gameObject.SetActive(true);
                    }

                    printAttackDirection = true;
                }
            }

            if (enemy.health <= 0)
            {
                MapControl.EnemyTargeted[targetX, targetY] = false;
                if (target != null)
                {
                    target.GetComponent<Unit>().enemySetMeTarget -= 1;
                    target = null;
                }
            }
        }

        if (tbm.currentState == TurnBaseBattleManager.BattleStates.ENEMYTURN)
        {
            switch (enemy.Mode)
            {
                case Unit.MODE.None:
                    break;

                case Unit.MODE.Attack:
                    {
                        if (AttackLockOn)
                        {
                            enemy.weapon.Attack(enemy.x, enemy.y, targetX, targetY);
                            MapControl.EnemyTargeted[targetX, targetY] = false;
                            AttackDirectionDisable();
                            AttackLockOn = false;
                        }
                    }
                    break;

                case Unit.MODE.Move:
                    {
                        if (enemy.MoveAvailable)
                        {
                            enemy.MoveAvailable = false;
                            enemy.activation = true;
                            targetNumCheck = true;

                            if (target != null)
                            {
                                target.GetComponent<Unit>().enemySetMeTarget -= 1;
                            }

                            FindTarget();
                            MoveEnemy();
                        }
                    }
                    break;

                case Unit.MODE.MoveEnd:
                    {
                        targetDistance = 999;

                        if (targetNumCheck)
                        {
                            targetNumCheck = false;

                            if (AttackableCheak())
                            {
                                AttackLockOn = true;
                                AttackTargetSet();
                                printAttackDirection = false;
                            }
                            else
                            {
                                // 공격이 불가능 할 경우 타겟을 취소
                                target = null;
                            }
                        }
                        enemy.activation = false;
                        enemy.Mode = Unit.MODE.Done;
                    }
                    break;

                case Unit.MODE.Done:
                    break;
            }
        }
    }

    // 공격 여부 체크 함수
    bool AttackableCheak()
    {
        for (int i = enemy.attackRange; i > 0; i--)
        {
            Unit tempTarget = target.GetComponent<Unit>();

            if (enemy.y == tempTarget.y && enemy.x - i == tempTarget.x) // 상
            {
                return true;
            }

            if (enemy.y == tempTarget.y && enemy.x + i == tempTarget.x) // 하
            {
                return true;
            }

            if (enemy.y - i == tempTarget.y && enemy.x == tempTarget.x) // 좌
            {
                return true;
            }

            if (enemy.y + i == tempTarget.y && enemy.x == tempTarget.x) // 좌
            {
                return true;
            }
        }
        return false;
    }

    // 타겟 탐지 함수
    void FindTarget()
    {
        Vector2 enemyPos = new Vector2(enemy.x, enemy.y); // 몬스터의 이동 전 위치
        f.MoveRangClear();
        f.MoveRange(enemy.movement, enemyPos, true); // 이동 범위 출력

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (MapControl.MoveState[i, j] == true || (i == enemy.x && j == enemy.y)) 
                {
                    foreach (GameObject player in tbm.playerList) // 가장 가까운 플레이어 탐색
                    {
                        int tempX = Mathf.Abs(player.GetComponent<Player>().x - i);
                        int tempY = Mathf.Abs(player.GetComponent<Player>().y - j);

                        int tempDistance = tempX + tempY; // 이동 거리 값

                        bool flag = false;

                        if (tempX == enemy.attackRange && tempY == 0) // 최적의 공격 범위일 경우
                        {
                            flag = true;
                        }
                        else if (tempY == enemy.attackRange && tempX == 0)
                        {
                            flag = true;
                        }

                        if (flag) // 최적의 공격 범위일 경우
                        {
                            targetDistance = tempDistance;
                            target = player;
                            mvPointX = i;
                            mvPointY = j;
                        }
                        else
                        {
                            if (tempDistance < targetDistance)
                            {
                                if (targetDistance > enemy.attackRange)
                                {
                                    targetDistance = tempDistance;
                                    target = player;
                                    mvPointX = i;
                                    mvPointY = j;
                                }
                            }
                        }

                    }

                    foreach (GameObject _object in tbm.objectList) // 가장 가까운 오브젝트 탐색
                    {
                        int tempX = Mathf.Abs(_object.transform.parent.parent.GetComponent<MapObject>().x - i);
                        int tempY = Mathf.Abs(_object.transform.parent.parent.GetComponent<MapObject>().y - j);

                        int tempDistance = tempX + tempY;

                        bool flag = false;

                        if (tempX == enemy.attackRange && tempY == 0)
                        {
                            flag = true;
                        }
                        else if (tempY == enemy.attackRange && tempX == 0)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            targetDistance = tempDistance;
                            target = _object;
                            mvPointX = i;
                            mvPointY = j;
                        }
                        else
                        {
                            if (tempDistance < targetDistance)
                            {
                                if (targetDistance > enemy.attackRange)
                                {
                                    targetDistance = tempDistance;
                                    target = _object;
                                    mvPointX = i;
                                    mvPointY = j;
                                }
                            }
                        }

                    }
                }
            }
        }

        //print(enemy.name + " 대상 : " + target);
        //print(enemy.name + " 이동지점 : " + mvPointX + "_" + mvPointY);
        //print(enemy.name + " 이동지점과 대상의 거리 : " + targetDistance);
    }

    void MoveEnemy()
    {
        int targetX, targetY;
        targetX = targetY = -1;

        if (target.tag == "Character")
        {
            targetX = target.GetComponent<Player>().x;
            targetY = target.GetComponent<Player>().y;
        }
        else if (target.tag == "Building")
        {
            targetX = target.GetComponent<Building>().x;
            targetY = target.GetComponent<Building>().y;
        }

        bool attakRangeFlag = false;

        for (int i = enemy.attackRange; i > 0; i--)
        {
            if (targetX < enemy.x && enemy.y == targetY) // 타겟이 위에 있다
            {
                if (enemy.x == targetX + i)
                {
                    attakRangeFlag = true;
                    break;
                }
            }
            if (targetX > enemy.x && enemy.y == targetY) // 타겟이 아래에 있다
            {
                if (enemy.x == targetX - i)
                {
                    attakRangeFlag = true;
                    break;
                }
            }
            if (targetY < enemy.y && enemy.x == targetX) // 타겟이 왼쪽에 있다
            {
                if (enemy.y == targetY + i)
                {
                    attakRangeFlag = true;
                    break;
                }
            }
            if (targetY > enemy.y && enemy.x == targetX) // 타겟이 오른쪽에 있다
            {
                if (enemy.y == targetY - i)
                {
                    attakRangeFlag = true;
                    break;
                }
            }
        }

        if (targetDistance == enemy.attackRange && (mvPointX != enemy.x || mvPointY != enemy.y))
        {
            attakRangeFlag = false;
        }

        if (attakRangeFlag || mvPointX == enemy.x && mvPointY == enemy.y)
        {
            //print(enemy + " 는 공격 범위(" + targetX + "_" + targetY + ")이기 때문에 이동하지 않음!");
            enemy.Mode = Unit.MODE.MoveEnd;

            return;
        }
        else
        {
            Vector2 enemyPos = new Vector2(enemy.x, enemy.y); // 몬스터의 이동 전 위치
            f.EnemyBFS(enemyPos, true, mvPointX, mvPointY); // 최단거리 계산
            f.MoveEnemy(this.gameObject, mvPointX, mvPointY); // 해당 좌표로 이동
        }
    }

    void AttackTargetSet()
    {
        target.GetComponent<Unit>().enemySetMeTarget += 1;

        if (target.tag == "Character")
        {
            Player player = target.GetComponent<Player>();

            if (enemy.x == player.x)
            { DistanceX = 0; }
            else
            { DistanceX = player.x - enemy.x; }

            if (enemy.y == player.y)
            { DistanceY = 0; }
            else
            { DistanceY = player.y - enemy.y; }
        }
        else if (target.tag == "Building")
        {
            MapObject mapObject = target.GetComponentInParent<MapObject>();

            if (enemy.x == mapObject.x)
            { DistanceX = 0; }
            else
            { DistanceX = mapObject.x - enemy.x; }

            if (enemy.y == mapObject.y)
            { DistanceY = 0; }
            else
            { DistanceY = mapObject.y - enemy.y; }
        }
    }

    void AttackDirectionDisable()
    {
        for (int i = 0; i < 4; i++)
        {
            enemy.transform.GetChild(Enemy.ATTACKDIRECTION).GetChild(i).gameObject.SetActive(false);
        }
    }

    void ProjectileTargetCheck()
    {
        MapControl.EnemyTargeted[targetX, targetY] = false;
        targetX = enemy.x + DistanceX;
        targetY = enemy.y + DistanceY;

        if (DistanceX == 0)
        {
            if (DistanceY < 0) // 좌
            {
                for (int i = enemy.y - 1; i >= 0; i--)
                {
                    if (MapControl.MapObjectArray[enemy.x, i] != null)
                    {
                        DistanceY = i - enemy.y;
                        break;
                    }
                    DistanceY = -enemy.y;
                }
            }
            else if (DistanceY > 0) // 우
            {
                for (int i = enemy.y + 1; i <= 7; i++)
                {
                    if (MapControl.MapObjectArray[enemy.x, i] != null)
                    {
                        DistanceY = i - enemy.y;
                        break;
                    }
                    DistanceY = 7 - enemy.y;
                }
            }
        }
        else if (DistanceY == 0)
        {
            if (DistanceX < 0) // 상
            {
                for (int i = enemy.x - 1; i >= 0; i--)
                {
                    if (MapControl.MapObjectArray[i, enemy.y] != null)
                    {
                        DistanceX = i - enemy.x;
                        break;
                    }
                    DistanceX = -enemy.x;
                }
            }
            else if (DistanceX > 0) // 하
            {
                for (int i = enemy.x + 1; i <= 7; i++)
                {
                    if (MapControl.MapObjectArray[i, enemy.y] != null)
                    {
                        DistanceX = i - enemy.x;
                        break;
                    }
                    DistanceX = 7 - enemy.x;
                }
            }
        }
    }
}
