using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrefectureMap : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    
    private void Start()
    {
        var prefectureList = CreateMap();
        SolveTsp(prefectureList);
        MakeLine(prefectureList);
    }

    private List<PrefectureData> CreateMap()
    {
        var csv = CsvLoader.Load("Csv/tohoku_pref");
        var pointPrefab = Resources.Load<GameObject>("Prefabs/point");

        var ret = new List<PrefectureData>();
        csv.ForEach(itr =>
        {
            var data = new PrefectureData(itr);
            ret.Add(data);
            data.ownObj = Instantiate(pointPrefab);
            data.ownObj.name = data.name;
            data.ownObj.transform.position = new Vector3(data.latitude, data.longitude);
        });

        return ret;
    }

    private void MakeLine(List<PrefectureData> prefectureList)
    {
        _lineRenderer.positionCount = prefectureList.Count + 1;
        
        var startPref = prefectureList.First();
        var currentPref = startPref;
        int index = 0;
        do
        {
            _lineRenderer.SetPosition(index, currentPref.ownObj.transform.position);
            currentPref = currentPref.nextPrefecture;
            index++;
        } while (startPref != currentPref);
        
        _lineRenderer.SetPosition(index, startPref.ownObj.transform.position);
    }

    private void SolveTsp(List<PrefectureData> prefectureList)
    {
        // 現在居る県
        var currentPref = prefectureList[Random.Range(0, prefectureList.Count)];
        // スタート県
        var startPref = currentPref;
        // 巡回予定の県
        var targetPrefs = prefectureList.ToList();
        targetPrefs.Remove(startPref);
        
        // 前回検索した県
        PrefectureData prevPref = null;
        // 前回の距離を保存
        float prevDistance = 0f;
        // 未判定の県
        var undecidedPrefs = targetPrefs.ToList();
        
        while(true)
        {
            var nextPref = undecidedPrefs[Random.Range(0, undecidedPrefs.Count)];

            // 次の地点の距離
            float nextDistance = (nextPref.ownObj.gameObject.transform.position -
                                  currentPref.ownObj.gameObject.transform.position).magnitude;

            // 判定対象から削除
            undecidedPrefs.Remove(nextPref);

            // 判定対象がない場合
            if (prevPref == null)
            {
                if (undecidedPrefs.Count <= 0)
                {
                    // 巡回終了
                    currentPref.nextPrefecture = nextPref;
                    nextPref.nextPrefecture = startPref;
                    break;
                }
                prevPref = nextPref;
                prevDistance = nextDistance;
                continue;
            }

            // 二つの地点の距離判定
            if (nextDistance < prevDistance)
            {
                // 前回判定したものよりも距離が短い
                prevPref = nextPref;
                prevDistance = nextDistance;
            }
            
            // 判定対象がない場合は経路決定
            // または
            // 前回判定したものよりも距離が長くても、確率で距離が長くても経路として確定する
            if (undecidedPrefs.Count <= 0 /*|| Random.Range(1, 101) <= 80*/)
            {
                currentPref.nextPrefecture = prevPref;
                targetPrefs.Remove(prevPref);
                undecidedPrefs = targetPrefs.ToList();
                currentPref = prevPref;
                prevPref = null;
            }
        } 
    }
}
