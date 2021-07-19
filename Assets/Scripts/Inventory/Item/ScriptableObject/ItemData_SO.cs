using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品的类型：可使用，武器，装备
public enum ItemType{ Useable, Weapon, Armor }

[CreateAssetMenu(fileName = "New Item",menuName = "Inventory")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    [TextArea]
    public string description = "";
    
    //是否可堆叠
    public bool stackable;

    [Header("Weapon")] 
    public GameObject weaponPrefab;
}
