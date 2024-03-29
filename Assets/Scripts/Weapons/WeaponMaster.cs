using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponMaster : ScriptableObject
{
    public string translateKey;
    public PlayerWeapons _playerWeapons;
    [HideInInspector] public Transform playerTransform;
    public GameObject weaponObj;
    public float cooldown;
    public int damage;
    public int attackAmount;
    public float delay;
    public float duration;

    public string name;
    public string desc;
    [TextArea] public string levelUpDesc;
    public int level = 1;
    public int maxHit;
    public Sprite image;

    [HideInInspector] public float timer;

    int index;

    public abstract void Attack();
    public abstract void LevelUp();

    public void Init(string translateKey,GameObject weaponObj,float delay,float duration, float cooldown,int damage,int attackAmount,int maxHit,string name,string desc,string levelUpDesc,Sprite image)
    {
        this.translateKey = translateKey;
        this.weaponObj = weaponObj;
        this.delay = delay;
        this.duration = duration;
        this.cooldown = cooldown;

        this.damage = damage + PlayerData.instance.persistentData.upgrades.damageUp;
        this.attackAmount = attackAmount;
        this.maxHit = maxHit;
        this.name = name;
        this.desc = desc;
        this.levelUpDesc = levelUpDesc;
        this.image = image;
    }

    public void Translate()
    {
        name = Localization.Localization.instance.GetString(translateKey);
        desc = Localization.Localization.instance.GetString(translateKey + "Desc");
        levelUpDesc = Localization.Localization.instance.GetString(translateKey + "LvlDesc");
    }

    public abstract void ReturnWeaponToPool(GameObject weapon);
}
