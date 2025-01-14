using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] string itemName;
    [SerializeField] int value;
    [SerializeField] Sprite icon;
}
