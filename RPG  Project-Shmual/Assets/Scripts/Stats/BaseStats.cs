using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
		[SerializeField] GameObject levelUpParticalEffectPrefab = null;
		[SerializeField] bool shouldUseModifiers = false;

		public event Action onLevelup;
		LazyValue<int> currentLevel;

		Experience experience;
		private void Awake()
		{
			experience = GetComponent<Experience>();
			currentLevel = new LazyValue<int>(CalculateLevel);
		}
		private void Start()
		{
			currentLevel.ForceInit();
		}
		private void OnEnable()
		{
			if (experience != null)
			{
				experience.onExperienceGained += UpdateLevel;
			}
		}
		private void OnDisable()
		{
			if (experience != null)
			{
				experience.onExperienceGained -= UpdateLevel;
			}
		}
		private void UpdateLevel()
		{
			int newLevel = CalculateLevel();
			if (newLevel > currentLevel.value)
			{
				currentLevel.value = newLevel;
				if (levelUpParticalEffectPrefab != null)
				{
					LevelUpEffect();
				}
				onLevelup();
			}
		}

		private void LevelUpEffect()
		{
			Instantiate(levelUpParticalEffectPrefab, transform); 
		}

		public int GetLevel()
		{
			return currentLevel.value;
		}
		public float GetStat(Stat stat)
		{
			return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModofier(stat) / 100);
		}

		private float GetBaseStat(Stat stat)
		{
			return progression.GetStat(stat, characterClass, GetLevel());
		}

		private float GetAdditiveModifier(Stat stat)
		{
			if (!shouldUseModifiers) return 0;

			float total = 0;
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach (float modifier in provider.GetAdditiveModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}
		private float GetPercentageModofier(Stat stat)
		{
			if (!shouldUseModifiers) return 0;

			float total = 0;
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach (int modifier in provider.GetPercentageModifiers(stat))
				{
					total += modifier;
				}

			}
			return total;
		}


		int CalculateLevel()
		{
			Experience experience = GetComponent<Experience>();
			if (experience == null) return startingLevel;


			float currentXP= experience.GetPoints();
			int penultimateLevel = progression.GetLevels(Stat.ExpereinceToLevelUp, characterClass);
			for (int level = 1; level <= penultimateLevel; level++)
			{
                float xpToLevelUp= progression.GetStat(Stat.ExpereinceToLevelUp, characterClass, level);
				if (xpToLevelUp > currentXP)
				{
                    return level;
				}
			}

            return penultimateLevel + 1;
		}
      
    }
}
