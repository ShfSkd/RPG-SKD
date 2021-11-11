using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
	public class Health : MonoBehaviour,ISaveable
	{
		[SerializeField] float regenerationPercentage = 70f;
		[SerializeField] UnityEvent<float> takeDamage;
		[SerializeField] UnityEvent onDie;

		LazyValue<float> healthPoints;

		bool isDead;
		private void Awake()
		{
			healthPoints = new LazyValue<float>(GetInitialHealth);
		}
		float GetInitialHealth()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}
		private void Start()
		{
			healthPoints.ForceInit();
		}

		private void OnEnable()
		{
			BaseStats baseStat = GetComponent<BaseStats>();
			if (baseStat != null)
			{
				baseStat.onLevelup += RegenerateHealth;
			}
		}
		private void OnDisable()
		{
			BaseStats baseStat = GetComponent<BaseStats>();
			if (baseStat != null)
			{
				baseStat.onLevelup -= RegenerateHealth;
			}
		}
		public bool IsDead()
		{
			return isDead;
		}

		public void TakeDamage(GameObject instigator,float damage)
		{
			print(gameObject.name + " tool damage: " + damage);
			
			healthPoints.value = Mathf.Max(healthPoints.value - damage,0);
			if (healthPoints.value <= 0)
			{
				onDie.Invoke();
				Die();
				AwardExperience(instigator);
			}
			else
			{
				takeDamage.Invoke(damage);
			}
		}
		public void Heal(float healthToRestore)
		{
			healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
		}
		public float GetHealthPoints()
		{
			return healthPoints.value;
		}
		public float GetMaxHealthPoints()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Health);
		}
		private void AwardExperience(GameObject instigator)
		{
			Experience experience = instigator.GetComponent<Experience>();
			if (experience == null) return;
			experience.GainExperiencePoint(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		public float GetPercentage()
		{
			return 100 * GetFraction();
		}
		public float GetFraction()
		{
			return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
		}
		private void Die()
		{
			if (isDead) return;

			isDead = true;
			GetComponent<Animator>().SetTrigger("death");
			GetComponent<ActionScheduler>().CancelCurrentAction();

		}
		private void RegenerateHealth()
		{
			float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
			healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
		}
		public object CaptureState()
		{
			return healthPoints.value;
		}
		public void RestoreState(object state)
		{
			healthPoints.value = (float)state;
			if (healthPoints.value <= 0)
			{
				Die();
			}
		}
	}
}