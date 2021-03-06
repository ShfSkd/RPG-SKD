using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace GameDevTV.Inventories.Editor
{
	public class InventoryItemEditor : EditorWindow
	{
		private InventoryItem selected;

		[MenuItem("Window/InventoryItem Editor")]
		public static void ShowEditorWindow()
		{
			GetWindow(typeof(InventoryItemEditor), false, "InventoryItem");
		}
		public static void ShowEditorWIndow(InventoryItem candidate)
		{
			InventoryItemEditor window = GetWindow(typeof(InventoryItemEditor), false, "InventoeyItem") as InventoryItemEditor;

			if (candidate)
				window.OnSelectionChange();
		}


		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceID,int line)
		{
			InventoryItem candidate = EditorUtility.InstanceIDToObject(instanceID) as InventoryItem;
			if (candidate != null)
			{
				ShowEditorWIndow(candidate);
				return true;
			}
			return false;
		}
		private void OnSelectionChange()
		{
			var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as InventoryItem;
			if (candidate == null) return;
			selected = candidate;
			Repaint();
		}
		private void OnGUI()
		{
			if (!selected)
			{
				EditorGUILayout.HelpBox("No Dialogue Selected", MessageType.Error);
				return;
			}
			EditorGUILayout.HelpBox($"{selected.name}/{selected.GetDisplayName()}", MessageType.Info);
		}
	}
}
