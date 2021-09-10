using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour {

    public bool flag;
    public Image panel;

    private void Start()
    {
        panel.color = new Color(0f, 0f, 0f, 1f);
    }

    private void FixedUpdate()
    {
        if (!flag && panel.color.a > 0f)
        {
            panel.color = new Color(0f, 0f, 0f, panel.color.a - 0.1f);
        }
        else if (flag && panel.color.a < 1f)
        {
            panel.color = new Color(0f, 0f, 0f, panel.color.a + 0.1f);
        }
    }
}
