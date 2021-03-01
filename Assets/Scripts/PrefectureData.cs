using System.Collections.Generic;
using UnityEngine;

public class PrefectureData
{
    public readonly int id;
    public readonly string name;
    public readonly float longitude;
    public readonly float latitude;

    public GameObject ownObj;

    public PrefectureData(List<string> csvLine)
    {
        id = int.Parse(csvLine[0]);
        name = csvLine[1];
        latitude = float.Parse(csvLine[2]);
        longitude = float.Parse(csvLine[3]);
    }
}
