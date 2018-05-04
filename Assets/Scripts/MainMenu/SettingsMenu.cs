using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu {

	public class SettingsMenu : SubMenu {

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;

        private void Awake() {

            _musicSlider.value = Game.SoundManager.Instance.MusicVolume;
            _effectsSlider.value = Game.SoundManager.Instance.EffectVolume;
        }

        public override void enableButtons() {

            base.enableButtons();

            _musicSlider.onValueChanged.AddListener(onMusicValueChanged);
            _effectsSlider.onValueChanged.AddListener(onEffectsValueChanged);
        }

        public override void disableButtons() {

            base.disableButtons();

            _musicSlider.onValueChanged.RemoveListener(onMusicValueChanged);
            _effectsSlider.onValueChanged.RemoveListener(onEffectsValueChanged);
        }

        private void onMusicValueChanged(float value) {

            Game.SoundManager.Instance.MusicVolume = value;
        }

        private void onEffectsValueChanged(float value) {

            Game.SoundManager.Instance.EffectVolume = value;
        }
    }
}