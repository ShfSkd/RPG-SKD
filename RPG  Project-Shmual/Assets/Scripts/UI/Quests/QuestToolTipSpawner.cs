﻿using GameDevTV.Core.UI.Tooltips;
using RPG.Quests;
using System.Collections;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestToolTipSpawner : TooltipSpawner
	{
		public override bool CanCreateTooltip()
		{
			return true;
		}

		public override void UpdateTooltip(GameObject tooltip)
		{
			QuestStatus status= GetComponent<QuestItemUI>().GetQuestStatus();
			tooltip.GetComponent<QuestTooltipUI>().Setup(status);
		}
	}
}