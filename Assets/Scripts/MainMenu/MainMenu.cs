using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IceGame
{
	public class MainMenu : MonoBehaviour
	{
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

		private void Start()
		{
			SoundManager.Instance.PlayMusic("Overworld");

			StartCoroutine(ActionManager.FadeTransition(false, _transitionImage, EnableMenuButtons));
		}

		private void handleSubMenuClicked(SubMenu subMenu)
		{
			setSubMenu(subMenu);
			DisableMenuButtons();

			MoveMenus(true, EnableSubMenuButton);
		}

		private void setSubMenu(SubMenu subMenu)
		{
			_currentSubMenu = subMenu;

			_currentSubMenu.transform.localPosition = _subMenuOffscreen.localPosition;
		}

		private void MoveMenus(bool isSubMenuNewFocus, System.Action doneCallback)
		{
			Vector3 mainMenuStart = isSubMenuNewFocus ? Vector3.zero : new Vector3(-Screen.width, 0, 0);
			Vector3 mainMenuEnd = isSubMenuNewFocus ? new Vector3(-Screen.width, 0, 0) : Vector3.zero;
			Vector3 subMenuStart = isSubMenuNewFocus ? new Vector3(Screen.width, 0, 0) : Vector3.zero;
			Vector3 subMenuEnd = isSubMenuNewFocus ? Vector3.zero : new Vector3(Screen.width, 0, 0);
			float duration = 0.75f;

			StartCoroutine(ActionManager.TranslateObject(_menu.gameObject, mainMenuStart, mainMenuEnd, duration));
			StartCoroutine(ActionManager.TranslateObject(_currentSubMenu.gameObject, subMenuStart, subMenuEnd, duration, doneCallback));
		}

		private void EnableMenuButtons()
		{
			_storyButton.onClick.AddListener(OnClickStoryButton);
			_zenButton.onClick.AddListener(OnClickZenButton);
			_instructionsButton.onClick.AddListener(OnClickInstructionsButton);
			_settingsButton.onClick.AddListener(OnClickSettingsButton);
			_creditsButton.onClick.AddListener(OnClickCreditsButton);
		}

		private void DisableMenuButtons()
		{
			_storyButton.onClick.RemoveListener(OnClickStoryButton);
			_zenButton.onClick.RemoveListener(OnClickZenButton);
			_instructionsButton.onClick.RemoveListener(OnClickInstructionsButton);
			_settingsButton.onClick.RemoveListener(OnClickSettingsButton);
			_creditsButton.onClick.RemoveListener(OnClickCreditsButton);
		}

		private void EnableSubMenuButton()
		{
			_currentSubMenu.EnableButtons();

			_currentSubMenu.BackButtonClick += onClickBackButton;
		}

		private void DisableSubMenuButton()
		{
			_currentSubMenu.DisableButtons();

			_currentSubMenu.BackButtonClick -= onClickBackButton;
		}

		private void StartExitTransition()
		{
			StartCoroutine(ActionManager.FadeTransition(true, _transitionImage, OnExitTransitionEnd));
		}

		private void OnExitTransitionEnd()
		{
			SceneManager.LoadScene(_sceneToLoad);
		}

		private void OnClickStoryButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			_sceneToLoad = "Level";

			StartExitTransition();
		}

		private void OnClickZenButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			_sceneToLoad = "Zen";

			StartExitTransition();
		}

		private void OnClickInstructionsButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			handleSubMenuClicked(_instructions);
		}

		private void OnClickSettingsButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			handleSubMenuClicked(_settings);
		}

		private void OnClickCreditsButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			handleSubMenuClicked(_credits);
		}

		private void onClickBackButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			DisableSubMenuButton();
			MoveMenus(false, EnableMenuButtons);
		}
	}
}