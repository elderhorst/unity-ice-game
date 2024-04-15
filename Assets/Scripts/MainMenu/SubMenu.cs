using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
	public class SubMenu : MonoBehaviour
	{
        [SerializeField] protected Button _backButton;

        public delegate void ButtonHandler();
        public event ButtonHandler BackButtonClick;

        public virtual void EnableButtons()
		{
			_backButton.onClick.AddListener(OnClickBackButton);
		}

		public virtual void DisableButtons()
		{
			_backButton.onClick.RemoveListener(OnClickBackButton);
		}

        protected void OnClickBackButton()
		{
            SoundManager.Instance.PlayEffect(StringKey.ButtonClickEffect);

            if (BackButtonClick != null)
			{
                BackButtonClick();
            }
        }
    }
}