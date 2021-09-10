using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    public GameObject gameObject;

    void SetPushDisable()
    {
        gameObject.GetComponent<Unit>().push = false;
        gameObject.GetComponent<Unit>().pushBack = false;
        gameObject.transform.GetChild(0).GetChild(gameObject.GetComponent<Unit>().unitID).GetComponent<SpriteRenderer>().enabled = true;
    }
}
