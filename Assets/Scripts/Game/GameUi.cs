using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game {

	public class GameUi : MonoBehaviour {

		[SerializeField] private Button _backButton;

		private void Start() {

			_backButton.onClick.AddListener(onClickBackButton);
		}

		private void onClickBackButton() {

			SceneManager.LoadScene("MainMenu");
		}
	}
}