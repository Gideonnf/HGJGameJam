using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : SingletonBase<ObjectPooler>
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int Count;
	}

	[Tooltip("Pooled objects need to implement the IPooledObject interface")]
	public List<Pool> Pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	// Use this for initialization
	void Start () {
		poolDictionary = new Dictionary<string, Queue<GameObject>> ();

		foreach (Pool pool in Pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();
			for (int i = 0; i < pool.Count; i++)
			{
				GameObject obj = Instantiate (pool.prefab);
				obj.SetActive (false);
				objectPool.Enqueue (obj);
			}

			poolDictionary.Add (pool.tag, objectPool);
		}
	}
	
	/// <summary>
	/// Calls an object from the pool to be active
	/// </summary>
	/// <param name="_tag">the string tag given to the object in the inspector</param>
	/// <param name="_position">the position to spawn the object at</param>
	/// <param name="_rotation">the rotation to spawn the object at</param>
	/// <returns></returns>
	public GameObject SpawnFromPool (string _tag, Vector3 _position, Quaternion _rotation)
	{
		if (!poolDictionary.ContainsKey (_tag)) 
		{
			Debug.LogWarning ("Pool with tag " + _tag + " doesn't exist");
			return null;
		}
		GameObject objectToSpawn = poolDictionary [_tag].Dequeue ();

		objectToSpawn.SetActive (true);
		objectToSpawn.transform.position = _position;
		objectToSpawn.transform.rotation = _rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject> ();
		// If the object derives from the IpooledObject interface, call the onobjectspawn
		if (pooledObj != null) 
		{
			pooledObj.OnObjectSpawn ();
		}

		poolDictionary [_tag].Enqueue (objectToSpawn);
		Debug.Log ("new size of " + _tag + " is now " + poolDictionary [_tag].Count);
		return objectToSpawn;
	}
}
