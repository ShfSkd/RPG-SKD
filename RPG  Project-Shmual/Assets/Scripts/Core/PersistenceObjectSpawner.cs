using System;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
	public class PersistenceObjectSpawner : MonoBehaviour
	{
		[SerializeField] GameObject persistenceObjectPrefab;

		static bool hasSpawn;
		private void Awake()
		{
			if (hasSpawn) return;
			SpawnPersistenceObject();

			hasSpawn = true;
		}

		private void SpawnPersistenceObject()
		{
			GameObject persistenceObject = Instantiate(persistenceObjectPrefab);
			DontDestroyOnLoad(persistenceObject);
		}
	}
}