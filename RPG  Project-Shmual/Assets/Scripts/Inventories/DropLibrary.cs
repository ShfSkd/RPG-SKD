using GameDevTV.Inventories;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(menuName = ("RPG/Inventory/Drop Library"))]
	public class DropLibrary : ScriptableObject
	{
		[SerializeField] DropConfig[] potentialDrops;
		[SerializeField] float[] dropChangePercentage;
		[SerializeField] int[] minDrops;
		[SerializeField] int[] maxDrops;

		[System.Serializable]
		class DropConfig
		{
			public InventoryItem item;
			public float[] relativeChange;
			public int[] minNumber;
			public int[] maxNumber;
			public int GetRandomNumber(int level)
			{
				if (!item.IsStackable()) return 1;


				int min = GetByLevel(minNumber, level);
				int max = GetByLevel(maxNumber, level);
				return Random.Range(min, max + 1);
			}
		}
		public struct Dropped
		{
			public InventoryItem item;
			public int number;
		}

		public IEnumerable<Dropped> GetRandomDrops(int level)
		{
			if (!ShouldRandomDrop(level))
			{
				yield break;
			}
			for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
			{
				yield return GetRandomDrop(level);
			}
		}

		Dropped GetRandomDrop(int level)
		{
			var drop = SelectedRandomItem(level);
			var result = new Dropped();

			result.item = drop.item;
			result.number = drop.GetRandomNumber(level);

			return result;
		}

		int GetRandomNumberOfDrops(int level)
		{
			int min=GetByLevel(minDrops,level);
			int max=GetByLevel(maxDrops,level);

			return Random.Range(min, max);
		}

		bool ShouldRandomDrop(int level)
		{
			bool dice = Random.Range(0, 100) < GetByLevel(dropChangePercentage, level);

			return dice;
		}
		DropConfig SelectedRandomItem(int level)
		{
			float totalChance = GetTotalChance(level);
			float randomRoll = Random.Range(0, totalChance);
			float chanceTotal = 0;
			foreach (var drop in potentialDrops)
			{
				chanceTotal += GetByLevel(drop.relativeChange, level);
				if (chanceTotal > randomRoll)
				{
					return drop;
				}
			}
			return null;
		}

		float GetTotalChance(int level)
		{
			float total = 0;
			foreach (var drop in potentialDrops)
			{
				total += GetByLevel(drop.relativeChange, level);
			}
			return total;
		}

		static T GetByLevel<T>(T[] values, int level)
		{
			if (values.Length == 0) return default;

			if (level > values.Length) return values[values.Length - 1];

			if (level <= 0) return default;

			return values[level - 1];
		}
	}
}