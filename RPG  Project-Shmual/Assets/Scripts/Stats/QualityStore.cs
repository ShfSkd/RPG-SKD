using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
	public class QualityStore : MonoBehaviour,IModifierProvider,ISaveable
	{
		[SerializeField] QualityBonus[] bonusConfig;

		[Serializable]
		class QualityBonus
		{
			public Quality quality;
			public Stat stat;
			public float additiveBonusPerPoint = 0;
			public float percentageBonusPerPoint = 0;
		}

		Dictionary<Quality, int> assignedPoints = new Dictionary<Quality, int>();
		Dictionary<Quality, int> stagedPoints = new Dictionary<Quality, int>();

		Dictionary<Stat, Dictionary<Quality, float>> additiveBonusCache;
		Dictionary<Stat, Dictionary<Quality, float>> percentageBonusCache;

		private void Awake()
		{
			additiveBonusCache = new Dictionary<Stat, Dictionary<Quality, float>>();
			percentageBonusCache = new Dictionary<Stat, Dictionary<Quality, float>>();
			foreach (var bonus in bonusConfig)
			{
				if (!additiveBonusCache.ContainsKey(bonus.stat))
				{
					additiveBonusCache[bonus.stat] = new Dictionary<Quality, float>();
				}
				if (!percentageBonusCache.ContainsKey(bonus.stat))
				{
					percentageBonusCache[bonus.stat] = new Dictionary<Quality, float>();
				}
				additiveBonusCache[bonus.stat][bonus.quality] = bonus.additiveBonusPerPoint;
				percentageBonusCache[bonus.stat][bonus.quality] = bonus.percentageBonusPerPoint;
			}
		}

		public int GetProposedPoints(Quality quality)
		{
			return GetPoints(quality) + GetStagedPoints(quality);
		}
		public int GetPoints(Quality quality)
		{
			return assignedPoints.ContainsKey(quality) ? assignedPoints[quality] : 0;
		}
		public int GetStagedPoints(Quality quality)
		{
			return stagedPoints.ContainsKey(quality) ? stagedPoints[quality] : 0;
		}
		public void AssignPoints(Quality quality,int points)
		{
			if (!CanAssignPoints(quality, points)) return;

			stagedPoints[quality] = GetStagedPoints(quality) + points;
		}
		public bool CanAssignPoints(Quality quality, int points)
		{
			if (GetStagedPoints(quality) + points < 0) return false;
			if (GetUnassignedPoints() < points) return false;

			return true;
		}
		public int GetUnassignedPoints()
		{
			return GetAssignablePoints() - GetTotalProposedPoints();
		}

		public int GetTotalProposedPoints()
		{
			int total = 0;
			foreach (int points in assignedPoints.Values)
			{
				total += points;
			}
			foreach (int points in stagedPoints.Values)
			{
				total += points;
			}
			return total;
		}

		public void Commit()
		{
			foreach (Quality quality in stagedPoints.Keys)
			{
				assignedPoints[quality] = GetProposedPoints(quality);
			}
			stagedPoints.Clear();
		}
		public int GetAssignablePoints()
		{
			return (int)GetComponent<BaseStats>().GetStat(Stat.TotalQualityPoints);
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if (!additiveBonusCache.ContainsKey(stat)) yield break;

			foreach (Quality quality in additiveBonusCache[stat].Keys)
			{
				float bonus = additiveBonusCache[stat][quality];
				yield return bonus * GetPoints(quality);
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if (!percentageBonusCache.ContainsKey(stat)) yield break;

			foreach (Quality quality in percentageBonusCache[stat].Keys)
			{
				float bonus = percentageBonusCache[stat][quality];
				yield return bonus * GetPoints(quality);
			}
		}

		public object CaptureState()
		{
			return assignedPoints;
		}

		public void RestoreState(object state)
		{
			assignedPoints = new Dictionary<Quality, int>((IDictionary<Quality, int>)state);
		}
	}
}