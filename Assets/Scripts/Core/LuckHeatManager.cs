using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckHeatManager : MonoBehaviour
{
    [Range(0, 100)]
    public int Luck = 50;
    [Range(0, 100)]
    public int Heat = 0;

    public void InitializeFromDealer(DealerData dealer)
    {
        Luck = dealer.initialLuck;
        Heat = dealer.initialHeat;
    }

    // ”’l‚É•â³‚ğ‚©‚¯‚é
    public float ApplyLuckHeat(float baseValue)
    {
        float luckBonus = Luck * 0.01f;
        float heatRange = 1f + Heat * 0.02f;

        return baseValue
             + Random.Range(-heatRange, heatRange)
             + luckBonus;
    }
}