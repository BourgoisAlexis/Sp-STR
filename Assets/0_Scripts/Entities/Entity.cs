using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    #region Variables
    protected List<Unit> targetedBy = new List<Unit>();
    protected int indexInGame;

    //Accessors
    public int Index => indexInGame;
    #endregion


    public void SetIndex(int _index)
    {
        indexInGame = _index;
    }

    public void AddTargetter(Unit _targetter)
    {
        if (!targetedBy.Contains(_targetter))
            targetedBy.Add(_targetter);
    }

    public void RemoveTargetter(Unit _targetter)
    {
        if (targetedBy.Contains(_targetter))
            targetedBy.Remove(_targetter);
    }

    public virtual void Destroyed(bool _fromNet)
    {
        List<Unit> toClean = new List<Unit>(targetedBy);
        for (int i = 0; i < toClean.Count; i++)
            toClean[i].SetTarget(null, false);

        GlobalManager.Instance.FXExplosion(transform.position);
        GlobalManager.Instance.EntityManager.RemoveEntity(indexInGame);

        if (!_fromNet)
            GlobalManager.Instance.OnlineManager.DestroyUnit(Index);

        Destroy(gameObject);
    }
}
