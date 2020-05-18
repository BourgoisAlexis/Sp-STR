using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private e_Teams team;
    [SerializeField] private UIManager _uiManager;

    private List<Entity> entities = new List<Entity>();

    //Accessors
    public UIManager UIManager => _uiManager;
    #endregion


    private void Awake()
    {
        Entity[] toAdd = GetComponentsInChildren<Entity>();
        foreach (Entity ent in toAdd)
            AddEntity(ent);
    }


    public void AddEntity(Entity _entity)
    {
        if (!entities.Contains(_entity))
        {
            entities.Add(_entity);
            _entity.SetupTeam(team, this, _uiManager.SpawnHealthBar(_entity.transform));
        }
    }

    public void RemoveEntity(Entity _entity)
    {
        entities.Remove(_entity);
        _uiManager.RemoveUI(_entity.transform);
    }
}
