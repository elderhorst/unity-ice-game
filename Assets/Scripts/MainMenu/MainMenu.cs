using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class MainMenu : MonoBehaviour {

		[SerializeField] private Button _storyButton;
		[SerializeField] private Button _zenButton;
		[SerializeField] private Button _creditsButton;
		[SerializeField] private Button _backButton;
		[SerializeField] private Image _transitionImage;
		[SerializeField] private Transform _menu;
		[SerializeField] private Transform _credits;

		private string _sceneToLoad;

		private void Start() {

			Game.SoundManager.Instance.playSong("Overworld");

			StartCoroutine(Actions.ActionManager.fadeTransition(false, _transitionImage, enableMenuButtons));
		}

		private void moveMenus(Vector3 delta, System.Action doneCallback) {

			float duration = 1.25f;

			StartCoroutine(Actions.ActionManager.translateObject(_menu.gameObject, _menu.localPosition, _menu.localPosition + delta, duration));
			StartCoroutine(Actions.ActionManager.translateObject(_credits.gameObject, _credits.localPosition, _credits.localPosition + delta, duration, doneCallback));
		}

		private void enableMenuButtons() {

			_storyButton.onClick.AddListener(onClickStoryButton);
			_zenButton.onClick.AddListener(onClickZenButton);
			_creditsButton.onClick.AddListener(onClickCreditsButton);
		}

		private void disableMenuButtons() {

			_storyButton.onClick.RemoveListener(onClickStoryButton);
			_zenButton.onClick.RemoveListener(onClickZenButton);
			_creditsButton.onClick.RemoveListener(onClickCreditsButton);
		}

		private void enableCreditsButton() {

			_backButton.onClick.AddListener(onClickBackButton);
		}

		private void disableCreditsButton() {

			_backButton.onClick.RemoveListener(onClickBackButton);
		}

		private void startExitTransition() {

			StartCoroutine(Actions.ActionManager.fadeTransition(true, _transitionImage, onExitTransitionEnd));
		}

		private void onExitTransitionEnd() {

			SceneManager.LoadScene(_sceneToLoad);
		}

		private void onClickStoryButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			_sceneToLoad = "Level";

			startExitTransition();
		}

		private void onClickZenButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			_sceneToLoad = "Zen";

			startExitTransition();
		}

		private void onClickCreditsButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			Vector3 delta = _menu.localPosition - _credits.localPosition;
			
			disableMenuButtons();
			moveMenus(delta, enableCreditsButton);
		}

		private void onClickBackButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			Vector3 delta = _credits.localPosition - _menu.localPosition;
			
			disableCreditsButton();
			moveMenus(delta, enableMenuButtons);
		}
	}
}