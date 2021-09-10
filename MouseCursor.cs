using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseCursor : MonoBehaviour
{
    public const int Default = 0;
    public const int Enemy = 6;

    void Update()
    {
        // 기본 마우스 커서 표시 OFF
        Cursor.visible = false;

        // 게임 플레이 씬이 아닐 경우 기본 마우스 포인터 출력
        if (SceneManager.GetActiveScene().name != "GamePlay")
        {
            // 커스텀 마우스 포인터의 좌표를 ScreenToWorldPoint 값으로 바꿔서 적용시킴.
            this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                  Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);

            this.transform.GetChild(MouseCursor.Default).gameObject.SetActive(true);
        }
        else
        {
            // 커스텀 마우스 포인터의 좌표를 ScreenToWorldPoint 값으로 바꿔서 적용시킴.
            this.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                  Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -10f);

            // 커스텀 마우스 커서가 변경되는 조건문들
            if (MapControl.isMouseIn == true)
            {
                if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null &&
                MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Enemy")
                {
                    this.transform.GetChild(MouseCursor.Default).gameObject.SetActive(false);
                    this.transform.GetChild(MouseCursor.Enemy).gameObject.SetActive(true);
                }
                else
                {
                    for (int i = 1; i < 8; i++)
                    {
                        this.transform.GetChild(i).gameObject.SetActive(false);
                    }

                    this.transform.GetChild(MouseCursor.Default).gameObject.SetActive(true);
                }
            }
            else
            {
                for (int i = 1; i < 8; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }

                this.transform.GetChild(MouseCursor.Default).gameObject.SetActive(true);
            }
        }
    }
}
