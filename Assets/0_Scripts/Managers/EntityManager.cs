using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private Camera mainCamera;
    [SerializeField] private e_Teams team;
    [SerializeField] private RectTransform selectionZoneCanvas;
    [SerializeField] private RectTransform selectionZone;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] private List<Entity> entities = new List<Entity>();
    private Vector3 startingPoint;
    private Vector3 currentPoint;
    private float margin = 1.5f; //Margin to display the selection zone
    private List<Unit> selectedUnits = new List<Unit>();
    private Entity selectedEntity;
    private UIManager _uiManager;

    //Accessors
    public int teamIndex => (int)team;
    #endregion


    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
        selectionZone.localScale = new Vector2(0, 0);
    }

    private void Update()
    {
        LeftClick();
        RightClick();
    }

    public void SetTeam(int _index)
    {
        team = (e_Teams)_index;
    }


    //Entities
    public void AddEntity(Entity _entity)
    {
        entities.Add(_entity);
        _entity.SetIndex(entities.Count - 1);
    }

    public void RemoveEntity(int _index)
    {
        entities[_index] = null;
    }


    //FromNet
    public void SetUnitDestination(int _index, Vector3 _desti)
    {
        entities[_index]?.GetComponent<Unit>()?.SetDestination(_desti, true);
    }

    public void SetUnitTarget(int _index, int _targetIndex)
    {
        entities[_index]?.GetComponent<Unit>()?.SetTarget(_targetIndex < 0 ? null : entities[_targetIndex], true);
    }

    public void DestroyUnit(int _index)
    {
        entities[_index].Destroyed(true);
    }

    public void UpdateHealth(int _index, int _value)
    {
        entities[_index].CorrectHealth(_value);
    }


    //Inputs
    private void LeftClick()
    {
        Vector2 mousePos = Input.mousePosition;

        //Selection Zone
        Vector3 diff = currentPoint - startingPoint;
        Vector3 pos = currentPoint - diff / 2;
        Vector2 scale = new Vector2(diff.x, diff.z);

        if (Input.GetMouseButtonDown(0) && !MouseOverUI())
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                startingPoint = hit.point;
                currentPoint = hit.point;

                Unit _unit = hit.transform.GetComponent<Unit>();
                Entity _entity = hit.transform.GetComponent<Entity>();

                UnselectEntity();

                if (_unit)
                {
                    if (_unit.Team == team)
                    {
                        if (!selectedUnits.Contains(_unit))
                            SelectUnit(_unit);
                    }
                    else
                    {
                        SendUnits(hit.point, _unit);
                    }
                }
                else if (_entity)
                {
                    if (_entity.Team == team)
                        SelectEntity(_entity);
                    else
                        SendUnits(hit.point, _entity);
                }
                else
                {
                        UnselectAllUnits();
                        UnselectEntity();
                }
            }
        }
        if (Input.GetMouseButton(0) && !MouseOverUI())
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 100, groundLayer))
            {
                currentPoint = hit.point;
                selectionZoneCanvas.position = new Vector3(0, hit.point.y + 0.3f, 0);

                selectionZone.localPosition = new Vector3(pos.x, pos.z, 0);
                selectionZone.localScale = scale;
            }
        }
        if (Input.GetMouseButtonUp(0) && !MouseOverUI())
        {
            Collider[] list = Physics.OverlapBox(pos, new Vector3(Mathf.Abs(scale.x)/2, 1, Mathf.Abs(scale.y)/2), Quaternion.identity, entityLayer);

            foreach (Collider col in list)
                if (col.GetComponent<Unit>()?.Team == team)
                    SelectUnit(col.GetComponent<Unit>());

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
                startingPoint = hit.point;

            selectionZone.localScale = new Vector2(0, 0);
        }
    }

    private void RightClick()
    {
        if (Input.GetMouseButtonDown(1) && !MouseOverUI())
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, groundLayer))
                SendUnits(hit.point, null);
        }
    }


    //Units
    private void SelectUnit(Unit _unit)
    {
        _unit.Select();
        selectedUnits.Add(_unit);
    }

    private void UnselectUnit(Unit _unit)
    {
        _unit.UnSelect();
        selectedUnits.Remove(_unit);
    }

    private void UnselectAllUnits()
    {
        for (int i = selectedUnits.Count - 1; i >= 0; i--)
            UnselectUnit(selectedUnits[i]);
    }

    private void SendUnits(Vector3 _destination, Entity _target)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].SetDestination(_destination, false);
            selectedUnits[i].SetTarget(_target, false);
        }

        UnselectAllUnits();
        UnselectEntity();
    }
    
    
    //Entities
    private void SelectEntity(Entity _entity)
    {
        UnselectAllUnits();
        UnselectEntity();
        _entity.Select();
        selectedEntity = _entity;
    }

    private void UnselectEntity()
    {
        if(selectedEntity != null)
        {
            selectedEntity.UnSelect();
            selectedEntity = null;
        }
    }


    //Checks
    private bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
