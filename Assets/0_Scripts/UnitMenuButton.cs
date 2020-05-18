using UnityEngine;

public class UnitMenuButton : Button
{
    [SerializeField] private int index;
    [SerializeField] private UnitSpawner unitSpawner;

    public override void Execute()
    {
        unitSpawner.SpawnUnit(index);
    }
}
