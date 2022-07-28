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
        //cinematicBarTop.PlaySequence();
        cinematicBlackBars.SetActive(true);
    }
    public void HideCinematicBlackBars()
    {
        //cinematicBarTop.PlaySequence();
        cinematicBlackBars.SetActive(false);
    }
}
