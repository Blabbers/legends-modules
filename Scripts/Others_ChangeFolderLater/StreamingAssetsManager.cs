using Animancer;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using Blabbers.Game00;

public class StreamingAssetsManager : MonoBehaviour
{
	#region Init

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}

	#endregion


	[SerializeField] string langCode;
	[SerializeField] int languageId;
	//[SerializeField] string folderPath;

	[SerializeField][ReorderableList] List<LinkBlocks> linkBlocks = new List<LinkBlocks>();
	[SerializeField] List<string> audioFileNames = new List<string>();
	//[SerializeField] string[] audioFileNames;
	[SerializeField] List<string> audioLoadKeys = new List<string>();

	//[SerializeField] List<AudioClip> audioClips = new List<AudioClip>();
	[SerializeField] AudioClip[] audioClips;

	Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

	#region Awake
	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this.gameObject);
			return;
		}


		//GetLanguageId
		//GetAudioClips();
	}


	private void Start()
	{
		GetAudioClips();
	}

	void GetAudioClips()
	{

		langCode = LocalizationExtensions.LocalizeText("langCode");

		if (langCode == "en") languageId = 0;
		else languageId = 1;


		Debug.Log($"StreamingAssetsManager.GetAudioClips()\nLangCode: {langCode} | languageId: {languageId}");

		string basePath = Application.streamingAssetsPath + "/Audio/EN/";

		if(languageId == 0)
		{
			basePath = Application.streamingAssetsPath + "/Audio/EN/";
		}
		else
		{
			basePath = Application.streamingAssetsPath + "/Audio/ES/";
		}

		string fullPath;

		audioFileNames = linkBlocks[languageId].links;
		//audioFileNames = linkBlocks[languageId].links.ToArray();
		audioClips = new AudioClip[audioFileNames.Count];

		//string fullPath = basePath + key + ".mp3"; //or ES for spanish, grab the language code needed from the start game payload.
		//string fullPath = Application.streamingAssetsPath + "/Audio/EN/" + key + ".mp3"; //or ES for spanish, grab the language code needed from the start game payload.


		for (int i = 0; i < audioFileNames.Count; i++)
		{
			fullPath = basePath + audioFileNames[i] + ".mp3";
			//keyValuePairs.Add(audioFileNames[i], i);
			keyValuePairs.Add(audioLoadKeys[i], i);

			StartCoroutine(LoadAudio(fullPath, audioFileNames[i], i));
		}

		
	}



	private IEnumerator LoadAudio(string path, string key, int id)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG); // Create the UnityWebRequest
		yield return www.SendWebRequest(); // Send the request


		if (www.result == UnityWebRequest.Result.Success) // Check if the request was successful
		{
			var _audioClip = DownloadHandlerAudioClip.GetContent(www); // Get the audio clip from the loaded data
			_audioClip.name = key; // Set the name of the audio clip

			//_audioSource.clip = _audioClip; // Set the audio clip on the AudioSource component
			PostRequest(_audioClip, id);
		}
		else
		{
			Debug.LogError("Error loading audio file: " + www.error); // Log an error message if the request failed
			PostRequest(null, 0);
		}

		www.Dispose(); // Clean up the UnityWebRequest object
	}

	void PostRequest(AudioClip clip, int id)
	{
		if(clip!=null)
		{
			audioClips[id]= clip;
		}
	}


	#endregion


	#region Instance
	private static StreamingAssetsManager _instance = null;
	public static StreamingAssetsManager Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<StreamingAssetsManager>("StreamingAssets_Manager"));
				_instance.gameObject.name = "--StreamingAssets_Manager";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion

	//Editor
	public void SetUpDictionaries(string langCode,int id, List<string> fileLoadNames, List<string> loadKeys)
	{
		//audioFileNames = fileLoadNames;
		audioLoadKeys = loadKeys;

		if(id == 0)
		{
			linkBlocks.Clear();
		}

		linkBlocks.Add(new LinkBlocks(langCode, fileLoadNames));
	}



	public AudioClip GetClipByKey(string key)
	{
		int id = 0;

		if (keyValuePairs.TryGetValue(key, out int value))
		{
			id = value;
		}
		else
		{
			return null;
		}
		return audioClips[id];
	}
}


[Serializable]
public class LinkBlocks
{
	public string langCode;
	[ReorderableList] public List<string> links = new List<string>();

	public LinkBlocks(string langCode)
	{
		this.langCode = langCode;
		links = new List<string>();
	}

	public LinkBlocks(string langCode, List<string> links)
	{
		this.langCode = langCode;
		this.links = links;
	}
}