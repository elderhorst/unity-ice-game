using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class SubMenu : MonoBehaviour {

        [SerializeField] private Button _backButton;

        public delegate void ButtonHandler();
        public event ButtonHandler BackButtonClick;

        public void enableButtons() {

			_backButton.onClick.AddListener(onClickBackButton);
		}

		public void disableButtons() {

			_backButton.onClick.RemoveListener(onClickBackButton);
		}

        private void onClickBackButton() {

            Game.SoundManager.Instance.playEffect("ButtonClick");

            if (BackButtonClick != null) {

                BackButtonClick();
            }
        }
    }
}