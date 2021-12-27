using RPG.Saving;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour,ISaveable
	{
		[SerializeField] float experiencePoints = 0;

		//public delegate void ExperienceGainedDelegate();
		public event Action onExperienceGained;

		private void Update()
		{
			if (Input.GetKey(KeyCode.E))
			{
				GainExperiencePoints(Time.deltaTime * 1000);
			}
		}

		public void GainExperiencePoints(float experience)
		{
			experiencePoints += experience;
			onExperienceGained();
		}
		public float GetPoints()
		{
			return experiencePoints;
		}
		public object CaptureState()
		{
			return experiencePoints;
		}
		public void RestoreState(object state)
		{
			experiencePoints = (float)state;
		}

	}
}