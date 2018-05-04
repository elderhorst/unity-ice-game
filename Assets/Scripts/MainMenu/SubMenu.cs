using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class SubMenu : MonoBehaviour {

        [SerializeField] protected Button _backButton;

        public delegate void ButtonHandler();
        public event ButtonHandler BackButtonClick;

        public virtual void enableButtons() {

			_backButton.onClick.AddListener(onClickBackButton);
		}

		public virtual void disableButtons() {

			_backButton.onClick.RemoveListener(onClickBackButton);
		}

        protected void onClickBackButton() {

            Game.SoundManager.Instance.playEffect("ButtonClick");

            if (BackButtonClick != null) {

                BackButtonClick();
            }
        }
    }
}