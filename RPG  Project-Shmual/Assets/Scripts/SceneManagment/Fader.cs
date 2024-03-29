﻿using System;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagment
{
	public class Fader : MonoBehaviour
	{
		CanvasGroup canvasGroup;
		Coroutine currentActiveFade = null;
		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}
		public void FadeOutImediate()
		{
			canvasGroup.alpha = 1;
		}
		public Coroutine FadeOut(float time)
		{
			return Fade(1,time);
		}
		public Coroutine FadeIn(float time)
		{
			 return Fade(0, time);
		}
		public Coroutine Fade(float target,float time)
		{
			if (currentActiveFade != null)
			{
				StopCoroutine(currentActiveFade);
			}
			currentActiveFade = StartCoroutine(FadeRoutine(target,time));
			return currentActiveFade;
		}

		internal void FadeOut(object fadeOutTime)
		{
			throw new NotImplementedException();
		}

		IEnumerator FadeRoutine(float target, float time)
		{
			while (!Mathf.Approximately(canvasGroup.alpha , target))
			{
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target, (Time.unscaledDeltaTime / time));

				yield return null;
			}
		}

	}
}