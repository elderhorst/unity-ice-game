using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game {

	public class GameUi : MonoBehaviour {

		[SerializeField] private Button _backButton;
		[SerializeField] private Image _transitionImage;
		[SerializeField] private Level _level;

		protected void Start() {

			StartCoroutine(Actions.ActionManager.fadeTransition(false, _transitionImage, onFinishedEnterTransition));
		}

		protected virtual void onFinishedEnterTransition() {

			_backButton.onClick.AddListener(onClickBackButton);

			_level.handleUiFinishedEnterTransition();
		}

		private void onFinishedExitTransition() {

			SceneManager.LoadScene("MainMenu");
		}

		private void onClickBackButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			StartCoroutine(Actions.ActionManager.fadeTransition(true, _transitionImage, onFinishedExitTransition));
		}
	}
}