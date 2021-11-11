using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName ="Progression",menuName ="Stats/New Progression",order =0)]
	public class Progression : ScriptableObject
	{
		[SerializeField] ProgressionCharacterClass[] characterClasses = null;

		Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

		public float GetStat(Stat stat, CharacterClass characterClass, int level)
		{
			BuildLookup();
			float[] levels = lookupTable[characterClass][stat];
			if (levels.Length < level) return 0;

			return levels[level - 1];
			//return OldWay(stat, characterClass, level);

		}

		public int GetLevels(Stat stat,CharacterClass characterClass)
		{
			BuildLookup();

			float[] levels = lookupTable[characterClass][stat];
			return levels.Length;
		}
		private void BuildLookup()
		{
			if (lookupTable != null) return;

			lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
			foreach (ProgressionCharacterClass progressionClass in characterClasses)
			{
				var statlookupTable = new Dictionary<Stat, float[]>();

				foreach (ProgressionStat progressionStat in progressionClass.stats)
				{
					statlookupTable[progressionStat.stat] = progressionStat.levels;
				}

				lookupTable[progressionClass.characterClass] = statlookupTable;
			}

		}

		[System.Serializable]
		class ProgressionCharacterClass
		{
			public CharacterClass characterClass;
			public ProgressionStat[] stats;
		}
		
	}
	[System.Serializable]
	public class ProgressionStat
	{
		public Stat stat;
		public float[] levels;
	}
	/*private float OldWay(Stat stat, CharacterClass characterClass, int level)
		{
			foreach (ProgressionCharacterClass progressionClass in characterClasses)
			{
				if (progressionClass.characterClass != characterClass) continue;

				foreach (ProgressionStat progressionStat in progressionClass.stats)
				{
					if (progressionStat.stat != stat) continue;

					if (progressionStat.levels.Length < level) continue;

					return progressionStat.levels[level - 1];
				}


			}
			return 0;
		}*/
}