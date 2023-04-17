using System.Collections;
using System.Linq;
using BeauRoutine;
using Blabbers;
using Blabbers.Game00;
using DG.Tweening;
using Fungus;
using NaughtyAttributes;
using UnityEngine;

public class UI_LoadingScreen : MonoBehaviour,ISingleton
{

	[Foldout("Runtime")][SerializeField] string currentKey;
	[Foldout("Runtime")][SerializeField] string nextScene;

	[Foldout("Components")] [SerializeField] TextLocalized displayText;
	[Foldout("Components")][SerializeField] CanvasGroup hintBlock;
	[Foldout("Components")] public MotionTweenPlayer motionTween;
	[Foldout("Components")] public AudioListener audioListener;

	public void OnCreated()
	{
	}

	void Awake()
	{
		ToggleAudioListener(false);
	}

	private void OnEnable()
	{
		//Checar qual a cena atual ou previous e carregar o texto baseado nisso
		hintBlock.alpha = 0;
		
		//Debug.Break();
	}


	public void ToggleAudioListener(bool active)
	{
		audioListener.enabled = active;
	}

	public void SetNextSceneName(string nextScene)
	{

		
		this.nextScene = nextScene;

		var scenes = GameData.Instance.loadingHints.ToList();
		var found = (scenes.Find(x => ConvertScenePathToName(x.nextScene.ScenePath) == nextScene));

		if (!found.Equals(default(GameData.LoadingHint)))
		{
			currentKey = found.hintKey;
			displayText.LocalizeText(currentKey);

			hintBlock.DOFade(1.0f, 0.5f);
		}
		else
		{
			//Disable hint		
		}

		ToggleAudioListener(true);
	}


	private string ConvertScenePathToName(string path)
	{
		string sceneName;
		string[] split;
		string[] split2;

		split = path.Split('/');
		sceneName = split[split.Length - 1];

		split2 = sceneName.Split('.');

		return split2[0];
	}
}