using GameDevTV.Utils;
using System.Collections;
using UnityEngine;
using RPG.SceneManagment;
using System;
using TMPro;

namespace RPG.UI
{
	public class MainMenuUI : MonoBehaviour
	{
		[SerializeField] TMP_InputField newGameField;

		LazyValue<SavingWrapper> savingWrapper;

		private void Awake()
		{
			savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
		}

		private SavingWrapper GetSavingWrapper()
		{
			return FindObjectOfType<SavingWrapper>();
		}
		public void ContinueGame()
		{
			savingWrapper.value.ContinueGame();
		}
		public void NewGame()
		{
			savingWrapper.value.NewGame(newGameField.text);
		}
		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}