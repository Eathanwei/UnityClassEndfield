using System;
using UnityEngine;

public class CombatEventManager : MonoBehaviour
{
    public static event Action IceDebuff;
    public static event Action ConsumeDebuff;
    public static event Action Heavy;
    public static event Action Infliction;

    // 廣播函式：供攻擊者呼叫
    public static void BroadcastIceDebuff()
    {
        IceDebuff?.Invoke();
    }
    public static void BroadcastConsumeDebuff()
    {
        ConsumeDebuff?.Invoke();
    }
    public static void BroadcastHeavy()
    {
        Heavy?.Invoke();
    }
    public static void BroadcastInfliction()
    {
        Infliction?.Invoke();
    }
}
