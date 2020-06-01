public class UnitDefault : Soldier
{
    protected override void Awake()
    {
        maxHP = 20;
        attackDamage = 1;
        range = 3;
        speed = 5;
        attackRate = 0.5f;

        base.Awake();
    }
}
