using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class MainMenu : MonoBehaviour {

		[SerializeField] private Button _storyButton;
		[SerializeField] private Button _zenButton;
		[SerializeField] private Button _exitButton;
		[SerializeField] private Image _transitionImage;

		private string _sceneToLoad;

		private void Start() {

			Game.SoundManager.Instance.playSong("Overworld");

			StartCoroutine(Actions.ActionManager.fadeTransition(false, _transitionImage, onFinishedEnterTransition));
		}

		private void onFinishedEnterTransition() {

			_storyButton.onClick.AddListener(onClickStoryButton);
			_zenButton.onClick.AddListener(onClickZenButton);
			_exitButton.onClick.AddListener(onClickExitButton);
		}

		private void startExitTransition() {

			StartCoroutine(Actions.ActionManager.fadeTransition(true, _transitionImage, onExitTransitionEnd));
		}

		private void onExitTransitionEnd() {

			SceneManager.LoadScene(_sceneToLoad);
		}

		private void onClickStoryButton() {

			_sceneToLoad = "Level";

			startExitTransition();
		}

		private void onClickZenButton() {

			_sceneToLoad = "Zen";

			startExitTransition();
		}

		private void onClickExitButton() {

			Application.Quit();
		}
	}
}