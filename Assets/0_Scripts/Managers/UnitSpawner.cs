using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    #region Variables
    [SerializeField] private int delay;
    [SerializeField] private GameObject[] unitPrefabs;
    
    private bool canSpawn = true;
    private Vector3 spawnPoint;
    private TeamManager teamManager;
    #endregion


    public void SetupSpawn(Vector3 _pos, TeamManager _tm)
    {
        spawnPoint = _pos;
        teamManager = _tm;
    }

    public void SpawnUnit(int _index)
    {
        if(canSpawn)
        {
            GameObject instance = Instantiate(unitPrefabs[_index], spawnPoint, Quaternion.identity);
            instance.transform.SetParent(teamManager.transform);
            teamManager.AddEntity(instance.GetComponent<Unit>());
            canSpawn = false;
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        canSpawn = true;
    }
}
