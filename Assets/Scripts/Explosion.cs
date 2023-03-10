using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Explosion : MonoBehaviour
{
    [HideInInspector] public PlayerWeapons _playerWeapons;
    [HideInInspector] public BombPool _bombPool;
    private void OnEnable()
    {
        GetComponent<Animator>().Play("Explosion");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<AIMaster>().TakeDamage(_playerWeapons.FindWeapon("Bomb").damage);
        }
    }

    public void ReturnToPool()
    {
        _bombPool.ReturnBombToPool(transform.parent.gameObject);
    }
}
