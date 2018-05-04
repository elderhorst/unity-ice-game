using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class MainMenu : MonoBehaviour {

		[SerializeField] private Button _storyButton;
		[SerializeField] private Button _zenButton;
		[SerializeField] private Button _instructionsButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _creditsButton;

		[SerializeField] private Image _transitionImage;

		[SerializeField] private Transform _menu;
		[SerializeField] private Transform _subMenuOffscreen;
		[SerializeField] private SubMenu _instructions;
		[SerializeField] private SubMenu _settings;
		[SerializeField] private SubMenu _credits;

		private SubMenu _currentSubMenu;

		private string _sceneToLoad;

		private void Start() {

			Game.SoundManager.Instance.playMusic("Overworld");

			StartCoroutine(Actions.ActionManager.fadeTransition(false, _transitionImage, enableMenuButtons));
		}

		private void handleSubMenuClicked(SubMenu subMenu) {

			setSubMenu(subMenu);
			disableMenuButtons();

			Vector3 delta = _menu.localPosition - _currentSubMenu.transform.localPosition;

			moveMenus(delta, enableSubMenuButton);
		}

		private void setSubMenu(SubMenu subMenu) {

			_currentSubMenu = subMenu;

			_currentSubMenu.transform.localPosition = _subMenuOffscreen.localPosition;
		}

		private void moveMenus(Vector3 delta, System.Action doneCallback) {

			Vector3 subStart = _currentSubMenu.transform.localPosition;
			Vector3 subEnd = _currentSubMenu.transform.localPosition + delta;
			float duration = 1.25f;

			StartCoroutine(Actions.ActionManager.translateObject(_menu.gameObject, _menu.localPosition, _menu.localPosition + delta, duration));
			StartCoroutine(Actions.ActionManager.translateObject(_currentSubMenu.gameObject, subStart, subEnd, duration, doneCallback));
		}

		private void enableMenuButtons() {

			_storyButton.onClick.AddListener(onClickStoryButton);
			_zenButton.onClick.AddListener(onClickZenButton);
			_instructionsButton.onClick.AddListener(onClickInstructionsButton);
			_settingsButton.onClick.AddListener(onClickSettingsButton);
			_creditsButton.onClick.AddListener(onClickCreditsButton);
		}

		private void disableMenuButtons() {

			_storyButton.onClick.RemoveListener(onClickStoryButton);
			_zenButton.onClick.RemoveListener(onClickZenButton);
			_instructionsButton.onClick.RemoveListener(onClickInstructionsButton);
			_settingsButton.onClick.RemoveListener(onClickSettingsButton);
			_creditsButton.onClick.RemoveListener(onClickCreditsButton);
		}

		private void enableSubMenuButton() {

			_currentSubMenu.enableButtons();

			_currentSubMenu.BackButtonClick += onClickBackButton;
		}

		private void disableSubMenuButton() {

			_currentSubMenu.disableButtons();

			_currentSubMenu.BackButtonClick -= onClickBackButton;
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

		private void onClickInstructionsButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			handleSubMenuClicked(_instructions);
		}

		private void onClickSettingsButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			handleSubMenuClicked(_settings);
		}

		private void onClickCreditsButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			handleSubMenuClicked(_credits);
		}

		private void onClickBackButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			Vector3 delta = _currentSubMenu.transform.localPosition - _menu.localPosition;
			
			disableSubMenuButton();
			moveMenus(delta, enableMenuButtons);
		}
	}
}