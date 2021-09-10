using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoUI : MonoBehaviour {

    // UI 창의 RECT 컴포넌트를 받아올변수
    public RectTransform rectTransform;

    // 제목을 저장할 변수
    public Text title;

    // 내용을 저장할 변수
    public Text text;

    // 오브젝트의 이름값을 받아올 변수
    public string objectName;
    //지정된 타일의 이름을 가져올 변수
    public int[,] MapTileNameArray = new int[8, 8];

    Transform tempObj = null;
    public int count;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        title = transform.GetChild(0).GetComponent<Text>();
        text = transform.GetChild(1).GetComponent<Text>();
        count = transform.GetChild(2).GetChildCount();
    }

    void Update()
    {
        //print(objectName); // 디버그용
        // 마우스가 맵 안에 있을 경우
        if (MapControl.isMouseIn == true) 
        {
            // 타일에 오브젝트가 존재하고 그것이 장애물일 경우 장애물 툴팁 출력
            if (MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y] != null &&
                MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].tag == "Obstacle")
            {
                objectName = MapControl.MapObjectArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapObject>().type;
               
            }

            // 타일이 숲 타일일 경우 숲 타일 툴팁 출력
            else if (MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().Forest == true)
            {
                objectName = MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().type + "_Forest";
            }

            // 타일이 물 타일일 경우 물 타일 툴팁 출력
            else if (MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().Water == true) 
            {
                objectName = MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().type;
            }

            // 타일의 기본 Type 툴팁 출력
            else
            {
                objectName = MapControl.MapTileArray[MapControl.Crt_X, MapControl.Crt_Y].GetComponent<MapTile>().type;
            }
        }
        else
        {
            objectName = "";
        }

        switch(objectName)
        {
            case "Grass":
                title.text = "지상 타일";
                text.text = "특별한 효과가 없습니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                break;

            case "Grass_Forest":
                title.text = "삼림 타일";
                text.text = "피해를 받으면, 화재가 발생합니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
                break;

            case "Water":
                title.text = "수상 타일";
                text.text = "해상에 있는 유닛은 공격할 수 없습니다. 날지 못\n" +
                            "하는 적들 대부분은 물에 빠지면 죽습니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
                break;

            case "GrassMountain":
                title.text = "산악 타일";
                text.text = "이동 경로를 막고 발사체를 막아냅니다.\n" +
                            "공격을 2번 받으면 파괴됩니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
                break;

            case "Building_0":
                title.text = "민간인 건물";
                text.text = "망 시설이 피해를 입으면 전력망 수치가 감소합니다.\n";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(4).gameObject.SetActive(true);
                break;

            case "Building_1":
                title.text = "민간인 건물";
                text.text = "망 시설이 피해를 입으면 전력망 수치가 감소합니다.\n";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(5).gameObject.SetActive(true);
                break;

            case "Snow_Forest":
                title.text = "삼림 타일";
                text.text = "피해를 받으면, 화재가 발생합니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(6).gameObject.SetActive(true);
                break;

            case "Snow":
                title.text = "지상 타일";
                text.text = "특별한 효과가 없습니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(7).gameObject.SetActive(true);
                break;

            case "SnowMountain":
                title.text = "산악 타일";
                text.text = "이동 경로를 막고 발사체를 막아냅니다.\n" +
                            "공격을 2번 받으면 파괴됩니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(8).gameObject.SetActive(true);
                break;

            case "SandMountain":
                title.text = "산악 타일";
                text.text = "이동 경로를 막고 발사체를 막아냅니다.\n" +
                            "공격을 2번 받으면 파괴됩니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(9).gameObject.SetActive(true);
                break;

            case "Sand":
                title.text = "산악 타일";
                text.text = "이동 경로를 막고 발사체를 막아냅니다.\n" +
                            "공격을 2번 받으면 파괴됩니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(10).gameObject.SetActive(true);
                break;

            case "Sand_Forest":
                title.text = "삼림 타일";
                text.text = "피해를 받으면, 화재가 발생합니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(11).gameObject.SetActive(true);
                break;

            case "SandWater":
                title.text = "수상 타일";
                text.text = "해상에 있는 유닛은 공격할 수 없습니다. 날지 못\n" +
                            "하는 적들 대부분은 물에 빠지면 죽습니다.";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                this.transform.GetChild(2).GetChild(12).gameObject.SetActive(true);
                break;

            default: // 해당이 없을 경우 빈 화면 출력
                title.text = "";
                text.text = "";
                for (int i = 0; i < count; i++)
                {
                    this.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
                }
                break;
        }

        // UI 창의 크기를 Text 내용에 맞춰서 최적화
        rectTransform.sizeDelta = new Vector2(text.preferredWidth + 75f, text.preferredHeight + 37f);

        // UI 최소 가로 크기 지정
        if (rectTransform.sizeDelta.x < 170f)
        {
            rectTransform.sizeDelta = new Vector2(170f, rectTransform.sizeDelta.y);
        }
        // UI 최소 세로 크기 지정
        if (rectTransform.sizeDelta.y < 60f)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 60f);
        }

        // 제목의 가로 길이를 Text의 가로길이와 동기화
        title.GetComponentInParent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth + 15f, 13f);

        // Text의 가로길이에 여백을 둠
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(text.preferredWidth + 15f, text.preferredHeight);
    }

}


