using UnityEngine;
using System.Collections.Generic;

public class FXManager : MonoBehaviour
{
    #region Variables
    public GameObject Impact;
    public GameObject Explosion;

    private Transform poolParent;
	Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>> ();
    #endregion


    private void Awake()
    {
		if (!poolParent)
			poolParent = transform;

        AddTier (Impact, 10);
        AddTier (Explosion, 5);
    }


	public void AddTier (GameObject _prefab, int _size)
	{
		int key = _prefab.GetInstanceID ();
		if (poolDictionary.ContainsKey (key))
			return;

		Queue<GameObject> queue = new Queue<GameObject> ();
		for (int i = 0; i < _size; i++)
		{
			GameObject newObject = Instantiate (_prefab) as GameObject;
			newObject.SetActive (false);
			newObject.transform.SetParent (poolParent);
			queue.Enqueue (newObject);
		}

		poolDictionary.Add (key, queue);
	}

	public void InstantiateFX (GameObject _prefab, Vector3 _position)
	{
		int key = _prefab.GetInstanceID ();
		if (!poolDictionary.ContainsKey (key))
		{
			Debug.LogWarning ("This pool does not exist");
			return;
		}

		GameObject instance = poolDictionary[key].Dequeue ();
		poolDictionary[key].Enqueue (instance);
		
		instance.SetActive (true);
		instance.transform.position = _position;
		instance.transform.rotation = Quaternion.identity;
		instance.GetComponent<VFXPlay>().Play();
	}
}