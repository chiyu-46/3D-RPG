using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack; 
    public CharacterData_SO templateData;
    
    public AttackData_SO attackData;
    //用于备份attackData作为基础攻击力
    private AttackData_SO baseAttackData;

    [Header("Weapon")] 
    public Transform weaponSlot;
    [HideInInspector] 
    public bool isCritical;
    [HideInInspector]
    public CharacterData_SO characterData;


    private void Awake()
    {
        if (templateData)
        {
            characterData = Instantiate(templateData);
        }

        baseAttackData = Instantiate(attackData);
    }

    #region Read from Data_SO
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentHealth = value;
        }
    }
    public int BaseDefence{
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public int CurrentDefence{
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    #region Read from AttackData
    public float attackRange{
        get
        {
            if (attackData != null)
            {
                return attackData.attackRange;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.attackRange = value;
        }
    }
    public float skillRange{
        get
        {
            if (attackData != null)
            {
                return attackData.skillRange;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.skillRange = value;
        }
    }
    public float coolDown{
        get
        {
            if (attackData != null)
            {
                return attackData.coolDown;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.coolDown = value;
        }
    }
    public int minDamge{
        get
        {
            if (attackData != null)
            {
                return attackData.minDamge;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.minDamge = value;
        }
    }
    public int maxDamge{
        get
        {
            if (attackData != null)
            {
                return attackData.maxDamge;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.maxDamge = value;
        }
    }

    public float criticalMultiplier{
        get
        {
            if (attackData != null)
            {
                return attackData.criticalMultiplier;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.criticalMultiplier = value;
        }
    }
    public float criticalChance{
        get
        {
            if (attackData != null)
            {
                return attackData.criticalChance;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            attackData.criticalChance = value;
        }
    }
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStates attack, CharacterStates defender)
    {
        int damage = Mathf.Max(attack.CurrentDamage() - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attack.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Hit");
        }

        //TODO:Update ui
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //TODO:经验up
        if (CurrentHealth <= 0)
        {
            attack.characterData.UpdateExp(characterData.killPoint);
        }
    }

    public void TakeDamage(int damage, CharacterStates defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
    }
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }

        return (int) coreDamage;
    }

    #endregion

    #region Equip Weapon

    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
            //TODO:更新属性（根据等级等条件）
            //TODO:切换武器动画
            attackData.ApplyWeaponData(weapon.weaponData);
        }
    }

    public void UnEquiWeapon()
    {
        if (weaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);
        //TODO:切换武器动画
    }

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquiWeapon();
        EquipWeapon(weapon);
    }
    #endregion
}
