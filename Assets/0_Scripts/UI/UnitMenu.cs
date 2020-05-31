using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    private UnitMenuButton[] buttons;


    private void Awake()
    {
        buttons = GetComponentsInChildren<UnitMenuButton>();
    }


    public void SetTeam(int _index)
    {
        foreach (UnitMenuButton b in buttons)
            b.Setup(_index);
    }

    public void SetSpawn(Vector3 _spawnPoint)
    {
        foreach (UnitMenuButton b in buttons)
            b.SetSpawn(_spawnPoint);
    }
}
