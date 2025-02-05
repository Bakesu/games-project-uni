using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] protected float _attackDistance;
    [SerializeField] protected float _maxAttackDistance;
    [SerializeField] protected float damageDealt;

    [Header("Movement")]
    [SerializeField] private float moveDistance;

    public abstract void TakeDamage();

}
