﻿using UnityEngine;

public class UnitTank : Soldier
{
    [SerializeField] private ParticleSystem shot;

    protected override void Awake()
    {
        maxHP = 30;
        attackDamage = 2;
        range = 4;
        speed = 7;
        attackRate = 1f;

        base.Awake();
    }

    protected override void Attack()
    {
        base.Attack();
        shot.Play();
    }
}
