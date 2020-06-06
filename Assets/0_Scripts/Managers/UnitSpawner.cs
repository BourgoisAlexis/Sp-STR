using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private int money;

    private OnlineManagerVERSUS _onlineManager;
    private GlobalManager _globalManager;
    private UIManager _uiManager;
    #endregion


    private void Awake()
    {
        _onlineManager = GetComponent<OnlineManagerVERSUS>();
        _globalManager = GetComponent<GlobalManager>();
        _uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        StartCoroutine(GenerateMoney());
        _uiManager.UpdateMoney(money);
    }


    public void SpawnUnit(int _index, Vector3 _pos, int _tmIndex, bool _fromNet)
    {
        GameObject instance = Instantiate(unitPrefabs[_index], _pos, Quaternion.identity);
        instance.transform.SetParent(_globalManager.TeamManagers[_tmIndex].transform);
        _globalManager.TeamManagers[_tmIndex].AddEntity(instance.GetComponent<Unit>());

        if (!_fromNet)
            _onlineManager.SpawnUnit(_index, _pos, _tmIndex);
    }


    //Money
    public int GetCost(int _index)
    {
        return unitPrefabs[_index].GetComponent<Unit>().Cost;
    }

    public bool Pay(int _index)
    {
        int cost = GetCost(_index);

        if (cost > money)
            return false;

        money -= cost;
        _uiManager.UpdateMoney(money);
        return true;
    }

    public void GetMoney(int _value)
    {
        money += _value;
        _uiManager.UpdateMoney(money);
    }

    private IEnumerator GenerateMoney()
    {
        while (true)
        {
            money++;
            _uiManager.UpdateMoney(money);
            yield return new WaitForSeconds(2);
        }
    }
}
