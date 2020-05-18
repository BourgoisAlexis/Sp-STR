using UnityEngine;

public class UnitTank : Unit
{
    [SerializeField] private ParticleSystem shot;


    protected override void Awake()
    {
        base.Awake();

        attackDamage = 2;
        range = 4;
        speed = 10;
        attackRate = 1f;

        shot.Stop();
    }

    protected override void Attack()
    {
        base.Attack();
        shot.Play();
    }
}
