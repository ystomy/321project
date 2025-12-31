using UnityEngine;

[CreateAssetMenu(menuName = "Game/DealerData")]
public class DealerData : ScriptableObject
{
    public string dealerName;

    [Header("‰Šú’l")]
    [Range(0, 100)]
    public int initialLuck = 50;
    [Range(0, 100)]
    public int initialHeat = 0;

    [Header("«Šiƒƒ‚")]
    [TextArea]
    public string description;
}
