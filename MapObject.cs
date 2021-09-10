using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {

    // 맵 오브젝트 관련 상수값을 정의
    public const int IMAGE = 0;

    public const int Empty = -1;
    public const int GrassMountain = 0;
    public const int Building_0 = 1;
    public const int Building_1 = 2;
    public const int SandMountain = 3;
    public const int SnowMountain = 4;

    // 오브젝트의 종류
    public string type;
    public int typeID;

    // 오브젝트의 셀 인덱스 값 변수
    public int x;
    public int y;

    private void Start()
    {
        x = (int)((transform.position.x / (1.0f / 2) + (transform.position.y - 0.37f) / (0.74f / 2)) / -2);
        y = (int)(((transform.position.y - 0.37f) / (0.74f / 2) - (transform.position.x / (1.0f / 2))) / -2);
    }
}
