using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // 유닛 관련 상수값을 정의
    public const int IMAGE = 0;

    // 유닛 MODE 변수
    public enum MODE { None, Move, MoveEnd, Attack, Done };
    public MODE Mode;

    // 유닛의 셀 인덱스 값 변수
    public int x;
    public int y;

    // 유닛의 기본 스텟 변수
    public int maxHealth;
    public int health;
    public int movement; // 이동력
    public bool ClickOn;  // ClickManager에서 클릭될때 이것만 켜주게된다. 나머지는 여기서 작동
    public bool MouseOn;
    public bool MoveRangeOn;
    public bool MoveAvailable;
    public int attackRange; // 공격 가능 거리(칸)
    public bool AttackAvailable;
    public bool AttackRangeOn;
    public bool unitDie;
    public int enemySetMeTarget;
    public bool onWater;
    public bool push;
    public bool pushBack;
    public Vector2 pushVector;
    public int unitID;

    public Functions f;
    public GameObject FacePicture;//케릭터 얼굴저장하는 변수
    public string WeaponType;
    public string WeaponName;
    public Weapon weapon;
    public Animator animator;
    public Animator animator2;
    public GameObject WeaponEffect;
    public bool WeaponEffectOn;
    public bool WeaponEffectCheck;
    public bool healthBarEnable;
    public bool showHpBar;
    public bool healthChange;
    public float uTime;
    public bool uTimeFlag;
    public int hpLog;


    // 유닛의 아웃라인을 출력함
    public void ShowOutline()
    {
        gameObject.GetComponentInChildren<Outline>().eraseRenderer = false;
    }

    // 유닛의 아웃라인 출력을 끔
    public void EraseOutline()
    {
        gameObject.GetComponentInChildren<Outline>().eraseRenderer = true;
    }

    // 유닛의 이동범위를 표시함
    public void ShowMovementRange()
    {
        f.MoveRange(movement, new Vector2(x, y), true);
    }

    // 유닛의 이동범위 출력을 끔
    public void ClearMovementRange()
    {
        if (MoveRangeOn)
        {
            f.MoveRangClear();
        }
    }
    public void ShowAttackRange()
    {
        weapon.ShowAttackRange(10, new Vector2(x, y), true);
    }
    public void ClearAttackRange()
    {
        if (AttackRangeOn)
        {
            f.AttackRangeClear();
        }
    }

    public void HealthChangeCheck()
    {
        if (!uTimeFlag)
        {
            uTimeFlag = true;
            uTime = Time.time + 0.1f;
            hpLog = health;
        }

        if (uTime <= Time.time)
        {
            uTimeFlag = false;
            if (hpLog != health)
            {
                healthChange = true;
                Invoke("healthChangeDelay", 2f);
            }
        }
    }

    public void healthChangeDelay()
    {
        healthChange = false;
    }

    public void ShowProfileBottom()
    {
        ShowFacePicture();
        //밑에 프로파일메뉴에 추가할것들
    }
    public void ClearProfileBottom()
    {
        ClearFacePicture();
        //밑에 프로파일메뉴에 추가할것들
    }
    void ShowFacePicture()
    {
        FacePicture.SetActive(true);
    }
    void ClearFacePicture()
    {
        FacePicture.SetActive(false);
    }
    //======================================================
    public void SetWeaponType(int id)
    {
        if (gameObject.tag == "Character")
        {
            switch (id)
            {

                case 0://인간형
                    WeaponType = "Melee";
                    break;
                case 1://4족
                    WeaponType = "Parabola";
                    break;
                case 2://탱크
                    WeaponType = "Projectile";
                    break;
                case 3://인간형
                    WeaponType = "Melee";
                    break;
                case 4://4족
                    WeaponType = "Projectile";
                    break;
                case 5://탱크
                    WeaponType = "Parabola";
                    break;
            }
        }
        if (gameObject.tag == "Enemy")
        {
            switch (id)
            {

                case 0://firefly
                    WeaponType = "Projectile";
                    break;
                case 1://spider
                    WeaponType = "Melee";
                    break;
                case 2://beetle
                    WeaponType = "Parabola";
                    break;
                case 3://Scarab
                    WeaponType = "Melee";
                    break;
            }
        }


    }
    public void SetWeaponName(int id)
    {
        if (gameObject.tag == "Character")
        {
            switch (id)
            {

                case 0: // 컴뱃 메크
                    WeaponName = "CharacterMelee";
                    break;
                case 1: // 자주포 메크
                    WeaponName = "CharacterParabola";
                    break;
                case 2: // 캐논 메크
                    WeaponName = "CharacterProjectile";
                    break;
                case 3: // 방패 메크
                    WeaponName = "CharacterMelee";
                    break;
                case 4: // D.VA 메크
                    WeaponName = "CharacterProjectile";
                    break;
                case 5: // K-9 메크
                    WeaponName = "CharacterParabola";
                    break;
            }
        }
        if (gameObject.tag == "Enemy")
        {
            switch (id)
            {

                case 0: // Firefly
                    WeaponName = "EnemyProjectile";
                    break;
                case 1: // Spider
                    WeaponName = "EnemyMelee";
                    break;
                case 2: // Beetle
                    WeaponName = "EnemyParabola";
                    break;
                case 3: // Scarab
                    WeaponName = "EnemyMelee";
                    break;
            }
        }


    }
    public void ShowWeaponEffectObj(int targetx, int targety, string weaponName)
    {
        WeaponEffect = weapon.SetWeaponEffect(targetx, targety, WeaponName);
        if (WeaponEffect == null)
            WeaponEffectOn = false;
        else
            WeaponEffectOn = true;
        return;
    }
    public void ClearWeaponEffectObj()
    {

        if (WeaponEffect != null)
        {
            Destroy(WeaponEffect);
            WeaponEffectOn = false;
        }
        return;
    }
    //======================================================
}
