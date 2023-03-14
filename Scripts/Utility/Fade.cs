using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		Singleton = null;
	}

	public static Fade Singleton;
	public Color color;
	public Image fadeSprite;
	public AnimationCurve easeCurve;

	public static void Out(float duration = 1f)
	{
		var color = Singleton.fadeSprite.color;
		color.a = 0f;
		Singleton.fadeSprite.color = color;
		Singleton.fadeSprite.DOFade(1f, duration).SetEase(Singleton.easeCurve);
	}
	public static void In(float duration = 1f)
	{
		var color = Singleton.fadeSprite.color;
		color.a = 1f;
		Singleton.fadeSprite.color = color;
		Singleton.fadeSprite.DOFade(0f, duration).SetEase(Singleton.easeCurve);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		//Out(1f);
		if (scene.name == "loading") return;
		In(1f);
    }

    void Awake() {
        Singleton = this;
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