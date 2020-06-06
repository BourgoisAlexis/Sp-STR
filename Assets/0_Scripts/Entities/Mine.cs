using UnityEngine;

public class Mine : Entity
{
    #region Variables
    private int harvestNumber = 10;
    private int harvestValue;
    #endregion


    private void Start()
    {
        harvestValue = Random.Range(5, 10);
    }

    public void Harvest()
    {
        GlobalManager.Instance.UnitSpawner.GetMoney(harvestValue);
        harvestNumber --;

        if (harvestNumber <= 0)
            Destroyed(false);
    }
}
