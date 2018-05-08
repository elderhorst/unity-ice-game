using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Zen {

    public class ZenUi : Game.GameUi {

        [SerializeField] private Button _restartButton;
		[SerializeField] private ZenLevel _zenLevel;

        protected override void onFinishedEnterTransition() {

			base.onFinishedEnterTransition();

            _restartButton.onClick.AddListener(onClickRestartButton);
		}
        
        private void onClickRestartButton() {

			Game.SoundManager.Instance.playEffect("ButtonClick");

			_zenLevel.restartLevel();
		}
    }
}