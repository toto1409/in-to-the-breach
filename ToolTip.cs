using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    public RectTransform rectTransform;  // UI 창의 RECT 컴포넌트를 받아올변수
    public Text text; // 내용을 저장할 변수
    public float alpha;
    public bool fadeIn;
    public bool fadeOut;
    public float fadeSpeed;
    public float delayTime;
    private int value;
    public string mode;

    private void Start()
    {
        fadeIn = true;
        alpha = 0f;

        switch(mode)
        {
            case "StartToolTip":
                value = Random.Range(0, 6);
                SetStartText(value);
                break;
            case "EndToolTip":
                value = Random.Range(0, 6);
                SetEndText(value);
                break;
            case "FireDamege":
                text.text = "화염 데미지!";
                break;
            case "BlockDamege":
                text.text = "출현 봉쇄!";
                break;
            case "BuildingDamege":
                value = Random.Range(0, 6);
                SetBuildingText(value);
                break;
        }
    }

    void Update()
    {
        // UI 창의 크기를 Text 내용에 맞춰서 최적화
        rectTransform.sizeDelta = new Vector2(text.preferredWidth + 5f, text.preferredHeight);

        // UI 최소 가로 크기 지정
        if (rectTransform.sizeDelta.x < 30f)
        {
            rectTransform.sizeDelta = new Vector2(30f, rectTransform.sizeDelta.y);
        }
        // UI 최소 세로 크기 지정
        if (rectTransform.sizeDelta.y < 13f)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 13f);
        }
    }

    private void FixedUpdate()
    {
        if(fadeIn)
        {
            if (alpha < 1f)
            {
                Image image = GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                alpha += fadeSpeed;
            }
            else
            {
                Invoke("FadeOut", delayTime);
            }
        }

        if(fadeOut)
        {
            if (alpha > 0f)
            {
                Image image = GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                alpha -= fadeSpeed;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void FadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }

    void SetStartText(int _value)
    {
        switch (_value)
        {
            case 0:
                text.text = "리프트 워커야!";
                break;
            case 1:
                text.text = "도와주세요!";
                break;
            case 2:
                text.text = "도와줘요! 리프트 워커!";
                break;
            case 3:
                text.text = "무슨 소리지?";
                break;
            case 4:
                text.text = "대원들이 도착했다!";
                break;
            case 5:
                text.text = "그들이 와줬어!";
                break;
        }
    }

    void SetEndText(int _value)
    {
        switch(_value)
        {
            case 0:
                text.text = "고마워요!";
                break;
            case 1:
                text.text = "우린 살았어!";
                break;
            case 2:
                text.text = "정말 최고야!";
                break;
            case 3:
                text.text = "정말 감사합니다!";
                break;
            case 4:
                text.text = "나쁜 벡놈들!";
                break;
            case 5:
                text.text = "이겼어! 승리야!";
                break;
        }
    }

    void SetBuildingText(int _value)
    {
        switch (_value)
        {
            case 0:
                text.text = "끄으악...";
                break;
            case 1:
                text.text = "리프트 워커...";
                break;
            case 2:
                text.text = "사람이 죽었어!...";
                break;
            case 3:
                text.text = "이제 끝이야..";
                break;
            case 4:
                text.text = "도..와..줘...";
                break;
            case 5:
                text.text = "살..려..줘...";
                break;
        }
    }
}
