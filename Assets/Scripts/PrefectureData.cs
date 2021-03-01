using System.Collections.Generic;
using UnityEngine;

public class PrefectureData
{
    public readonly int id;
    public readonly string name;
    public readonly double longitude;
    public readonly double latitude;

    public GameObject ownObj;

    public PrefectureData(List<string> csvLine)
    {
        id = int.Parse(csvLine[0]);
        name = csvLine[1];
        latitude = double.Parse(csvLine[2]);
        longitude = double.Parse(csvLine[3]);
    }
}
