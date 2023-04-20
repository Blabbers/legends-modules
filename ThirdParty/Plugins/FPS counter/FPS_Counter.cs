using UnityEngine;


public class FPS_Counter : MonoBehaviour
{
    /* Assign this script to any object in the Scene to display frames per second */

    public Color displayColor = Color.yellow;
    public float updateInterval = 0.5f; //How often should the number update
    public int fontSize = 15;
    public Vector2 textDimensions = new Vector2(100, 25);

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();

    // Use this for initialization
    void Start()
    {

        timeleft = updateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = displayColor;
        textStyle.fontSize = fontSize;
	}

	// Update is called once per frame
	void Update()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_CLOUD_BUILD

		textStyle.normal.textColor = displayColor;
		timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
#endif
	}

#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_CLOUD_BUILD
	void OnGUI()
    {
        //Display the fps and round to 2 decimals

        GUI.Label(new Rect(5, 5, textDimensions.x, textDimensions.y), fps.ToString("F2") + "FPS", textStyle);
    }

#endif

}
