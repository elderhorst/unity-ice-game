using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	public class SoundManager : MonoBehaviour {

		private static SoundManager _instance;
		private AudioSource _songSource;
		private List<AudioSource> _effects;

		public static SoundManager Instance {
			get {

				if (_instance == null) {

					GameObject obj = new GameObject();
					_instance = obj.AddComponent<SoundManager>();
				}
 
				return _instance;
			}
		}

		private void Awake() {

			gameObject.name = "SoundManager";
			DontDestroyOnLoad(this);

			_effects = new List<AudioSource>();
		}

		private void Update() {

			for (int i = 0; i < _effects.Count; i++) {

				if (!_effects[i].isPlaying) {

					Destroy(_effects[i]);
					_effects.RemoveAt(i);

					i--;
				}
			}
		}

		public void playSong(string name) {

			AudioClip clip = Resources.Load<AudioClip>("Sounds/Music/" + name);
			
			if (_songSource != null) {

				if (clip == _songSource.clip) {
				
					return;
				}

				_songSource.Stop();
			}
			else {

				_songSource = gameObject.AddComponent<AudioSource>();
			}

			_songSource.clip = clip;
			_songSource.loop = true;
			_songSource.Play();
		}

		public void playEffect(string name) {

			AudioClip clip = Resources.Load<AudioClip>("Sounds/Effects/" + name);
			AudioSource source = gameObject.AddComponent<AudioSource>();

			source.clip = clip;
			source.Play();

			_effects.Add(source);
		}
	}
}
