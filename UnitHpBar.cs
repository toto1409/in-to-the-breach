using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpBar : MonoBehaviour {

    public Unit unit;
    private bool unitLoad;
    public Animator animator;

    private void Start()
    {
        unit = transform.parent.GetComponent<Unit>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!unitLoad && unit)
        {
            unitLoad = true;

            animator.SetInteger("maxHealth", unit.maxHealth);
        }

        if (unitLoad)
        {
            animator.SetInteger("health", unit.health);
        }
    }
}
