using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game {

	public class GameUi : MonoBehaviour {

		[SerializeField] private Button _backButton;
		[SerializeField] private Image _transitionImage;
		[SerializeField] private Level _level;

		private void Start() {

			StartCoroutine(Actions.ActionManager.fadeTransition(false, _transitionImage, onFinishedEnterTransition));
		}

		private void onFinishedEnterTransition() {

			_backButton.onClick.AddListener(onClickBackButton);

			_level.handleUiFinishedEnterTransition();
		}

		private void onFinishedExitTransition() {

			SceneManager.LoadScene("MainMenu");
		}

		private void onClickBackButton() {

			StartCoroutine(Actions.ActionManager.fadeTransition(true, _transitionImage, onFinishedExitTransition));
		}
	}
}