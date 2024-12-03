using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Myeongjin
{
	public class CUIManager : Singleton<CUIManager>
	{
		[SerializeField] Image image;

		public IEnumerator FadeIn(float fixedTime, Action StartMethod = null, Action EndMethod = null)
		{
			float time = 0;

            StartMethod?.Invoke();

            while (time < fixedTime)
			{
				time += Time.deltaTime;
				Color color = image.color;
				color.a = time / fixedTime;
				image.color = color;
				yield return null;
			}

            EndMethod?.Invoke();
        }
		public IEnumerator FadeOut(float fixedTime, Action StartMethod = null, Action EndMethod = null)
		{
			float time = fixedTime;

            StartMethod?.Invoke();

            while (time > 0)
			{
				time -= Time.deltaTime;
				Color color = image.color;
				color.a = time / fixedTime;
				image.color = color;
				yield return null;
			}

			EndMethod?.Invoke();
        }
	}
}