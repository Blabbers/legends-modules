using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Blabbers.Game00;

public class Fade : MonoBehaviour, ISingleton
{
	public void OnCreated()
	{
	}

	public static Fade Instance => Singleton.Get<Fade>();
	public Color color;
	public Image fadeSprite;
	public AnimationCurve easeCurve;

	public static void Out(float duration = 1f)
	{
		var color = Instance.fadeSprite.color;
		color.a = 0f;
		Instance.fadeSprite.color = color;
		Instance.fadeSprite.DOFade(1f, duration).SetEase(Instance.easeCurve);
	}
	public static void In(float duration = 1f)
	{
		var color = Instance.fadeSprite.color;
		color.a = 1f;
		Instance.fadeSprite.color = color;
		Instance.fadeSprite.DOFade(0f, duration).SetEase(Instance.easeCurve);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (scene.name == "loading") return;
		In(1f);
	}

	void Awake()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			//Fade.In(1f);
			Fade.Out(1f);
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			//Fade.Out(1f);
			Fade.In(1f);
		}
	}
#endif
}