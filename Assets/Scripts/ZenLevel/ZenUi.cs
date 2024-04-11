using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
    public class ZenUi : GameUi
	{
        [SerializeField] private Button _restartButton;
		[SerializeField] private ZenLevel _zenLevel;

        protected override void OnFinishedEnterTransition()
		{
			base.OnFinishedEnterTransition();

            _restartButton.onClick.AddListener(OnClickRestartButton);
		}
        
        private void OnClickRestartButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			_zenLevel.RestartLevel();
		}
    }
}