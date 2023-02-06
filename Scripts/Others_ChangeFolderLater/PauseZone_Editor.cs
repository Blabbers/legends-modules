

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseZone_Editor : MonoBehaviour
{

	public BoxCollider2D boxTrigger;
	public Color fillColor;




	public void TriggerEnter(Collider2D col)
	{
#if UNITY_EDITOR
		boxTrigger.gameObject.SetActive(false);
		Debug.Break();
		Debug.ClearDeveloperConsole();
#endif
	}


#if UNITY_EDITOR

	private void OnDrawGizmos()
	{
		DrawTrigger();
	}


	void DrawTrigger()
	{
		GizmosUtility.DrawWireRectangle(boxTrigger.transform.position, boxTrigger.size, Color.red, boxTrigger.offset);
		GizmosUtility.DrawRectangle(boxTrigger.transform.position, boxTrigger.size, fillColor, boxTrigger.offset);
	}

#endif


}


