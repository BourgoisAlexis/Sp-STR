using UnityEngine;

public class Barrack : Entity
{
    [SerializeField] private Transform spawnPoint;


    protected override void Awake()
    {
        maxHP = 50;

        base.Awake();
    }


    public override void Select()
    {
        base.Select();

        GlobalManager.Instance.UIManager.ActiveUnitMenu(spawnPoint.position);
    }

    public override void UnSelect()
    {
        base.UnSelect();

        GlobalManager.Instance.UIManager.ShutDownMenus();
    }
}
