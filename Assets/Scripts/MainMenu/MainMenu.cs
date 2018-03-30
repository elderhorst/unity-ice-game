using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class MainMenu : MonoBehaviour {

		[SerializeField] private Button _storyButton;
		[SerializeField] private Button _zenButton;

		private void Start() {

			_storyButton.onClick.AddListener(onClickStoryButton);
			_zenButton.onClick.AddListener(onClickZenButton);
		}

		private void onClickStoryButton() {

			SceneManager.LoadScene("Level");
		}

		private void onClickZenButton() {

			SceneManager.LoadScene("Zen");
		}
	}
}