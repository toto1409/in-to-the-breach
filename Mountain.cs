using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : Unit
{
    public const int GrassMountain = 0;
    public Outline outline;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);
        
        animator = this.GetComponent<Animator>();
        outline = this.GetComponent<Outline>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        health = 2;
    }

    void Update()
    {
        AnimationCheak();

        // 공격 범위일 경우 오브젝트 아웃라인 출력
        if (MapControl.AttackState[x, y])
        {
            outline.eraseRenderer = false;
        }
        else
        {
            outline.eraseRenderer = true;
        }

        if (health <= 0)
        {
            MapControl.MapObjectArray[x, y] = null;
            spriteRenderer.sortingLayerName = "MapTile";
            GetComponent<Mountain>().enabled = false;
        }
    }

    public void AnimationCheak()
    {
        animator.SetInteger("Hp", health);
    }
}
