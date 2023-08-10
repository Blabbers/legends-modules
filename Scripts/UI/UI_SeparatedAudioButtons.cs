using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_SeparatedAudioButtons : MonoBehaviour
	{
		public Button muteButton_SFX, unmuteButton_SFX;
		public Button muteButton_Music, unmuteButton_Music;

		private void Start()
		{
			muteButton_SFX.gameObject.SetActive(Singleton.Get<AudioController>().gameplaySource.mute);
			unmuteButton_SFX.gameObject.SetActive(!Singleton.Get<AudioController>().gameplaySource.mute);

			muteButton_SFX.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().MuteSFX(false); });
			unmuteButton_SFX.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().MuteSFX(true); });


			muteButton_Music.gameObject.SetActive(Singleton.Get<AudioController>().musicSource.mute);
			unmuteButton_Music.gameObject.SetActive(!Singleton.Get<AudioController>().musicSource.mute);

			muteButton_Music.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().MuteMusic(false); });
			unmuteButton_Music.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().MuteMusic(true); });
		}
	}
}