using UnityEngine;

public class Barrack : Entity
{
    [SerializeField] private Transform spawnPoint;

    public override void Select()
    {
        base.Select();

        _teamManager.UIManager.ActiveUnitMenu(spawnPoint.position, _teamManager);
    }
}
