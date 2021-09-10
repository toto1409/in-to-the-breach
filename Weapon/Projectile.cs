using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int x, y;
    public int x1, y1;
    public int x2, y2;//쏜곳
    public bool timeSet;
    public float time;
    public Vector2 PushDirection;
    public Animator a;
    public bool ready;
    public float speed;
    public MapControl mc;
    public Functions f;
    public bool hit;
    public Vector2[] trajectory = new Vector2[3];
    public int TrajectoryCount;
    public int MaxCount;
    public string WeaponType;
    public string WeaponName;
    public GameObject ObjMe;
    public float delay;
    public Sound s;


    private void Awake()
    {
        TrajectoryCount = 0;
        trajectory = new Vector2[3];
        for (int i = 0; i < 3; i++)
        {
            trajectory[i].x = -1;
            trajectory[i].y = -1;
        }
        timeSet = true;
        hit = false;
        ready = false;
        speed = 6.0f;
        mc = GameObject.Find("GameSystem").GetComponent<MapControl>();
        f = GameObject.Find("GameSystem").GetComponent<Functions>();
    }

    void Start()
    {
        x1 = (int)((transform.position.x / (1.0f / 2) + (transform.position.y) / (0.74f / 2)) / -2);
        y1 = (int)(((transform.position.y) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);
        ObjMe = GetObject(x1, y1);
        TurnOnWeaponImage(ObjMe);
        a = gameObject.GetComponentInChildren<Animator>();
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
        SetTrajectory(trajectory, WeaponType);

        ready = true;

        // 발사될 때 사운드
        if(ObjMe.tag == "Character")
        {
            switch(ObjMe.GetComponent<Unit>().WeaponType)
            {
                case "Projectile":
                    s.SoundPlay("EffectSound/mech_brute_skill_modified_cannons");
                    break;
                case "Parabola":
                    s.SoundPlay("EffectSound/mech_distance_skill_defense_strike");
                    break;
                case "Melee":
                    s.SoundPlay("EffectSound/mech_prime_skill_titanfist_null");
                    break;
            }
        }
        else if(ObjMe.tag == "Enemy")
        {
            switch (ObjMe.GetComponent<Unit>().WeaponType)
            {
                case "Projectile":
                    s.SoundPlay("EffectSound/enemy_firefly_sold_1_attack_spit");
                    break;
                case "Parabola":
                    s.SoundPlay("EffectSound/enemy_blobber_1_attack_launch");
                    break;
                case "Melee":
                    s.SoundPlay("EffectSound/mech_prime_skill_titanfist_null");
                    break;
            }
        }
    }

    void Update()
    {
        start();
    }

    void start()
    {
        if (ready)
        {
            x1 = (int)((transform.position.x / (1.0f / 2) + (transform.position.y) / (0.74f / 2)) / -2);
            y1 = (int)(((transform.position.y) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);
            float step = speed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, trajectory[TrajectoryCount], step);

            if (transform.position.x == trajectory[TrajectoryCount].x && transform.position.y == trajectory[TrajectoryCount].y)
            {
                if (TrajectoryCount + 1 != MaxCount)
                {
                    TrajectoryCount++;
                }

                if (TrajectoryCount == 2)
                {
                    gameObject.transform.eulerAngles = Vector3.forward * -180.0f;
                }
            }

            if (x1 == x2 && y1 == y2)
            {
                SetExplosionRotation();
                a.SetBool("Hit", true);
                if (hit == false)
                {
                    hit = true;
                    f.Hit(x, y, x2, y2);
                    //밀리는 시각적함수
                    //딜레이
                    PushObj(x2, y2);
                }
                Explosion();
            }
        }
    }

    void SetExplosionRotation()
    {
        if (WeaponName != "CharacterMelee" && WeaponName != "EnemyMelee")
        {
            gameObject.transform.eulerAngles = Vector3.forward * 0f;
        }
    }

    void Explosion()
    {

        if (timeSet)
        {
            time = Time.time;
            timeSet = false;
        }

        if (time + delay <= Time.time)
        {
            Destroy(this.gameObject);
        }
    }

    void GetWeaponName(GameObject obj)
    {

        int id;
        string direction = GetDirection(x1, y1, x2, y2);
        if (obj.tag == "Character")
        {
            Player player = obj.GetComponent<Player>();
            id = obj.GetComponent<Player>().playerID;
            WeaponType = player.WeaponType;
            WeaponName = player.WeaponName;
            switch (id)
            {
                case 0: // 컴뱃 메크
                    MaxCount = 1;
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 0.4f;
                    break;
                case 1: // 자주포 메크
                    MaxCount = 3;
                    delay = 1.0f;
                    break;
                case 2: // 캐논 메크
                    MaxCount = 1;
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 1.0f;
                    break;
                case 3: // 방패 메크
                    MaxCount = 1;
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 0.4f;
                    break;
                case 4: // D.VA 메크
                    MaxCount = 1;
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 1.0f;
                    break;
                case 5: // K-9 메크
                    MaxCount = 3;
                    delay = 1.0f;
                    break;
            }

        }
        else if (obj.tag == "Enemy")
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            id = enemy.enemyID;
            WeaponType = enemy.WeaponType;
            WeaponName = enemy.WeaponName;
            switch (id)
            {
                case 0://firefly
                    MaxCount = 1;
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 1.0f;
                    break;
                case 1://spider
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 1.0f;
                    MaxCount = 1;
                    break;
                case 2://beetle
                    MaxCount = 3;
                    delay = 1.0f;
                    break;
                case 3://굴착벌레
                    SetDirection(x1, y1, x2, y2, direction);
                    delay = 1.0f;
                    MaxCount = 1;
                    break;
            }

        }
    }

    void TurnOnWeaponImage(GameObject obj) //그 오브젝트의 맞는 투사체의 이미지를 켜주는 함수들
    {
        GetWeaponName(obj);
        gameObject.transform.Find(WeaponName).gameObject.SetActive(true);
    }

    void SetDirection(int x1, int y1, int x2, int y2, string direction)
    {

        if (WeaponType == "Melee" || WeaponType == "Projectile")
        {
            switch (direction)
            {
                case "왼쪽위":
                    gameObject.transform.eulerAngles = Vector3.forward * 50.0f;
                    PushDirection = new Vector2(0, -1);
                    break;
                case "왼쪽아래":
                    gameObject.transform.eulerAngles = Vector3.forward * 134.0f;
                    PushDirection = new Vector2(1, 0);
                    break;
                case "오른쪽위":
                    gameObject.transform.eulerAngles = Vector3.forward * -48.0f;
                    PushDirection = new Vector2(-1, 0);
                    break;
                case "오른쪽아래":
                    gameObject.transform.eulerAngles = Vector3.forward * -132.0f;
                    PushDirection = new Vector2(0, 1);
                    break;
            }
        }
    }

    string GetDirection(int x1, int y1, int x2, int y2)
    {
        if (x2 == x1 || y2 == y1)//x인덱스나 y인덱스가 같다면 (직선4방향을 범위로 임의 지정)
        {
            if (!(x2 == x1 && y2 == y1))//같은 자리를 클릭한게 아니라면 공격실행
            {
                if (x2 == x1)//x축이 같다면 y축으로 공격한것
                {
                    if (y2 < y1) //y2의 어느방향인지 확인. 새로클릭한 y2가 old한 전꺼보다 작다면 작은쪽으로 공격
                    {

                        //Debug.Log("맞지않음 왼쪽위방향 i: " + i + ", x2: " + x2);
                        //gameObject.transform.eulerAngles = Vector3.forward * 50.0f;
                        return "왼쪽위";

                    }
                    else if (y2 > y1)
                    {
                        //  Debug.Log("맞지않음 오른쪽아래 방향");
                        //gameObject.transform.eulerAngles = Vector3.forward * -132.0f;
                        return "오른쪽아래";
                    }

                }
                else if (y2 == y1)
                {
                    if (x2 < x1)
                    {

                        // Debug.Log("맞지않음 오른쪽위방향");
                        //gameObject.transform.eulerAngles = Vector3.forward * -48.0f;

                        return "오른쪽위";

                    }
                    else if (x2 > x1)
                    {

                        //Debug.Log("맞지않음 왼쪽아래 방향");
                        //gameObject.transform.eulerAngles = Vector3.forward * 134.0f;

                        return "왼쪽아래";

                    }
                }

            }
        }
        return "오류";
    }

    void SetTrajectory(Vector2[] v, string WeaponType)
    {
        switch (WeaponType)
        {
            case "Projectile":
                v[0].x = mc.Cell_Center_Pos[x2, y2].x;
                v[0].y = mc.Cell_Center_Pos[x2, y2].y;
                break;
            case "Parabola":

                v[0].x = mc.Cell_Center_Pos[x1, y1].x;
                v[0].y = mc.Cell_Center_Pos[x1, y1].y + 10;
                v[1].x = mc.Cell_Center_Pos[x2, y2].x;
                v[1].y = mc.Cell_Center_Pos[x2, y2].y + 10;
                v[2].x = mc.Cell_Center_Pos[x2, y2].x;
                v[2].y = mc.Cell_Center_Pos[x2, y2].y;
                speed = 18.0f;
                break;
            case "Melee":
                v[0].x = mc.Cell_Center_Pos[x2, y2].x;
                v[0].y = mc.Cell_Center_Pos[x2, y2].y;
                break;
        }

    }
    GameObject GetObject(int x, int y)
    {
        GameObject obj;
        obj = MapControl.MapObjectArray[x, y];
        return obj;
    }
    void PushObj(int x2, int y2)
    {
        if (ObjMe.tag == "Character")
        {

            if (WeaponType == "Projectile" || WeaponType == "Melee")
            {

                CalculatePush(x2, y2, PushDirection);

            }
            else if (WeaponType == "Parabola")
            {

                CalculatePush(x2 + 1, y2, new Vector2(1, 0));
                CalculatePush(x2 - 1, y2, new Vector2(-1, 0));
                CalculatePush(x2, y2 + 1, new Vector2(0, 1));
                CalculatePush(x2, y2 - 1, new Vector2(0, -1));
            }
        }


    }
    void CalculatePush(int x, int y, Vector2 pushdirection)
    {
        if (CheckPushArray(x2, y2, (int)pushdirection.x, (int)pushdirection.y) == true)
        {
            if (CheckBeforePush(x, y, pushdirection) == false)
            {
                DmgForBothPush(x, y, pushdirection);

            }
            else
            {
                if (CheckPushArray(x, y, (int)pushdirection.x, (int)pushdirection.y) == true)
                {
                    if (MapControl.MapObjectArray[x, y] != null)
                    {
                        if (MapControl.MapObjectArray[x, y].tag != "Obstacle")
                        {
                            MapControl.MapObjectArray[x, y].GetComponent<Unit>().push = true;
                            MapControl.MapObjectArray[x, y].GetComponent<Unit>().pushVector = pushdirection;

                            MapControl.MapObjectArray[x, y].transform.position =
                            new Vector2(MapControl.Cell[x + (int)pushdirection.x, y + (int)pushdirection.y].x,
                            MapControl.Cell[x + (int)pushdirection.x, y + (int)pushdirection.y].y);

                            MapControl.MapObjectArray[x + (int)pushdirection.x,
                                 y + (int)pushdirection.y] = MapControl.MapObjectArray[x, y];
                            MapControl.MapObjectArray[x, y] = null;
                        }

                    }

                }

            }
        }

    }
    void DmgForBothPush(int x, int y, Vector2 pushdirection)
    {
        MapControl.MapObjectArray[x, y].GetComponent<Unit>().health -= 1;
        Debug.Log(MapControl.MapObjectArray[x, y].name + "가 푸쉬데미지입음");
        if (MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y].tag == "Character"
            || MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y].tag == "Enemy")
        {
            MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y].GetComponent<Unit>().health -= 1;

            MapControl.MapObjectArray[x, y].GetComponent<Unit>().push = true;
            MapControl.MapObjectArray[x, y].GetComponent<Unit>().pushBack = true;
            MapControl.MapObjectArray[x, y].GetComponent<Unit>().pushVector = pushdirection;
        }
        else
        {
            MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y].transform.GetComponentInChildren<Unit>().health -= 1;

            MapControl.MapObjectArray[x, y].GetComponent<Unit>().push = true;
            MapControl.MapObjectArray[x, y].GetComponent<Unit>().pushBack = true;
            MapControl.MapObjectArray[x, y].GetComponent<Unit>().pushVector = pushdirection;
        }
        Debug.Log(MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y].name + "가 푸쉬데미지입음");
    }
    bool CheckPushArray(int x, int y, int Xoffset, int Yoffset)//x,y에위치한 오브젝트를 밀어도되나 체크
    {
        if (x + Xoffset >= 8 || x + Xoffset <= -1)
        {
            return false;
        }
        if (y + Yoffset >= 8 || y + Yoffset <= -1)
        {
            return false;
        }
        return true;
    }
    bool CheckBeforePush(int x, int y, Vector2 pushdirection)
    {
        if (CheckPushArray(x, y, (int)pushdirection.x, (int)pushdirection.y) == true)
        {
            if (MapControl.MapObjectArray[x, y] != null)
            {
                if (MapControl.MapObjectArray[x, y].tag == "Character" || MapControl.MapObjectArray[x, y].tag == "Enemy")
                {
                    if (MapControl.MapObjectArray[x + (int)pushdirection.x, y + (int)pushdirection.y] != null)
                    {
                        return false;
                    }
                }

            }
        }

        return true;
    }
}
