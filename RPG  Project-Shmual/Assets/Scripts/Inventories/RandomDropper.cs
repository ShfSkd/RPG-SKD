using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
	public class RandomDropper : ItemDropper
	{
		[Tooltip("How far the picups be spread grom the dropper.")]
		[SerializeField] float spreadDistance = 1;
		[SerializeField] DropLibrary dropLibrary;

		const int ATTEMPS = 30;

		public void RandomDrop()
		{
			var baseStats = GetComponent<BaseStats>();
			var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
			foreach (var drop in drops)
			{
				DropItem(drop.item, drop.number);
			}

		}

		protected override Vector3 GetDropLocation()
		{
			for (int i = 0; i < ATTEMPS; i++)
			{
				Vector3 randomPoint = transform.position + Random.insideUnitSphere * spreadDistance;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}
			return transform.position;
		}
	}
}