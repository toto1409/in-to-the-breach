using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    public const int Building_0 = 1;
    public const int Building_1 = 2;

    public int typeID;
    public Component[] outline;
    private bool flag;
    public Sound s;

    private void Start()
    {
        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);

        typeID = this.transform.parent.parent.GetComponent<MapObject>().typeID;
        animator = this.GetComponent<Animator>();
        outline = GetComponentsInChildren<Outline>();
        enemySetMeTarget = 0;
        flag = true;
        s = GameObject.Find("GameSystem").GetComponent<Sound>();

        switch (typeID)
        {
            case Building.Building_0:
                health = 1;
                break;
            case Building.Building_1:
                health = 2;
                break;
        }
    }

    void Update()
    {
        AnimationCheak();

        // 공격 범위일 경우 오브젝트 아웃라인 출력
        if (MapControl.AttackState[x,y]) 
        {
            foreach (Outline t in outline) { t.eraseRenderer = false; }
        }
        else
        {
            foreach (Outline t in outline) { t.eraseRenderer = true; }
        }

        if(typeID == Building.Building_1)
        {
            if (health == 1 && flag)
            {
                flag = false;
                TurnBaseBattleManager.GetInst().enegy -= 1;

                GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
                GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
                tooltip.GetComponent<ToolTip>().mode = "BuildingDamege";
                tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
                tooltip.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f);
                tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                s.SoundPlay("EffectSound/prop_building_destroyed_01");
            }
        }

        if (health <= 0)
        {
            TurnBaseBattleManager tm = TurnBaseBattleManager.GetInst();
            tm.enegy -= 1;
            tm.objectList.Remove(this.gameObject);

            GameObject tooltipPrefab = Resources.Load("Prefabs/ToolTip") as GameObject;
            GameObject tooltip = MonoBehaviour.Instantiate(tooltipPrefab) as GameObject;
            tooltip.GetComponent<ToolTip>().mode = "BuildingDamege";
            tooltip.transform.parent = GameObject.Find("TitleCanvas").transform;
            tooltip.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f);
            tooltip.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            s.SoundPlay("EffectSound/prop_building_destroyed_03");

            MapControl.MapObjectArray[x, y] = null;
            GetComponent<Building>().enabled = false;
        }
    }

    public void AnimationCheak()
    {
        animator.SetInteger("Hp", health);
    }
}
