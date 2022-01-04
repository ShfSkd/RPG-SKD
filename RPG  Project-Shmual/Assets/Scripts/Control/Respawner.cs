using Cinemachine;
using RPG.Attributes;
using RPG.SceneManagment;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
	public class Respawner : MonoBehaviour
	{
		[SerializeField] Transform respawnLocation;
		[SerializeField] float respawnDealy = 3;
		[SerializeField] float fadeTime = 0.2f;
		[SerializeField] float healthRegenPercentage = 20;
		[SerializeField] float enemyHealthRegenPercentage = 20;

		private void Awake()
		{
			GetComponent<Health>().onDie.AddListener(Respawn);
		}
		private void Start()
		{
			if (GetComponent<Health>().IsDead())
				Respawn();
		}
		private void Respawn()
		{
			StartCoroutine(RespawnRoutine());
		}
		IEnumerator RespawnRoutine()
		{
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			savingWrapper.Save();
			yield return new WaitForSeconds(respawnDealy);
			Fader fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fadeTime);
			RespawnPlayer();
			ResetEnemies();
			savingWrapper.Save();
			yield return fader.FadeIn(fadeTime);

		}

		private void ResetEnemies()
		{
			foreach (AIController enemyController in FindObjectsOfType<AIController>())
			{
				Health health = enemyController.GetComponent<Health>();
				if (health && !health.IsDead())
				{
					enemyController.Reset();
					health.Heal(health.GetMaxHealthPoints() * enemyHealthRegenPercentage / 100);
				}
			}
		}

		private void RespawnPlayer()
		{
			Vector3 positionDealth = respawnLocation.position - transform.position;
			GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
			Health health = GetComponent<Health>();
			health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
			ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
			if (activeVirtualCamera.Follow == transform)
			{
				activeVirtualCamera.OnTargetObjectWarped(transform, positionDealth);
			}
		}
	}
}