using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
	public static class Animate
	{
		public static async Task FadeTransition(bool fadeIn, Image image)
		{
			float start = fadeIn ? 0 : 1;
			float end = fadeIn ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration)
			{
				Color color = image.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				image.color = color;

				currentTime += Time.deltaTime;

				await Task.Yield();
			}
		}

		public static async Task FadeTransition(bool fadeIn, SpriteRenderer sprite)
		{
			float start = fadeIn ? 0 : 1;
			float end = fadeIn ? 1 : 0;

			float currentTime = 0;
			float duration = 0.3f;

			while (currentTime <= duration)
			{
				Color color = sprite.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				sprite.color = color;

				currentTime += Time.deltaTime;

				await Task.Yield();
			}
		}
		
		public static async Task FadeText(bool fadeIn, float duration, MaskableGraphic graphic)
		{
			float start = fadeIn ? 0 : 1;
			float end = fadeIn ? 1 : 0;

			float currentTime = 0;

			while (currentTime <= duration)
			{
				Color color = graphic.color;
				color.a = Mathf.Lerp(start, end, currentTime / duration);

				graphic.color = color;

				currentTime += Time.deltaTime;

				await Task.Yield();
			}
		}

		public static async Task TranslateObject(GameObject obj, Vector3 start, Vector3 end, float duration)
		{
			float currentTime = 0;

			while (currentTime <= duration)
			{
				currentTime += Time.deltaTime;
				
				float t = currentTime / duration;
				float x = Mathf.Lerp(start.x, end.x, Mathf.Lerp(Ease.In(t), Ease.Out(t), t));
				float y = Mathf.Lerp(start.y, end.y, t);
				float z = Mathf.Lerp(start.z, end.z, t);

				obj.transform.localPosition = new Vector3(x, y, z);

				await Task.Yield();
			}
		}
	}
}
