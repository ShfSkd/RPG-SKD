using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
    {
		private bool playerHasWalkThrow;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player") &&!playerHasWalkThrow)
			{
				playerHasWalkThrow = true;
				GetComponent<PlayableDirector>().Play();
			}
		}
	}
}
