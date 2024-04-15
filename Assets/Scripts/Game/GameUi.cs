using System;
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
			FadeUi(false, OnFinishedEnterTransition);
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

			FadeUi(true, OnFinishedExitTransition);
		}
		
		private async void FadeUi(bool fadeIn, Action onFinishedTransition)
		{
			await Animate.FadeTransition(fadeIn, _transitionImage);
			
			onFinishedTransition();
		}
	}
}