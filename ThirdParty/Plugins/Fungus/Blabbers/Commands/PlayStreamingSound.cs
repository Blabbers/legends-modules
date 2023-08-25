using Animancer;
using Blabbers.Game00;
using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


[CommandInfo("Blabbers",
			 "Play Stream",
			 "Plays a Sound from the Streaming Assets")]
[AddComponentMenu("")]

public class PlayStreamingSound : Command
{
	//public string key;
	public List<string> keys;

	[SerializeField] protected bool animateCharacterIn = false;
	[SerializeField] protected bool animateCharacterOut = false;
	[SerializeField] protected bool showHud = true;
	[SerializeField] protected bool showHudOnCharacterOut = true;

	[Range(0, 1)]
	[Tooltip("Volume level of the sound effect")]
	[SerializeField] protected float volume = 1;

	[Tooltip("Wait until the sound has finished playing before continuing execution.")]
	[SerializeField] protected bool waitUntilFinished = true;
	[SerializeField] protected bool ignoreTimeScale = true;

	AudioClip _audioClip;
	AudioSource _audioSource;
	float animDuration = 1;
	//string audioFilePath;

	public override void OnEnter()
	{
		if (keys == null) return;

		if (keys.Count > 0)
		{
			//var id = Random.Range(0, keys.Count);
			var id = 0;

			if (keys.Count > 1)
			{
				id = UI_AudioCharacterScreen.Instance.GetNextPlayId();

				if (id >= keys.Count)
				{
					id = 0;
					UI_AudioCharacterScreen.Instance.ResetPlayId();
				}
			}

			//Debug.Log($"PlayStreamingSound.OnEnter({keys[id]})");
			PostRequest(StreamingAssetsManager.Instance.GetClipByKey(keys[id]));
		}


	}

	private IEnumerator LoadAudio(string path, string key)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG); // Create the UnityWebRequest
		yield return www.SendWebRequest(); // Send the request


		if (www.result == UnityWebRequest.Result.Success) // Check if the request was successful
		{
			_audioClip = DownloadHandlerAudioClip.GetContent(www); // Get the audio clip from the loaded data
			_audioClip.name = key; // Set the name of the audio clip

			//_audioSource.clip = _audioClip; // Set the audio clip on the AudioSource component
			PostRequest(_audioClip);
		}
		else
		{
			Debug.LogError("Error loading audio file: " + www.error); // Log an error message if the request failed
			PostRequest(null);
		}

		www.Dispose(); // Clean up the UnityWebRequest object
	}


	void PostRequest(AudioClip clip)
	{
		if (clip == null)
		{
			Debug.LogError($"PlayStreamingSound: Clip not found\nCheck this key: {keys[0]}");
			Continue();
		}
		else
		{

			//Debug.Log($"PlayStreamingSound.PostRequest({clip.name})");

			if (animateCharacterIn)
			{
				//Animate character in
				//UI_Clickblock.Instance.ToggleClickBlock(active);

				UI_AudioCharacterScreen.Instance.AnimateIn(animDuration);
				Wait(animDuration + 0.5f, () => PlayAudio(clip, volume));
			}
			else
			{
				PlayAudio(clip, volume);
			}

		}
	}


	void PlayAudio(AudioClip clip, float volume)
	{
		var clipDuration = clip.length;


		Fungus_StreamingAudioPlayer.Instance.PlayAudio(clip, volume);

		//Set speaking = true
		if (showHud)
		{
			UI_AudioCharacterScreen.Instance.ToggleCharacterSpeaking(true);
		}

		if (waitUntilFinished)
		{

			Wait(clipDuration, () => PostAudioPlay());

			#region MyRegion
			//StartCoroutine(_Wait());
			//IEnumerator _Wait()
			//{
			//	if (ignoreTimeScale)
			//	{
			//		yield return new WaitForSecondsRealtime(duration);
			//		Continue();
			//	}
			//	else
			//	{

			//		var t = duration;
			//		while (t > 0)
			//		{
			//			t -= Time.deltaTime;
			//			yield return null;
			//		}

			//		Continue();
			//	}
			//} 
			#endregion
		}
		else
		{
			PostAudioPlay();

		}
	}

	void PostAudioPlay()
	{

		//Set speaking = false
		if (showHud)
		{
			UI_AudioCharacterScreen.Instance.ToggleCharacterSpeaking(false);
		}

		if (animateCharacterOut)
		{
			//Animate character out
			UI_AudioCharacterScreen.Instance.AnimateOut(animDuration, showHudOnCharacterOut);
			Wait(animDuration + 1.0f, () => Continue());
		}
		else
		{
			Continue();
		}

	}


	void Wait(float duration, Action callback)
	{
		StartCoroutine(_Wait());
		IEnumerator _Wait()
		{
			if (ignoreTimeScale)
			{
				yield return new WaitForSecondsRealtime(duration);
				callback?.Invoke();
			}
			else
			{

				var t = duration;
				while (t > 0)
				{
					t -= Time.deltaTime;
					yield return null;
				}

				callback?.Invoke();
			}
		}
	}


	#region Default
	public override Color GetButtonColor()
	{
		return new Color32(216, 228, 170, 255);
	}

	public override string GetSummary()
	{
		string namePrefix = $"Play Sound:";
		string suffix = "";
		string inPrefix, outPrefix;

		inPrefix = outPrefix = "";

		if (keys == null || keys.Count == 0)
		{
			namePrefix = "<color=red><b>*INSERT ONE OR MORE KEYS TO PLAY SOUND*</b></color>";
			return namePrefix;
		}
		else
		{
			if (string.IsNullOrEmpty(keys[0]))
			{
				namePrefix = "<color=red><b>*INSERT ONE OR MORE KEYS TO PLAY SOUND*</b></color>";
				return namePrefix;
			}
			else
			{
				//namePrefix = $"Play Sound: {keys[0]}";
				suffix = keys[0];

				if (animateCharacterIn) inPrefix = "<color=red><b>(ANIMATE IN)</b></color>";
				if (animateCharacterOut) outPrefix = "<color=red><b>(ANIMATE OUT)</b></color>";

				return $"{inPrefix} {namePrefix} {suffix} {outPrefix}";
			}
		}

	}
	#endregion

}
