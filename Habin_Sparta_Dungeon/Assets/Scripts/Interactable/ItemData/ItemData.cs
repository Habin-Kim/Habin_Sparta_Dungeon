using UnityEngine;

public enum ItemType
{
    /// <summary>
    /// 닿을 때 바로 작동
    /// </summary>
    Instant,

    /// <summary>
    /// E로 상호작용시 인벤토리
    /// </summary>
    Inventory
}

public enum InstantType
{
    Stamina,
    Speed
}

[System.Serializable]
public class ItemDataInstant
{
    public InstantType type;
    public float value;
    public float duration;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Instant Item Effects")]
    public ItemDataInstant[] Instants;
}
