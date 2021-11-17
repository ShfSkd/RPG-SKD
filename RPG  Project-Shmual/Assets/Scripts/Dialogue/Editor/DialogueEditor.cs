using System;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
	public class DialogueEditor : EditorWindow
	{
		Dialogue selectedDialogue = null;
		[NonSerialized] GUIStyle nodeStyle;
		[NonSerialized] GUIStyle playerNodeStyle;
		[NonSerialized] DialogueNode draggingNode = null;
		[NonSerialized] Vector2 draggingOffset;
		[NonSerialized] DialogueNode creatingNode = null;
		[NonSerialized] DialogueNode deletingNode = null;
		[NonSerialized] DialogueNode linkingParentNode = null;
		[NonSerialized] bool isDraggingCanvas;
		[NonSerialized] Vector2 draggingCanvasOffset;
		Vector2 scrollPosition;

		const float canvasSize = 4000;
		const float backgroundSize = 50;
		[MenuItem("Window/Dialogue Editor", priority = 1)]
		public static void ShowEditorWindow()
		{
			GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
		}
		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
			if (dialogue != null)
			{
				ShowEditorWindow();
				return true;
			}
			return false;
		}
		private void OnEnable()
		{
			Selection.selectionChanged += onSelectionChange;
			nodeStyle = new GUIStyle();
			nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
			nodeStyle.normal.textColor = Color.white;
			nodeStyle.padding = new RectOffset(20, 20, 20, 20);
			nodeStyle.border = new RectOffset(12, 12, 12, 12);

			Selection.selectionChanged += onSelectionChange;
			playerNodeStyle = new GUIStyle();
			playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
			playerNodeStyle.normal.textColor = Color.white;
			playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
			playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
		}

		private void onSelectionChange()
		{
			Dialogue newDialogue = Selection.activeObject as Dialogue;
			if (newDialogue != null)
			{
				selectedDialogue = newDialogue;
				Repaint();
			}
		}

		private void OnGUI()
		{
			if (selectedDialogue == null)
			{
				EditorGUILayout.LabelField("No Dialogue Selected");
			}
			else
			{
				ProccesEvents();

				scrollPosition= EditorGUILayout.BeginScrollView(scrollPosition);

				Rect canavs= GUILayoutUtility.GetRect(canvasSize, canvasSize);
				Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
				Rect textureCoordinates = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
				GUI.DrawTextureWithTexCoords(canavs, backgroundTexture, textureCoordinates);

				foreach (DialogueNode node in selectedDialogue.GetAllNodes())
				{
					DrawConnections(node);
				}
				foreach (DialogueNode node in selectedDialogue.GetAllNodes())
				{
					DrawNode(node);
				}

				EditorGUILayout.EndScrollView();

				if (creatingNode != null)
				{		
					selectedDialogue.CreateNode(creatingNode);
					creatingNode = null;
				}
				if (deletingNode != null)
				{
					selectedDialogue.DeleteNode(deletingNode);
					deletingNode = null;
				}
			}
		}


		private void ProccesEvents()
		{
			
			if (Event.current.type == EventType.MouseDown && draggingNode == null)
			{
				draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
				if (draggingNode != null)
				{
					draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
					Selection.activeObject = draggingNode;	
				}
				else
				{
					isDraggingCanvas = true;
					draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
					Selection.activeObject = selectedDialogue;
				}
			}
			else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
			{
				draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

				GUI.changed = true;
			}
			else if (Event.current.type == EventType.MouseDrag && isDraggingCanvas)
			{
				scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

				GUI.changed = true;
			}

			else if (Event.current.type == EventType.MouseUp && draggingNode != null)
			{
				draggingNode = null;
			}
			else if (Event.current.type == EventType.MouseUp && isDraggingCanvas)
			{
				isDraggingCanvas = false;
			}
		}


		private void DrawNode(DialogueNode node)
		{
			GUIStyle style = nodeStyle;
			if (node.IsPlayerSpeaking())
			{
				style = playerNodeStyle;
			}

			GUILayout.BeginArea(node.GetRect(), style);

			node.SetText(EditorGUILayout.TextField(node.GetText()));

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				creatingNode = node;
			}

			DrawLinkButtons(node);

			if (GUILayout.Button("x"))
			{
				deletingNode = node;
			}
			GUILayout.EndHorizontal();

			GUILayout.EndArea();
		}

		private void DrawLinkButtons(DialogueNode node)
		{
			if (linkingParentNode == null)
			{
				if (GUILayout.Button("Link"))
				{
					linkingParentNode = node;
				}
			}
			else if (linkingParentNode == node)
			{
				if (GUILayout.Button("Cancel"))
				{
					linkingParentNode = null;
				}
			}
			else if (linkingParentNode.GetChildren().Contains(node.name))
			{
				if (GUILayout.Button("Unlink"))
				{
					linkingParentNode.RemoveChild(node.name);
					linkingParentNode = null;
				}
			}
			else
			{
				if (GUILayout.Button("Child"))
				{
					
					linkingParentNode.AddChild(node.name);
					linkingParentNode = null;
				}
			}
		}

		private void DrawConnections(DialogueNode node)
		{
			Vector3 startPositon = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
			foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
			{
				Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
				Vector3 controlPointOffset = endPosition - startPositon;
				controlPointOffset.y = 0;
				controlPointOffset.x *= 0.8f;
				Handles.DrawBezier(
					startPositon, endPosition,
					startPositon + controlPointOffset,
					endPosition - controlPointOffset,
					Color.white, null, 4f);
			}
		}

		private DialogueNode GetNodeAtPoint(Vector2 point)
		{
			DialogueNode foundNode = null;
			foreach (DialogueNode node in selectedDialogue.GetAllNodes())
			{
				if (node.GetRect().Contains(point))
					foundNode = node;
			}
			return foundNode;
		}
/*		void ResetNodes()
		{
			draggingNode = null;
			creatingNode = null;
			deletingNode = null;
			linkingParentNode = null;
		}*/
	}
}