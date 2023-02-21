using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName ="Custom/Weapons/Fireball")]
public class FireballWeapon : WeaponMaster
{
    public FireballPool _fireballPool;
    [SerializeField] AudioClip fireballClip;
    Transform nearestEnemy;
    float temp = 100f;

    public override void Attack()
    {
        FindNearestEnemy();

        GameObject fireball = _fireballPool.TakeFireballFromPool();
        fireball.transform.position = playerTransform.position;
        fireball.transform.right = nearestEnemy.position - playerTransform.position;
        
        timer = cooldown;
    }

    public override void LevelUp()
    {
        WeaponMaster weapon =_playerWeapons.FindWeapon("Fireball");
        weapon.maxHit += 1;
        weapon.cooldown -= 0.2f;
        if (weapon.cooldown < 0.5f)
        {
            weapon.cooldown = 0.5f;
        }
        weapon.damage += 1;
    }
    
    public override void ReturnWeaponToPool(GameObject weapon)
    {
        _fireballPool.ReturnFireballToPool(weapon);
    }

    void FindNearestEnemy()
    {
        foreach (GameObject enemy in EnemySpawner.Instance.activeEnemyList)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance < temp)
            {
                nearestEnemy = enemy.transform;
                temp = distance;
            }
        }
        temp = 100;
    }
}
