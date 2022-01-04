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
		public UnityEvent onDie;

		[Serializable]
		public class TakeDamageEvent : UnityEvent<float>
		{

		}

		LazyValue<float> healthPoints;

		bool wasDeadLastFrame;
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
			return healthPoints.value <= 0;
		}

		public void TakeDamage(GameObject instigator,float damage)
		{
			print(gameObject.name + " tool damage: " + damage);
			
			healthPoints.value = Mathf.Max(healthPoints.value - damage,0);
			if (IsDead())
			{
				onDie.Invoke();
				AwardExperience(instigator);
			}
			else
			{
				takeDamage.Invoke(damage);
			}
			UpdateState();
		}
		public void Heal(float healthToRestore)
		{
			healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
			UpdateState();
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
			experience.GainExperiencePoints(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
		}

		public float GetPercentage()
		{
			return 100 * GetFraction();
		}
		public float GetFraction()
		{
			return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
		}
		private void UpdateState()
		{
			Animator animator = GetComponent<Animator>();
			if (!wasDeadLastFrame && IsDead())
			{
				animator.SetTrigger("death");
				GetComponent<ActionScheduler>().CancelCurrentAction();
			}
			if (wasDeadLastFrame && !IsDead())
			{
				animator.Rebind();
			}
			wasDeadLastFrame = IsDead();

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

			UpdateState();
		}
	}
}