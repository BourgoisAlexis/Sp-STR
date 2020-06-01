using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGenerator : MonoBehaviour
{
    private void Awake()
    {
        Mine[] toAdd = GetComponentsInChildren<Mine>();
        foreach (Mine ent in toAdd)
            GlobalManager.Instance.EntityManager.AddEntity(ent);
    }
}
