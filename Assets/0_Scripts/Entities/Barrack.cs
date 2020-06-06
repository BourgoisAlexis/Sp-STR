using UnityEngine;

public class Barrack : Selectable
{
    [SerializeField] private Transform spawnPoint;


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
