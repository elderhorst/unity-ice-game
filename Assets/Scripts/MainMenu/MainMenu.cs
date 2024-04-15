using System.Threading.Tasks;
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

			StartEnterTransition();
		}

		private void handleSubMenuClicked(SubMenu subMenu)
		{
			SetSubMenu(subMenu);
			DisableMenuButtons();

			MoveMenus(true, EnableSubMenuButton);
		}

		private void SetSubMenu(SubMenu subMenu)
		{
			_currentSubMenu = subMenu;

			_currentSubMenu.transform.localPosition = _subMenuOffscreen.localPosition;
		}

		private async void MoveMenus(bool isSubMenuNewFocus, System.Action doneCallback)
		{
			Vector3 mainMenuStart = isSubMenuNewFocus ? Vector3.zero : new Vector3(-Screen.width, 0, 0);
			Vector3 mainMenuEnd = isSubMenuNewFocus ? new Vector3(-Screen.width, 0, 0) : Vector3.zero;
			Vector3 subMenuStart = isSubMenuNewFocus ? new Vector3(Screen.width, 0, 0) : Vector3.zero;
			Vector3 subMenuEnd = isSubMenuNewFocus ? Vector3.zero : new Vector3(Screen.width, 0, 0);
			float duration = 0.75f;

			Task animateMainMenu = Animate.TranslateObject(_menu.gameObject, mainMenuStart, mainMenuEnd, duration);
			Task animateSubMenu = Animate.TranslateObject(_currentSubMenu.gameObject, subMenuStart, subMenuEnd, duration);
			
			await Task.WhenAll(animateMainMenu, animateSubMenu);
			
			doneCallback();
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
		
		private async void StartEnterTransition()
		{
			await Animate.FadeTransition(false, _transitionImage);
			
			EnableMenuButtons();
		}

		private async void StartExitTransition()
		{
			await Animate.FadeTransition(true, _transitionImage);
			
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