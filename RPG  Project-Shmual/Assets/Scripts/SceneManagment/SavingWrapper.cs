using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagment
{
	public class SavingWrapper : MonoBehaviour
	{
		//const string defaultSaveFile = "save";
		private const string currentSaveKey = "CurrentSaveName";
		[SerializeField] float fadeInTime = 0.2f;
		[SerializeField] float fadeOutTime = 0.2f;
		[SerializeField] int firstLevelBuildIndex = 1;
		[SerializeField] int menuLevelBuildIndex = 0;

		public void ContinueGame()
		{
			if (!PlayerPrefs.HasKey(currentSaveKey)) return;
			if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;

			StartCoroutine(LoadfLastScene());
		}
		public void NewGame(string saveFile)
		{
			if (string.IsNullOrEmpty(saveFile)) return;
			SetCurrentSave(saveFile);
			StartCoroutine(LoadFirstScene());
		}

		public void LoadGame(string saveFile)
		{
			SetCurrentSave(saveFile);
			ContinueGame();
		}
		public void LoadMenu()
		{
			StartCoroutine(LoadMenuScene());
		}
		private void SetCurrentSave(string saveFile)
		{
			PlayerPrefs.SetString(currentSaveKey, saveFile);
		}
		string GetCurrentSave()
		{
			return PlayerPrefs.GetString(currentSaveKey) ;
		}

		private IEnumerator LoadFirstScene()
		{
			Fader fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
			yield return fader.FadeIn(fadeInTime);
		}
		private IEnumerator LoadMenuScene()
		{
			Fader fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
			yield return fader.FadeIn(fadeInTime);
		}

		private IEnumerator LoadfLastScene()
		{
			Fader fader = FindObjectOfType<Fader>();
			yield return fader.FadeOut(fadeOutTime);
			yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
			yield return fader.FadeIn(fadeInTime);
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				Save();
			}
			if (Input.GetKeyDown(KeyCode.L))
			{
				Load();
			}
			if (Input.GetKeyDown(KeyCode.Delete))
			{
				DeleteSave();
				print("DeleteSaving " + Path.Combine(Application.persistentDataPath, GetCurrentSave() + ".sav"));
			}
		}

		private void DeleteSave()
		{
			GetComponent<SavingSystem>().Delete(GetCurrentSave());
		}

		public void Load()
		{
			GetComponent<SavingSystem>().Load(GetCurrentSave());
		}

		public void Save()
		{
			GetComponent<SavingSystem>().Save(GetCurrentSave());
		}
		public IEnumerable<string> ListSaves()
		{
			return GetComponent<SavingSystem>().ListSaves();
		}
	}
}