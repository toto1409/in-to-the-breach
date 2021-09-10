using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveEffect : MonoBehaviour
{
    Transform transform;
    float value;

    private void Start()
    {
        transform = Camera.main.transform;
    }

    public void CameraShake(float _value)
    {
        Move1(_value);
    }

    void Move1(float _value)
    {
        value = _value;
        transform.position = new Vector3(0f, -2.2f + value, -100f);
        Invoke("Move2", 0.05f);
    }

    void Move2()
    {
        transform.position = new Vector3(0f, -2.2f - value, -100f);
        Invoke("Move3", 0.05f);
    }

    void Move3()
    {
        transform.position = new Vector3(0f, -2.2f + value, -100f);
        Invoke("Move4", 0.05f);
    }

    void Move4()
    {
        transform.position = new Vector3(0f, -2.2f, -100f);
    }
}
