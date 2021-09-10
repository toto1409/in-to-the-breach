using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public int Dmg;
    public int MinRange;
    public int MaxRange;
    public string direction;
    public Functions f;
    
    public Weapon SearchWeaponType(string WeaponType)
    {
        Weapon temp = GetComponent<Weapon>();
        switch (WeaponType)
        {
            case "Projectile":
                temp = GetComponent<WeaponProjectile>();
                break;
            case "Parabola":
                temp = GetComponent<WeaponParabola>();
                break;
            case "Melee":
                temp = GetComponent<WeaponPunch>();
                break;
                //다른케이스들 추가
        }
        return temp;
    }

    public virtual void Attack(int x1, int y1, int x2, int y2) { }
    public virtual bool IsObjHit(int x, int y)
    {
        if (MapControl.MapObjectArray[x, y] != null)
            return true;
        else
            return false;
    }
    public virtual Vector2 AttackAlgorithm(int x1, int y1, int x2, int y2) { return new Vector2(-2, -2); }
    public virtual void ShowAttackRange(int a, Vector2 tile, bool isFirst) { return; }
    public GameObject SpawnProjectile(int x1, int y1, int x2, int y2)
    {
        GameObject pp = Resources.Load("Prefabs/Projectile") as GameObject;
        GameObject projectile = MonoBehaviour.Instantiate(pp) as GameObject;
        MapControl mc = GameObject.Find("GameSystem").GetComponent<MapControl>();
        projectile.transform.position = mc.Cell_Center_Pos[x1, y1];
        Projectile tempp = projectile.GetComponentInChildren<Projectile>();
        tempp.x1 = tempp.x = x1;
        tempp.x2 = x2;
        tempp.y1 = tempp.y = y1;
        tempp.y2 = y2;
        
        //tempp.direction.x = x1 - x2;
        //tempp.direction.y = y1 - y2;
        //tempp.ready = true;
        return projectile;
    }
    public virtual GameObject SetWeaponEffect(int x, int y, string weaponName)
    {
        return null;
    }
}
