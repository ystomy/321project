[System.Serializable]
public class Card
{
    public int suit;   // 0:♠ 1:♥ 2:♦ 3:♣  / 4:Joker
    public int number; // 1-13 / Jokerは0,1
    public int value;    // BJ計算用（A=11とか）
    public string Id => $"{suit}-{number}";
}
