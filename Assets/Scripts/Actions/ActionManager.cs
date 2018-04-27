using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Actions {

	public static class ActionManager {

		public static IEnumerator fadeTransition(bool fadeIn, Image image, System.Action doneCallback) {

			float start = (fadeIn) ? 0 : 1;
			float end = (fadeIn) ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration) {

				Color color = image.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				image.color = color;

				currentTime += Time.deltaTime;

				yield return null;
			}

			doneCallback();
		}

		public static IEnumerator fadeTransition(bool fadeIn, SpriteRenderer sprite, System.Action doneCallback) {

			float start = (fadeIn) ? 0 : 1;
			float end = (fadeIn) ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration) {

				Color color = sprite.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				sprite.color = color;

				currentTime += Time.deltaTime;

				yield return null;
			}

			doneCallback();
		}

		public static IEnumerator translateObject(GameObject obj, Vector3 start, Vector3 end, float duration, System.Action doneCallback = null) {

			float currentTime = 0;

			while (currentTime <= duration) {

				float x = Mathf.Lerp(start.x, end.x, currentTime / duration);
				float y = Mathf.Lerp(start.y, end.y, currentTime / duration);
				float z = Mathf.Lerp(start.z, end.z, currentTime / duration);

				obj.transform.localPosition = new Vector3(x, y, z);

				currentTime += Time.deltaTime;

				yield return null;
			}

			if (doneCallback != null) {

				doneCallback();
			}
		}
	}
}
