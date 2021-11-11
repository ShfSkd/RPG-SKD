using RPG.Control;
using RPG.Saving;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagment
{
	public class Portal : MonoBehaviour
	{
		enum DestinationIdetifier
		{
			A, B, C, D, E
		}

		[SerializeField] int sceneToLoad = -1;
		[SerializeField] Transform spawnPoint;
		[SerializeField] DestinationIdetifier destination;
		[SerializeField] float fadeInTime = 2f;
		[SerializeField] float fadeOutTime = 1f;
		[SerializeField] float fadeWaitTime = 0.5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				StartCoroutine(Transition());
			}
		}
		IEnumerator Transition()
		{
			if (sceneToLoad < 0)
			{
				Debug.LogError("Scene to load not set");
				yield break;
			}
			DontDestroyOnLoad(gameObject);

			Fader fader = FindObjectOfType<Fader>();
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			player.enabled = false;

			yield return fader.FadeOut(fadeOutTime);

			savingWrapper.Save();

			yield return SceneManager.LoadSceneAsync(sceneToLoad);
			PlayerController newPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			newPlayer.enabled = false;

			savingWrapper.Load();

			Portal otherPortal = GetOtherPortal();
			UpdatePortal(otherPortal);

			savingWrapper.Save();

			yield return new WaitForSeconds(fadeWaitTime);
			fader.FadeIn(fadeInTime);

			newPlayer.enabled = true;
			Destroy(gameObject);


		}

		private void UpdatePortal(Portal otherPortal)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<NavMeshAgent>().enabled = false;
			player.transform.position = otherPortal.spawnPoint.position;
			player.transform.rotation = otherPortal.spawnPoint.rotation;
			player.GetComponent<NavMeshAgent>().enabled = true;
		}

		private Portal GetOtherPortal()
		{
			foreach (Portal portal in FindObjectsOfType<Portal>())
			{
				if (portal == this) continue;

				if (portal.destination != destination) continue;

				return portal;

			}
			return null;

		}
	}
}