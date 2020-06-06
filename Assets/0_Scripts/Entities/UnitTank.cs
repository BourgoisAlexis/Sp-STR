using UnityEngine;

public class UnitTank : UnitSoldier
{
    [SerializeField] private ParticleSystem shot;


    protected override void Attack()
    {
        base.Attack();

        shot.Play();
    }
}
