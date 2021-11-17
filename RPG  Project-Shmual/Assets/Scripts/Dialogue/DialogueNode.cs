using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
	public class DialogueNode : ScriptableObject
	{
		[SerializeField] bool isPlayerSpeaking;
		[SerializeField] string text;
		[SerializeField] List<string> childern = new List<string>();
		[SerializeField] Rect rect = new Rect(0, 0, 200, 100);

		public string GetText()
		{
			return text;
		}
		public List<string> GetChildren()
		{
			return childern;
		}
		public Rect GetRect()
		{
			return rect;
		}
		public bool IsPlayerSpeaking()
		{
			return isPlayerSpeaking;
		}
#if UNITY_EDITOR
		public void SetPosition(Vector2 newPosition)
		{
			Undo.RecordObject(this, "Move Dialogue");
			rect.position = newPosition;
			EditorUtility.SetDirty(this);
		}
		public void SetText(string newText)
		{
			if (newText != text)
			{
				Undo.RecordObject(this, "Update Dialogue Text");
				text = newText;
				EditorUtility.SetDirty(this);
			}

		}
		public void AddChild(string childID)
		{
			Undo.RecordObject(this, "Add Dialogue Link");
			childern.Add(childID);
			EditorUtility.SetDirty(this);
		}
		public void RemoveChild(string childID)
		{
			Undo.RecordObject(this, "Remopve Dialogue Link");
			childern.Remove(childID);
			EditorUtility.SetDirty(this);
		}

		public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
		{
			Undo.RecordObject(this, "Change Dialogue Speaker");
			isPlayerSpeaking = newIsPlayerSpeaking;
			EditorUtility.SetDirty(this);
		}

#endif
	}
}