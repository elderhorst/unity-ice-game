using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IceGame
{
	public class GameUi : MonoBehaviour
	{
		[SerializeField] private Button _backButton;
		[SerializeField] private Image _transitionImage;
		[SerializeField] private Level _level;

		protected void Start()
		{
			StartCoroutine(ActionManager.FadeTransition(false, _transitionImage, OnFinishedEnterTransition));
		}

		protected virtual void OnFinishedEnterTransition()
		{
			_backButton.onClick.AddListener(OnClickBackButton);

			_level.HandleUiFinishedEnterTransition();
		}

		private void OnFinishedExitTransition()
		{
			SceneManager.LoadScene("MainMenu");
		}

		private void OnClickBackButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			StartCoroutine(ActionManager.FadeTransition(true, _transitionImage, OnFinishedExitTransition));
		}
	}
}