using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private e_Teams team;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private EntityManager _entityManager;

    private List<Entity> entities = new List<Entity>();

    //Accessors
    public UIManager UIManager => _uiManager;
    public e_Teams Team => team;
    #endregion


    private void Start()
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
            _entityManager.AddEntity(_entity);
        }
    }

    public void RemoveEntity(Entity _entity)
    {
        entities.Remove(_entity);
        _entityManager.RemoveEntity(_entity.Index);
        _uiManager.RemoveHealthBar(_entity.transform);
    }
}
