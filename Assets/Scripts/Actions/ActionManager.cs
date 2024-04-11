using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
	public static class ActionManager
	{
		public static IEnumerator FadeTransition(bool fadeIn, Image image, System.Action doneCallback)
		{
			float start = (fadeIn) ? 0 : 1;
			float end = (fadeIn) ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration)
			{
				Color color = image.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				image.color = color;

				currentTime += Time.deltaTime;

				yield return null;
			}

			doneCallback();
		}

		public static IEnumerator FadeTransition(bool fadeIn, SpriteRenderer sprite, System.Action doneCallback)
		{
			float start = (fadeIn) ? 0 : 1;
			float end = (fadeIn) ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration)
			{
				Color color = sprite.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				sprite.color = color;

				currentTime += Time.deltaTime;

				yield return null;
			}

			doneCallback();
		}

		public static IEnumerator TranslateObject(GameObject obj, Vector3 start, Vector3 end, float duration, System.Action doneCallback = null)
		{
			float currentTime = 0;

			while (currentTime <= duration)
			{

				currentTime += Time.deltaTime;
				
				float t = currentTime / duration;
				float x = Mathf.Lerp(start.x, end.x, Mathf.Lerp(EaseIn(t), EaseOut(t), t));
				float y = Mathf.Lerp(start.y, end.y, t);
				float z = Mathf.Lerp(start.z, end.z, t);

				obj.transform.localPosition = new Vector3(x, y, z);

				yield return null;
			}

			if (doneCallback != null)
			{
				doneCallback();
			}
		}
		
		private static float EaseIn(float t)
		{
			return t * t;
		}
		
		private static float EaseOut(float t)
		{
			return (1 - ((1 - t) * (1 - t)));
		}
	}
}
