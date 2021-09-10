using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{

    public float delayTime;

    void Start()
    {
        Destroy(transform.parent.gameObject, delayTime);
    }
}
