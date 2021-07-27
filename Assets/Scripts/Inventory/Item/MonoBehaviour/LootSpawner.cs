using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item;
        
        //出现的权重
        [Range(0, 1)] 
        public float weight;
    }
    
    //可掉落物品列表
    public LootItem[] lootItems;
    
    //物品掉落
    public void SpawnLoot()
    {
        float currentValue = Random.value;
        for (int i = 0; i < lootItems.Length; i++)
        {
            if (currentValue <= lootItems[i].weight)
            {
                GameObject obj = Instantiate(lootItems[i].item);
                obj.transform.position = transform.position + Vector3.up * 2;
            }
        }
    }
}
