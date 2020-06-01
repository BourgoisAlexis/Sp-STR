using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private e_Teams team;

    private List<Selectable> entities = new List<Selectable>();

    //Accessors
    public e_Teams Team => team;
    #endregion


    private void Start()
    {
        Selectable[] toAdd = GetComponentsInChildren<Selectable>();
        foreach (Selectable ent in toAdd)
            AddEntity(ent);
    }


    public void AddEntity(Selectable _entity)
    {
        if (!entities.Contains(_entity))
        {
            entities.Add(_entity);
            _entity.SetupTeam(team, this, GlobalManager.Instance.UIManager.SpawnHealthBar(_entity.transform));
            GlobalManager.Instance.EntityManager.AddEntity(_entity);
        }
    }

    public void RemoveEntity(Selectable _entity)
    {
        entities.Remove(_entity);
        GlobalManager.Instance.EntityManager.RemoveEntity(_entity.Index);
        GlobalManager.Instance.UIManager.RemoveHealthBar(_entity.transform);
    }
}
