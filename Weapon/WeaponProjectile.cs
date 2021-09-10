using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : Weapon
{
    void Start()
    {
        Dmg = 1;
        MinRange = 0;
        MaxRange = 7;
        f = GameObject.Find("GameSystem").GetComponent<Functions>();
    }

    public override void Attack(int x1, int y1, int x2, int y2)
    {
        Vector2 vec;
        
        base.Attack(x1, y1, x2, y2);
        vec = AttackAlgorithm(x1, y1, x2, y2);
        if(vec.x != -1 && vec.y != -1)
        {
            SpawnProjectile(x1, y1, (int)vec.x, (int)vec.y);
        }
    }
    
    //밑에 알고리즘처럼 그 방향으로 한칸씩 가면서 MapObjArray 에서 찾아서 뭐가 있을경우 맞은 그자리 Vector2를 리턴.
    //아무것도안맞았다면 쏜방향 끝을 가져옴
    //만약  맨밑의 return new Vector2( -1, -1) 했다면 오류. 
    public override Vector2 AttackAlgorithm(int x1, int y1, int x2, int y2)
    {
        base.AttackAlgorithm(x1, y1, x2, y2);
        //직선 공격 알고리즘
        //--------------------직선 투사체 공격 알고리즘-------------

        if (x2 == x1 || y2 == y1)//x인덱스나 y인덱스가 같다면 (직선4방향을 범위로 임의 지정)
        {
            if (!(x2 == x1 && y2 == y1))//같은 자리를 클릭한게 아니라면 공격실행
            {
                if (x2 == x1)//x축이 같다면 y축으로 공격한것
                {
                    if (y2 < y1) //y2의 어느방향인지 확인. 새로클릭한 y2가 old한 전꺼보다 작다면 작은쪽으로 공격
                    {
                        for (int i = y1 - 1; i >= 0; i--)
                        {
                            if (IsObjHit(x2, i))
                            {
                                
                                return new Vector2(x2, i);
                            }
                            if (i == 0)
                            {
                                //Debug.Log("맞지않음 왼쪽위방향 i: " + i + ", x2: " + x2);
                                return new Vector2(x2, i);
                            }
                        }
                    }
                    else if (y2 > y1)
                    {
                        for (int i = y1 + 1; i <= 7; i++)
                        {
                            if (IsObjHit(x2, i))
                            {

                                return new Vector2(x2, i);
                            }
                            if (i == 7)
                            {
                                //Debug.Log("맞지않음 오른쪽아래 방향");
                                return new Vector2(x2, i);
                            }
                        }
                    }

                }
                else if (y2 == y1)
                {
                    if (x2 < x1)
                    {
                        for (int i = x1 - 1; i >= 0; i--)
                        {
                            if (IsObjHit(i, y2))
                            {
                                return new Vector2(i, y2);
                            }
                            if (i == 0)
                            {
                                //Debug.Log("맞지않음 오른쪽위방향");
                                return new Vector2(i, y2);
                            }
                        }
                    }
                    else if (x2 > x1)
                    {
                        for (int i = x1 + 1; i <= 7; i++)
                        {
                            if (IsObjHit(i, y2))
                            {
                                return new Vector2(i, y2);
                            }
                            if (i == 7)
                            {
                                //Debug.Log("맞지않음 왼쪽아래 방향");
                                return new Vector2(i, y2);
                            }
                        }
                    }
                }

            }
        }
        //Debug.Log("에러 vector2 (-1 -1)");
        return new Vector2(-1, -1);
        //-----------------직선 투사체 공격 알고리즘 끝---------------------
    }
    public override void ShowAttackRange(int a, Vector2 tile, bool isFirst)
    {
        base.ShowAttackRange(a,tile, isFirst);
        

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
            ShowAttackRange(a - 1, upTile, false);
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
}
