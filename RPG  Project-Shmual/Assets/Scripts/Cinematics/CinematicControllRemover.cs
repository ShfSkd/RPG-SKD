using RPG.Control;
using RPG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicControllRemover : MonoBehaviour
	{
		GameObject player;
		private void Awake()
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
		private void OnEnable()
		{
			GetComponent<PlayableDirector>().played += DisbleControl;
			GetComponent<PlayableDirector>().stopped += EnableControl;
		}
		private void OnDisable()
		{
			GetComponent<PlayableDirector>().played -= DisbleControl;
			GetComponent<PlayableDirector>().stopped -= EnableControl;
		}
		void DisbleControl(PlayableDirector playable)
		{
			player.GetComponent<ActionScheduler>().CancelCurrentAction();
			player.GetComponent<PlayerController>().enabled = false;
		}
		void EnableControl(PlayableDirector playable)
		{
			player.GetComponent<PlayerController>().enabled = true;

		}
	}
}