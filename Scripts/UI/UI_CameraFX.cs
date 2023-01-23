using BeauRoutine;
using Blabbers.Game00;
using NaughtyAttributes;
using UnityEngine;

public class UI_CameraFX : MonoBehaviour, ISingleton
{


    [SerializeField] private GameObject vignette;
    [SerializeField] private GameObject circleIn;
    [SerializeField] private GameObject circleOut;
    [SerializeField] private GameObject cinematicBlackBars;
    [SerializeField] private MotionTweenPlayer cinematicBarTop, cinematicBarBot;
    private Camera camera;


	#region Instance
	private static UI_CameraFX _instance = null;
	public static UI_CameraFX Instance
	{
		get
		{
			if (!_instance) _instance = Resources.Load<UI_CameraFX>("CameraFXPrefab");
            _instance.gameObject.name = "--Screen--CameraFX";
			DontDestroyOnLoad(_instance);

			return _instance;
		}
	}
	#endregion



	//private static void InstantiateIfNull()
 //   {
 //       if(instance == null)
 //       {
 //           GameObject temp = Instantiate(Resources.Load("CameraFXPrefab")) as GameObject;
 //           temp.name = "--Screen--CameraFX";
 //           instance = temp.GetComponent<UI_CameraFX>();

 //           DontDestroyOnLoad(temp);

	//	}
           


	//}



    [Button]
    void CircleInTest()
    {
        CircleIn(Vector3.zero, 0.5f);

	}

    
    public void OnCreated()
    {
    }
    private void Awake()
    {
        camera = Camera.main;
    }

    public void CircleIn(Vector3 worldPosition, float delay = 0f)
    {
        Routine.Start(Routine.Delay(Run, delay));
        void Run()
        {
            circleIn.transform.parent.gameObject.SetActive(true);
            circleIn.SetActive(true);
            circleIn.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        }
    }
    
    public void CircleOut(Vector3 worldPosition, float delay = 0f)
    {
        Routine.Start(Routine.Delay(Run, delay));
        void Run()
        {
            circleIn.transform.parent.gameObject.SetActive(false);
            circleOut.transform.parent.gameObject.SetActive(true);
            circleOut.SetActive(true);
            circleOut.transform.position = camera.WorldToScreenPoint(worldPosition);   
        }
    }

    public void ShowCinematicBlackBars()
    {
        Debug.Log("UI_CameraFX - ShowCinematicBlackBars");

        //InstantiateIfNull();
		//cinematicBarTop.PlaySequence();
		cinematicBlackBars.SetActive(true);
    }
    public void HideCinematicBlackBars()
    {
        //InstantiateIfNull();

		//cinematicBarTop.PlaySequence();
		cinematicBlackBars.SetActive(false);
    }
}
