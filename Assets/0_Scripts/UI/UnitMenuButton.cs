using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitMenuButton : Button
{
    #region Variables
    [SerializeField] private int index;
    [SerializeField] private int delay;

    private bool canSpawn = true;
    private Image _image;
    private int teamIndex;
    private Vector3 spawnPoint;
    #endregion


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Setup(int _index)
    {
        teamIndex = _index;
        GetComponentInChildren<TextMeshProUGUI>().text = GlobalManager.Instance.UnitSpawner.GetCost(index).ToString();
    }

    public void SetSpawn(Vector3 _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }

    public override void Execute()
    {
        if (canSpawn)
            if(GlobalManager.Instance.UnitSpawner.Pay(index))
            {
                GlobalManager.Instance.UnitSpawner.SpawnUnit(index, spawnPoint, teamIndex, false);
                StartCoroutine(Delay());
            }
    }

    private IEnumerator Delay()
    {
        canSpawn = false;
        float init = 0;
        
        while (init < delay)
        {
            yield return new WaitForEndOfFrame();
            init += Time.deltaTime;
            _image.fillAmount = init / delay;
        }
        canSpawn = true;
    }
}
