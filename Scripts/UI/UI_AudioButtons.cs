using UnityEngine;
using UnityEngine.UI;

namespace Blabbers.Game00
{
	public class UI_AudioButtons : MonoBehaviour
	{
		public Button muteButton, unmuteButton;

		private void Start()
		{
			muteButton.gameObject.SetActive(Singleton.Get<AudioController>().musicSource.mute);
			unmuteButton.gameObject.SetActive(!Singleton.Get<AudioController>().musicSource.mute);

			muteButton.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().Mute(false); });
			unmuteButton.onClick.AddListenerOnce(() => { Singleton.Get<AudioController>().Mute(true); });
		}
	}
}