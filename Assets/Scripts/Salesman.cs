using System;
using Random = UnityEngine.Random;

[Serializable]
public class Salesman
{
    // 初期体力
    public float defaultStamina = 100;
    // 確率調整用のパラメータ
    public float rate = 1f;
    // 体力減少率
    public float decreaseRate = 0.98f;
    // この数値以下になった時、巡回を終了する
    public float finalStamina = 1.0e-7f;
    // 現在の体力
    public float stamina;
 
    public Salesman()
    {
        stamina = defaultStamina;
    }

    /// <summary>
    /// 距離が長くても選択するかどうか
    /// </summary>
    public bool Judge()
    {
        float rot = Random.Range(0, defaultStamina);
        return rot < stamina * rate;
    }

    /// <summary>
    /// 体力を減少する
    /// </summary>
    public void Travel() 
    {
        stamina *= decreaseRate;
    }

    /// <summary>
    /// 体力リセット
    /// </summary>
    public void Reset()
    {
        stamina = defaultStamina;
    }
}