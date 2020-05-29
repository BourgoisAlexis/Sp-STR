using UnityEngine;

public class UnitTank : Unit
{
    [SerializeField] private ParticleSystem shot;


    protected override void Awake()
    {
        maxHP = 30;
        attackDamage = 2;
        range = 4;
        speed = 7;
        attackRate = 1f;
        cost = 10;

        base.Awake();
    }

    protected override void Attack()
    {
        base.Attack();
        shot.Play();
    }
}
