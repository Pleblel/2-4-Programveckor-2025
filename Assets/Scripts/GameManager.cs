using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//Elm
[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    public bool isQuestItem;

    public Item(string name, Sprite icon, bool stackable, bool questItem)
    {
        itemName = name;
        this.icon = icon;
        isStackable = stackable;
        isQuestItem = questItem;
    }

    public virtual void Use()
    {
        Debug.Log($"Using item: {itemName}");
    }
}