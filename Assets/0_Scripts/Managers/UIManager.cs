using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private UnitMenu _unitMenu;

    private Dictionary<Transform, Transform> uiTarget = new Dictionary<Transform, Transform>();
    #endregion


    //HealthBars
    private void LateUpdate()
    {
        foreach (KeyValuePair<Transform, Transform> follow in uiTarget)
            follow.Value.position = mainCamera.WorldToScreenPoint(follow.Key.position + Vector3.up * 0.7f);
    }

    public HealthBar SpawnHealthBar(Transform _owner)
    {
        GameObject instance = Instantiate(healthBarPrefab, uiCanvas);
        uiTarget.Add(_owner, instance.transform);
        instance.transform.SetAsFirstSibling();
        return instance.GetComponent<HealthBar>();
    }

    public void RemoveHealthBar(Transform _owner)
    {
        uiTarget.Remove(_owner);
    }


    public void EndWaitingScreen()
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("WS_Out");
    }

    public void ActiveUnitMenu(Vector3 _spawnPoint)
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("UnitMenu");
        _unitMenu.SetSpawn(_spawnPoint);
    }

    public void UpdateMoney(int _value)
    {
        money.text = _value.ToString();
    }
    

    //End
    public void Deafeat()
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("Defeat");
    }

    public void Victory()
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("Victory");
    }


    public void ShutDownMenus()
    {
        uiCanvas.GetComponent<Animator>().SetTrigger("UnitMenu");
    }
}
