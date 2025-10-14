using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LootSpace : MonoBehaviour
{

	[SerializeField] private List<GameObject> _lootPoints;
	public List<GameObject> LootPointsToDraw => _lootPoints;

	[SerializeField] private List<ItemsChances> _itemsObjectsToSpawn;

	[SerializeField] private bool _spawnOnStart = false;
	public bool SpawnOnStart => _spawnOnStart;

	private bool _itemsIsSpawned = false;

	private void Start()
	{
		for (int i = 0; i < _lootPoints.Count; i++)
		{
			if (_lootPoints[i].transform.childCount > 0)
				DestroyImmediate(_lootPoints[i].transform.GetChild(0).gameObject);
		}
	}

	private void OnEnable()
	{
		if (_spawnOnStart)
			s_LootSpawn.SpawnLoot += InGameInstantiateObjects;
	}

	private void OnDisable()
	{
		s_LootSpawn.SpawnLoot -= InGameInstantiateObjects;
	}

	public void InGameInstantiateObjects()
	{
		if (_itemsIsSpawned) return;

		InstantiateObjects();
	}

	private void InstantiateObjects()
	{
		_itemsIsSpawned = true;

		int MaxChance = 0;

		for (int i = 0; i < _itemsObjectsToSpawn.Count; i++)
		{
			MaxChance += _itemsObjectsToSpawn[i].SpawnChanceProcent;
		}
		if (MaxChance < 100) MaxChance = 100;


		for (int i = 0; i < _lootPoints.Count; i++)
		{
			int RandomInt = Random.RandomRange(1, MaxChance + 1);
			int Interval = 0;

			for (int g = 0; g < _itemsObjectsToSpawn.Count; g++)
			{
				Interval += _itemsObjectsToSpawn[g].SpawnChanceProcent;

				if (RandomInt <= Interval)
				{
					GameObject go = Instantiate(_itemsObjectsToSpawn[g].ItemObjectToSpawn, _lootPoints[i].transform.position, _lootPoints[i].transform.rotation);
					go.transform.parent = _lootPoints[i].transform;
					break;
				}
			}
		}
	}

	private void OpenWithKey(GameObject PlayerTransform)
	{
		Debug.Log("Open");
	}

#if UNITY_EDITOR

	public void CreateLootPoint()
	{
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_lootPoints.Add(go);
		go.transform.position = transform.position;
		go.transform.rotation = transform.rotation;
		go.transform.parent = transform;
		go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		go.name = "Loot spawn point";

		DestroyImmediate(go.GetComponent<MeshRenderer>());
		DestroyImmediate(go.GetComponent<BoxCollider>());

		for (int i = 0; i < _lootPoints.Count; i++)
		{
			if (_lootPoints[i] == null)
				_lootPoints.Remove(_lootPoints[i]);
		}
	}

	public void SummonLoot()
	{
		ClearLoot();
		InstantiateObjects();
	}

	public void ClearLoot()
	{
		for (int i = 0; i < _lootPoints.Count; i++)
		{
			if (_lootPoints[i].transform.childCount > 0)
			DestroyImmediate(_lootPoints[i].transform.GetChild(0).gameObject);
		}
	}

	private void OnDrawGizmos()
	{

		Gizmos.color = Color.green;

		foreach (GameObject go in _lootPoints)
		{
			if (go != null)
			{
				Gizmos.matrix = go.transform.localToWorldMatrix;

				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			}
		}
	}

#endif
}



[System.Serializable]
public struct ItemsChances
{
	public GameObject ItemObjectToSpawn;
	[Range(0, 100)]
	public int SpawnChanceProcent;
}