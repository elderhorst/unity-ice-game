using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	public class SoundManager : MonoBehaviour {

		private static SoundManager _instance;
		private AudioSource _songSource;

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
	}
}
