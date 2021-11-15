using RPG.Saving;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace RPG.SceneManagment
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defaultSaveFile = "save";

		[SerializeField] float fadeInTime = 0.2f;

		private void Awake()
		{
			StartCoroutine(LoadfLastScene());
		}
		private IEnumerator LoadfLastScene()
		{
			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutImediate();
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
				print("DeleteSaving " + Path.Combine(Application.persistentDataPath, defaultSaveFile + ".sac"));
			}
		}

		private void DeleteSave()
		{
			GetComponent<SavingSystem>().Delete(defaultSaveFile);
		}

		public void Load()
		{
			 StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
		}

		public void Save()
		{
			GetComponent<SavingSystem>().Save(defaultSaveFile);
		}
	}
}