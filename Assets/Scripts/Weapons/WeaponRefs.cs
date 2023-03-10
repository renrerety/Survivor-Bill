using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponRefs : MonoBehaviour
{
    public static WeaponRefs Instance;
    [SerializeField] public List<WeaponMaster> weaponRefs = new List<WeaponMaster>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else if (Instance == null)
        {
            Instance = this;
        }
    }

    public WeaponMaster FindWeaponRef(string name)
    {
        foreach (WeaponMaster weapon in weaponRefs)
        {
            if (weapon.name == name)
            {
                return weapon;
            }
        }

        return null;
    }
}
