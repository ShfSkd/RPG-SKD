using RPG.Attributes;
using RPG.Combat;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Effects/Spawn Projectile Effect", order = 0)]
	public class SpawnProjectileEffect : EffectStragtegy
	{
		[SerializeField] Projectile projectileToSpawn;
		[SerializeField] float damage;
		[SerializeField] bool isRigthHand = true;
		[SerializeField] bool useTargetPoint = true;
		public override void StartEffect(AbilityData data, Action finished)
		{
			Fighter fighter = data.GetUser().GetComponent<Fighter>();
			Vector3 spawnPosition = fighter.GetHandTransform(isRigthHand).transform.position;
			if (useTargetPoint)
				SpawnProjetilesForTargetPoint(data,spawnPosition);
			else
				SpawnProjetilesForTargets(data, spawnPosition);
			finished();
		}

		private void SpawnProjetilesForTargetPoint(AbilityData data, Vector3 spawnPosition)
		{
			Projectile projectile = Instantiate(projectileToSpawn);
			projectile.transform.position = spawnPosition;
			projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
		}

		private void SpawnProjetilesForTargets(AbilityData data, Vector3 spawnPosition)
		{
			foreach (var target in data.GetTargets())
			{
				Health health = target.GetComponent<Health>();
				if (health)
				{
					Projectile projectile = Instantiate(projectileToSpawn);
					projectile.transform.position = spawnPosition;
					projectile.SetTarget(health, data.GetUser(), damage);
				}

			}
		}
	}
}