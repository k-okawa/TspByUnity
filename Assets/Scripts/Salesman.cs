using System;
using Random = UnityEngine.Random;

[Serializable]
public class Salesman
{
    public float defaultStamina = 100;
    // スタミナがMaxの時、長い距離を選択するときのレート
    public float rate = 0.5f;
    public float decreaseRate = 0.98f;
    public float finalStamina = 1.0e-7f;
    
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

    public void Travel() 
    {
        stamina *= decreaseRate;
    }

    public void Reset()
    {
        stamina = defaultStamina;
    }
}