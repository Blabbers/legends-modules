using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using BeauRoutine;

namespace Blabbers.Game00
{
	public class AudioSFXPlayer : MonoBehaviour
	{
		[SerializeField]
		private float delay = 0f;
		[SerializeField]
		private bool playOnEnable = true;
		[SerializeField]
		private bool playOnDisable = false;
		[SerializeField]
		private bool playSelectedIndex = false;
		[ShowIf("playSelectedIndex")]
		[SerializeField]
		private int selectedIndex;
		[BoxGroup("SFX")]
		public AudioSFX sfxClip;
		void OnEnable()
		{
			if (playOnEnable)
			{
				Play();
			}
		}
		void OnDisable()
		{
			if (playOnDisable)
			{
				Play();
			}
		}

		public void Play()
		{
			Routine.Start(Run());
			IEnumerator Run()
			{
				if (delay > 0f)
				{
					yield return new WaitForSeconds(delay);
				}
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