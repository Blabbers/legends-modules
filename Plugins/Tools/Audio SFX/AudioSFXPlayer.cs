using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace Blabbers.Game00
{
	public class AudioSFXPlayer : MonoBehaviour
	{
		[SerializeField]
		private float delay = 0f;
		[SerializeField]
		private bool playOnEnable = true;
		[SerializeField]
		private bool playSelectedIndex = false;
		[ShowIf("playSelectedIndex")]
		[SerializeField]
		private int selectedIndex;
		[BoxGroup("SFX")]
		public AudioSFX sfxClip;
		void OnEnable()
		{
			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				if (delay > 0f)
				{
					yield return new WaitForSeconds(delay);
				}
				if (playOnEnable)
				{
					if (playSelectedIndex)
					{
						sfxClip.PlaySelectedIndex(selectedIndex);
					}
					else
					{
						sfxClip.Play();
					}
				}
			}
		}
	}
}