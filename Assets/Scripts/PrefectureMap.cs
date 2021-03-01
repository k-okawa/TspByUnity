using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PrefectureMap : MonoBehaviour
{
    private const string POINT_PREFAB_PATH = "Prefabs/point";
    
    [SerializeField] private string _csvPath = "Csv/pref";
    [SerializeField] private Salesman _salesman;
    [SerializeField] private Text _distanceText;

    private List<PrefectureData> _prefectureList;
    private List<Vector3> _pointList = new List<Vector3>();
    
    private void Start()
    {
        _prefectureList = CreateMap();
        float distance = SolveTsp(_prefectureList);
        MakePointList(_prefectureList);
        _distanceText.text = $"Distance:{distance}";
    }

    public void OnClickRetry()
    {
        _prefectureList.ForEach(itr => itr.Reset());
        float distance = SolveTsp(_prefectureList);
        MakePointList(_prefectureList);
        _distanceText.text = $"Distance:{distance}";
    }
    
    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    private void OnRenderObject()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(new Color(0,1,1));
        foreach (var point in _pointList)
        {
            GL.Vertex(point);
        }
        GL.End();
        GL.PopMatrix();
    }

    private List<PrefectureData> CreateMap()
    {
        var csv = CsvLoader.Load(_csvPath);
        var pointPrefab = Resources.Load<GameObject>(POINT_PREFAB_PATH);

        var ret = new List<PrefectureData>();
        csv.ForEach(itr =>
        {
            var data = new PrefectureData(itr);
            ret.Add(data);
            data.ownObj = Instantiate(pointPrefab, this.transform);
            data.ownObj.name = data.name;
            data.ownObj.transform.localPosition = new Vector3(data.latitude, data.longitude);
        });

        return ret;
    }

    private void MakePointList(List<PrefectureData> prefectureList)
    {
        _pointList.Clear();
        
        var startPref = prefectureList.First();
        var currentPref = startPref;
        do
        {
            _pointList.Add(currentPref.ownObj.transform.position);
            currentPref = currentPref.nextPrefecture;
        } while (startPref != currentPref);
        
        _pointList.Add(startPref.ownObj.transform.position);
    }

    private float SolveTsp(List<PrefectureData> prefectureList)
    {
        _salesman.Reset();
        
        // 現在居る県
        //var currentPref = prefectureList[Random.Range(0, prefectureList.Count)];
        var currentPref = prefectureList.Last();
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
        
        // 総移動距離
        float totalDistance = 0f;

        int loopCount = 0;
        while(true)
        {
            loopCount++;
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
                    totalDistance += nextDistance;
                    nextPref.nextPrefecture = startPref;
                    totalDistance += (nextPref.ownObj.transform.position - startPref.ownObj.transform.position).magnitude;
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
            } else if (_salesman.Judge())
            {
                // 距離が長くても交換する
                prevPref = nextPref;
                prevDistance = nextDistance;
                _salesman.Travel(nextDistance);
            }
            
            // 判定対象がない場合は経路決定
            if (undecidedPrefs.Count <= 0)
            {
                currentPref.nextPrefecture = prevPref;
                totalDistance += prevDistance;
                targetPrefs.Remove(prevPref);
                undecidedPrefs = targetPrefs.ToList();
                currentPref = prevPref;
                prevPref = null;
            }
        }
        Debug.Log($"LoopCount:{loopCount}");
        return totalDistance;
    }
}
