using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour {

    public Player player;
    private bool playerLoad;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!playerLoad && player)
        {
            playerLoad = true;

            animator.SetInteger("maxHealth", player.maxHealth);
        }

        if(playerLoad)
        {
            animator.SetInteger("health", player.health);
        }
    }
}
