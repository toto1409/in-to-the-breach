using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Inst = null;

    //클릭할때 항상 받을 새로운 변수들
    public GameObject objMouseOver;
    public GameObject obj;
    public Outline outline;
    public Player player;
    public Enemy enemy;
    public int x, y;
    public bool attackOn;

    // 저장되서 다음 클릭전까지 쓰일 변수들
    GameObject objOld;
    GameObject objMouseOverOld;
    Outline outlineOld;
    Player playerOld;
    Enemy enemyOld;
    Functions f;
    public Sound s;


    public static ClickManager GetInst()
    {
        return Inst;
    }

    private void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        objMouseOver = null;
        obj = null;
        outline = null;
        player = null;
        enemy = null;

        objOld = null;
        objMouseOverOld = null;
        outlineOld = null;
        playerOld = null;
        enemyOld = null;

        f = GameObject.Find("GameSystem").GetComponent<Functions>();
        s = GameObject.Find("GameSystem").GetComponent<Sound>();
    }

    public void ClickCheck()
    {
        if (Input.GetMouseButtonDown(0))//왼클릭
        {
            ClearAllNew();

            if (playerOld != null)
            {
                playerOld.ClickOn = false;
            }

            if (enemyOld != null)
            {
                enemyOld.ClickOn = false;
            }

            if(!attackOn)
            {
                if (f.IsObjOn()) // 오브젝트가 있는가?
                {
                    s.SoundPlay("EffectSound/ui_map_window_open");

                    x = MapControl.Crt_X;
                    y = MapControl.Crt_Y;
                    obj = MapControl.MapObjectArray[x, y];
                    outline = obj.GetComponentInChildren<Outline>();

                    if (obj.tag == "Enemy")
                    {
                        enemy = obj.GetComponent<Enemy>();

                        if (objOld != null && objOld.tag == "Character")
                        {
                            if (objOld.GetComponent<Player>().Mode == Unit.MODE.Attack) { }
                            else
                            {
                                enemy.ClickOn = true;
                            }
                        }
                        else
                        {
                            enemy.ClickOn = true;
                        }
                    }

                    if (obj.tag == "Character")
                    {
                        player = obj.GetComponent<Player>();
                        player.ClickOn = true;

                        if (player.Mode == Player.MODE.None)
                        {
                            player.Mode = Player.MODE.Move;
                        }

                    }
                }

                ClearAllOld();
                SaveToOld();
            }
        }

        // 오른쪽 마우스 클릭
        if (Input.GetMouseButtonDown(1))
        {
            if (player != null)
            {
                s.SoundPlay("EffectSound/ui_map_window_close");

                switch (player.Mode)
                {
                    case Unit.MODE.Move:
                        {
                            player.EndMouseOver();
                            player.ClearMovePath();
                            player.ClickOn = false;
                            player.Mode = Unit.MODE.None;
                        }
                        break;
                    case Unit.MODE.Attack:
                        {
                            attackOn = false;
                            f.AttackRangeClear();

                            if (player.MoveAvailable)
                            {
                                player.Mode = Unit.MODE.None;
                            }
                            else
                            {
                                player.Mode = Unit.MODE.MoveEnd;
                            }
                        }
                        break;
                }
            }
            else if (enemy != null)
            {
                s.SoundPlay("EffectSound/ui_map_window_close");
            }

            ClearAllNew();
            ClearAllOld();
        }

        if (Input.GetKeyDown("1"))
        {
            if (player.Mode != Unit.MODE.Done && !player.onWater)
            {
                player.Mode = Unit.MODE.Attack;
                switch(player.WeaponType)
                {
                    case "Melee":
                        GameObject.Find("WeponButton").transform.GetChild(1).GetChild(player.playerID).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("wapown/Arms_Press");
                        break;
                    case "Parabola":
                        GameObject.Find("WeponButton").transform.GetChild(1).GetChild(player.playerID).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("wapown/OneCannon_Press");
                        break;
                    case "Projectile":
                        GameObject.Find("WeponButton").transform.GetChild(1).GetChild(player.playerID).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("wapown/TwoCannon_Press");
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            HealPlayer();
        }
    }

    public void HealPlayer()
    {
        if(player.Mode == Unit.MODE.Move || player.Mode == Unit.MODE.MoveEnd)
        {
            if(!player.onWater)
            {
                if (player.health < player.maxHealth)
                {
                    player.health += 1;
                }
                player.Mode = Unit.MODE.Done;
                player.ClickOn = false;
                player.MoveAvailable = false;
                player.EndMouseOver();
                player.ClearMovePath();
                GameObject.Find("ProfileInfoBottom").transform.GetChild(2).gameObject.SetActive(false);
                ProfileInfo.GetInst().ClearProfile();

                // 힐 이펙트 출력
                GameObject healEffectPrefab = Resources.Load("Prefabs/Effect") as GameObject;
                GameObject healEffect = MonoBehaviour.Instantiate(healEffectPrefab) as GameObject;
                healEffect.transform.position = MapControl.Cell[player.x, player.y];
                healEffect.transform.GetChild(1).gameObject.SetActive(true);
                s.SoundPlay("EffectSound/prop_pylon_power_04");
            }
        }
    }

    //새로운 클릭이 들어오면 전에 출력해서 쓰이던 old변수가 null이 아니라면 리셋
    public void ClearAllOld()
    {
        if (outlineOld != null)
        {

            outlineOld = null;
        }
        if (objMouseOverOld != null)
        {

            objMouseOverOld = null;
        }
        if (playerOld != null)
        {
            playerOld.ClickOn = false;
            playerOld = null;
        }
        if (enemyOld != null)
        {
            enemyOld.ClickOn = false;
            enemyOld = null;
        }
        if (objOld != null)
        {

            objOld = null;
        }
    }

    //새로운 클릭이 들어오면 전에 출력해서 쓰이던 old변수가 null이 아니라면 리셋
    public void ClearAllNew()
    {
        if (outline != null)
        {
            outline = null;
        }
        if (objMouseOver != null)
        {
            objMouseOver = null;
        }
        if (player != null)
        {
            player = null;
        }
        if (enemy != null)
        {
            enemy = null;
        }
        if (obj != null)
        {
            obj = null;
        }
    }

    //함수가 종료되기전 그동안 저장해서 표시했던 new들을 old에 저장.
    void SaveToOld()
    {
        if (outline != null)
        {
            outlineOld = outline;
        }
        if (objMouseOver != null)
        {
            objMouseOverOld = objMouseOver;
        }
        if (player != null)
        {
            playerOld = player;
        }
        if (enemy != null)
        {
            enemyOld = enemy;
        }
        if (obj != null)
        {
            objOld = obj;
        }

    }
}