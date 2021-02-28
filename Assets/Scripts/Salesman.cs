using System;
using Random = UnityEngine.Random;

[Serializable]
public class Salesman
{
    public float defaultStamina = 100;
    // スタミナがMaxの時、長い距離を選択するときのレート
    public float rate = 0.5f;
    
    public float _stamina;
 
    public Salesman()
    {
        _stamina = defaultStamina;
    }

    /// <summary>
    /// 距離が長くても選択するかどうか
    /// </summary>
    public bool Judge()
    {
        float rot = Random.Range(0, defaultStamina);
        return rot < _stamina * rate;
    }

    public void Travel(float distance)
    {
        _stamina -= distance;
        if (_stamina < 0)
        {
            _stamina = 0;
        }
    }

    public void Reset()
    {
        _stamina = defaultStamina;
    }
}