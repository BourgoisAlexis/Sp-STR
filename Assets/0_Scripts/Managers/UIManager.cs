using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private GameObject healthBarPrefab;

    private Dictionary<Transform, Transform> needToFollow = new Dictionary<Transform, Transform>();
    private UnitSpawner _unitSpawner;
    #endregion


    private void Awake()
    {
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void LateUpdate()
    {
        foreach (KeyValuePair<Transform, Transform> follow in needToFollow)
            follow.Value.position = mainCamera.WorldToScreenPoint(follow.Key.position + Vector3.up * 0.7f);
    }


    public HealthBar SpawnHealthBar(Transform _owner)
    {
        GameObject instance = Instantiate(healthBarPrefab, uiCanvas);
        needToFollow.Add(_owner, instance.transform);
        instance.transform.SetAsFirstSibling();
        return instance.GetComponent<HealthBar>();
    }

    public void RemoveUI(Transform _owner)
    {
        needToFollow.Remove(_owner);
    }

    public void ActiveUnitMenu(Vector3 _spawnPoint, TeamManager _tm)
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("UnitMenu");
        _unitSpawner.SetupSpawn(_spawnPoint, _tm);
    }

    public void ShutDownMenus()
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("UnitMenu");
    }
}
