using UnityEngine;

public class Mine : Entity
{
    private int resources = 10;
    private int harvestValue;

    private void Start()
    {
        harvestValue = Random.Range(5, 10);
    }

    public void Harvest()
    {
        GlobalManager.Instance.UnitSpawner.GetMoney(harvestValue);
        resources --;

        if (resources <= 0)
            Destroyed(false);
    }
}
