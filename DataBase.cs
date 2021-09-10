using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 프로그램 실행 내내 유지되는 전역 클래스
public static class DataBase
{
    public static int[] playerMech = new int[3];  // 플레이어 캐릭터 3개의 값을 저장할 변수
    public static int playMapID = new int(); // 플레이할 맵의 ID 값을 저장
}
