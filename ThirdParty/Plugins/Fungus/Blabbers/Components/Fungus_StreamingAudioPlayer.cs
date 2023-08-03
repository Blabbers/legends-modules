
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungus_StreamingAudioPlayer : MonoBehaviour
{

	[SerializeField] AudioSource audioSource;


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}


	#region Instance
	private static Fungus_StreamingAudioPlayer _instance = null;
	public static Fungus_StreamingAudioPlayer Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<Fungus_StreamingAudioPlayer>("Others/Fungus_StreamAudio"));
				_instance.gameObject.name = "Fungus_StreamAudio";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion


	public void PlayAudio(AudioClip clip, float volume)
	{
		audioSource.clip = clip;
		audioSource.volume = volume;

		audioSource.Play();
	}

}

