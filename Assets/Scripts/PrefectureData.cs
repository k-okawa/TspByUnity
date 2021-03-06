using System.Collections.Generic;
using UnityEngine;

public class PrefectureData
{
    public readonly int id;
    // 県庁所在地名
    public readonly string name;
    // 経度
    public readonly double longitude;
    // 緯度
    public readonly double latitude;

    // Unity上のオブジェクト
    public GameObject ownObj;

    public PrefectureData(List<string> csvLine)
    {
        id = int.Parse(csvLine[0]);
        name = csvLine[1];
        latitude = double.Parse(csvLine[2]);
        longitude = double.Parse(csvLine[3]);
    }
}
