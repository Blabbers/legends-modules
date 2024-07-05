using BeauRoutine;
using Blabbers.Game00;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UI_CameraFX : MonoBehaviour, ISingleton
{
    [SerializeField] private GameObject vignette;
    [SerializeField] private GameObject circleIn;
    [SerializeField] private GameObject circleOut;
    [SerializeField] private GameObject cinematicBlackBars;
    [SerializeField] private MotionTweenPlayer cinematicBarTop, cinematicBarBot;
	[SerializeField] private MotionTweenPlayer circleInTween, circleOutTween;
	public TextMeshProUGUI cinematicDidYouKnowTip;
	private Camera camera;

	#region Instance
	private static UI_CameraFX _instance = null;
	public static UI_CameraFX Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<UI_CameraFX>("CameraFXPrefab"));
				_instance.gameObject.name = "--Screen--CameraFX";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion
	    
    public void OnCreated()
    {
    }
    private void Awake()
    {
        camera = Camera.main;
    }

    public void CircleIn(Vector3 worldPosition, UnityAction callback = null , float delay = 0f)
    {
		if (callback!=null) circleInTween.OnAnimationFinished.AddListener(callback);

		Routine.Start(Routine.Delay(Run, delay));
        void Run()
        {
            circleIn.transform.parent.gameObject.SetActive(true);
            circleIn.SetActive(true);
            circleIn.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        }
    }
    
    public void CircleOut(Vector3 worldPosition, UnityAction callback = null , float delay = 0f)
    {
		if (callback != null) circleOutTween.OnAnimationFinished.AddListener(callback);

		Routine.Start(Routine.Delay(Run, delay));
        void Run()
        {
            circleIn.transform.parent.gameObject.SetActive(false);
            circleOut.transform.parent.gameObject.SetActive(true);
            circleOut.SetActive(true);
            circleOut.transform.position = camera.WorldToScreenPoint(worldPosition);   
        }
    }

	public void ShowCinematicBlackBars(UnityAction callback, string hintText)
	{
		cinematicDidYouKnowTip.text = hintText;

		// First make sure the childs are enabled
		cinematicBarTop.gameObject.SetActive(true);
		cinematicBarBot.gameObject.SetActive(true);

		cinematicBarTop.OnAnimationFinished.RemoveAllListeners();
		cinematicBarTop.OnAnimationFinished.AddListener(callback);

		// Then enable the parent that will trigger the animation
		cinematicBlackBars.SetActive(true);
	}

	public void HideCinematicBlackBars(UnityAction callback)
	{
		cinematicBarTop.DisableWithExitTween(HandleTweenFinished);
		cinematicBarBot.DisableWithExitTween(null);
		void HandleTweenFinished()
		{
			cinematicDidYouKnowTip.text = "";
			callback?.Invoke();
			cinematicBlackBars.SetActive(false);
		}
	}

}
