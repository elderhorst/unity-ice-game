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
	}
}
