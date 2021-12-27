using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab", order = 0)]
	public class SpawnTargetPrefabEffects : EffectStragtegy
	{
		[SerializeField] Transform prefabToSpawn;
		[SerializeField] float destroyDelay = -1;

		public override void StartEffect(AbilityData data, Action finished)
		{
			data.StartCoroutine(EffectCoroutine(data, finished));	
		}
		IEnumerator EffectCoroutine(AbilityData data, Action finished)
		{
			Transform instance = Instantiate(prefabToSpawn);
			instance.position = data.GetTargetedPoint();

			if (destroyDelay > 0)
			{
				yield return new WaitForSeconds(destroyDelay);
				Destroy(instance.gameObject);
			}

			finished();
			
		}
	}
}