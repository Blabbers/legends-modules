using Blabbers;
using Blabbers.Game00;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleGameplayCamera : MonoBehaviour, ISingleton
{
	#region Variables
	public bool followTarget = false;

	//Runtime
	[Header("State")]
	[Foldout("Runtime")][SerializeField] protected bool isFollowing = false;
	[Foldout("Runtime")][SerializeField] protected bool isFocusing = false;
	[Foldout("Runtime")][SerializeField] protected bool isZooming = false;
	[Foldout("Runtime")][SerializeField] protected float currentVel;
	[Foldout("Runtime")][SerializeField] protected Vector3 targetPos;
	[Foldout("Runtime")][SerializeField] protected Transform cameraTR;
	

	[Foldout("Components")] public Transform targetObj;
	[Foldout("Components")][SerializeField] protected Camera mainCam;


	
	[Foldout("Configs")][SerializeField] protected AnimationCurve lerpCurve;
	[Foldout("Configs")][SerializeField] protected List<ZoomConfigs> zoomPresets;
	[Foldout("Configs")][SerializeField] protected Vector2 cameraOffset;
	[Foldout("Configs")][SerializeField] protected float cameraSpeed = 5.0f;
	[Foldout("Configs")][SerializeField] protected float cameraSmooth = .1f; 
	#endregion

	public void OnCreated(){}

	protected virtual void Awake() {
		cameraTR = transform;
		if(!mainCam) mainCam = GetComponent<Camera>();
		if (followTarget)
		{
			StartPosition((Vector2)targetObj.position);
		}
		

		//defaultFocus = mainCam.transform.localPosition;
		
	}

	public virtual void Update()
	{
		if (!followTarget || !isFollowing) return;
		if (isFocusing) return;

		targetPos = targetObj.position;
		LerpCameraPos(targetPos);            // Moves camera
	}


	public Camera GetCamera()
	{
		return mainCam;
	}

	#region Simple Methods
	public void StartPosition(Vector2 pos)
	{
		cameraTR.position = new Vector3(pos.x + cameraOffset.x, cameraTR.position.y, cameraTR.position.z);
	}

	public virtual void LerpCameraPos(Vector3 targetPos)
	{
		//float newX = Mathf.SmoothDamp(cameraTR.position.x, follow.position.x + cameraOffset.x, ref currentVel, cameraSmooth, cameraSpeed);
		//cameraTR.position = new Vector3(newX, cameraTR.position.y, cameraTR.position.z);

		float newX = Mathf.SmoothDamp(cameraTR.position.x, targetPos.x + cameraOffset.x, ref currentVel, cameraSmooth, cameraSpeed);
		cameraTR.position = new Vector3(newX, cameraTR.position.y, cameraTR.position.z);
	}
	#endregion

	#region Focus

	public virtual void FocusOn(Vector3 pos, ZoomConfigs preset, ActionEvent focusFinish, Action callback, float duration = 0.75f)
	{
		isFocusing = true;

		Vector3 newPos = FocusGeneric(pos);

		DG.Tweening.Sequence s = DOTween.Sequence();

		s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve);
		s.Join(mainCam.DOOrthoSize(preset.size, duration)).SetEase(lerpCurve);
		s.AppendCallback(() => focusFinish?.Invoke(callback));
		s.AppendCallback(() => GenericFocusFinish());
	}

	public virtual void FocusOn(Vector3 pos, ZoomConfigs preset, float duration = 0.75f, bool unscaledTime = false)
	{
		Debug.Log($"<SimpleGameplayCamera> FocusOn()".Colored("orange") +
			$"\npos: {pos} | zoom.size: {preset.size} | duration: {duration}".Colored("white"));

		isFocusing = true;

		Vector3 newPos = FocusGeneric(pos);

		DG.Tweening.Sequence s = DOTween.Sequence();

		if (unscaledTime)
		{
			s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve).SetUpdate(true);
			s.Join(mainCam.DOOrthoSize(preset.size, duration)).SetEase(lerpCurve).SetUpdate(true);
		}
		else
		{
			s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve);
			s.Join(mainCam.DOOrthoSize(preset.size, duration)).SetEase(lerpCurve);
		}


		s.AppendCallback(() => GenericFocusFinish());
	}

	protected Vector3 FocusGeneric(Vector3 pos)
	{
		Vector3 newPos = new Vector3(pos.x, pos.y, mainCam.transform.position.z);
		//previousZoom = zoom;

		//inTransition = true;
		//if (zoomCoroutine != null)
		//{
		//	StopCoroutine(zoomCoroutine);
		//}


		return newPos;
	}


	void GenericFocusFinish()
	{
		isZooming = false;
		isFocusing = false;
	}

	public void ResetFocus(UnityEvent callback, float duration = 0.75f)
	{
		Vector3 newPos = new Vector3(targetObj.position.x, targetObj.position.y, mainCam.transform.position.z);
		float size;



		DG.Tweening.Sequence s = DOTween.Sequence();
		size = zoomPresets[(int)ZoomMode.NormalZoom].size;

		s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve);
		s.Join(mainCam.DOOrthoSize(size, duration)).SetEase(lerpCurve);
		s.AppendCallback(() => ResetFocusFinish(callback));

		//mainCam.transform.DOLocalMove(newPos, duration).OnComplete(() => ResetFocusFinish(callback));

		//mainCam.transform.DOLocalMove(defaultFocus, duration).OnComplete(() => ResetFocusFinish(callback));
		//LerpCameraZoom(GetConfigs(previousZoom));
	}

	public void ResetFocus(Action callback, float duration = 0.75f)
	{
		Vector3 newPos = new Vector3(targetObj.position.x, targetObj.position.y, mainCam.transform.position.z);
		float size;



		DG.Tweening.Sequence s = DOTween.Sequence();
		size = zoomPresets[(int)ZoomMode.NormalZoom].size;

		s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve);
		s.Join(mainCam.DOOrthoSize(size, duration)).SetEase(lerpCurve);
		s.AppendCallback(() => ResetFocusFinish(callback));
	}

	public virtual void ResetFocus(float duration = 0.75f, bool unscaledTime = false)
	{
		Vector3 newPos = new Vector3(targetObj.position.x, targetObj.position.y, mainCam.transform.position.z);
		float size;

		DG.Tweening.Sequence s = DOTween.Sequence();
		size = zoomPresets[(int)ZoomMode.NormalZoom].size;

		if (unscaledTime)
		{
			s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve).SetUpdate(true);
			s.Join(mainCam.DOOrthoSize(size, duration)).SetEase(lerpCurve).SetUpdate(true);
		}
		else
		{
			s.Append(mainCam.transform.DOMove(newPos, duration)).SetEase(lerpCurve);
			s.Join(mainCam.DOOrthoSize(size, duration)).SetEase(lerpCurve);
		}

		s.AppendCallback(() => ResetFocusFinish());
	}


	protected void ResetFocusFinish(UnityEvent callback)
	{
		callback?.Invoke();
		ResetFocusFinish();
		//isZooming = false;
		//isFocusing = false;
	}



	protected void ResetFocusFinish(Action callback)
	{
		callback?.Invoke();
		ResetFocusFinish();
		//isZooming = false;
		//isFocusing = false;
	}

	protected virtual void ResetFocusFinish()
	{
		isZooming = false;
		isFocusing = false;
	}


	#endregion

	#region Zoom
	public virtual void ZoomOut()
	{
		Debug.Log("<SimpleGameplayCamera> ZoomOut".Colored());
		ZoomConfigs current = zoomPresets[(int)ZoomMode.ZoomOut];

		LerpCameraZoom(current);
	}

	public virtual void ZoomIn()
	{
		Debug.Log("<SimpleGameplayCamera> ZoomOut".Colored());
		ZoomConfigs current = zoomPresets[(int)ZoomMode.ZoomIn];

		LerpCameraZoom(current);
	}

	public virtual void ZoomReset()
	{
		Debug.Log("<SimpleGameplayCamera> ZoomOut".Colored());
		ZoomConfigs current = zoomPresets[(int)ZoomMode.NormalZoom];

		LerpCameraZoom(current);

	}

	public virtual void SoftZoomOut()
	{
		if (isZooming) return;

		Debug.Log("SoftZoomOut".Colored());

		ZoomConfigs current = zoomPresets[(int)ZoomMode.SoftZoomOut];
		LerpCameraZoom(current);


		//mainCam.DOOrthoSize(current.size + softZoomIntensity, 0.75f).OnComplete(() => inTransition = false);

		//SetZoomState(ZoomMode.ZoomOut);
	}


	public virtual void CustomZoom(ZoomConfigs current)
	{
		LerpCameraZoom(current);
	}



	protected void LerpCameraZoom(ZoomConfigs target)
	{
		if (isZooming) mainCam.DOKill();

		isZooming = true;
		mainCam.DOOrthoSize(target.size, target.duration).SetUpdate(true).OnComplete(()=> ZoomFinish());
	}

	void ZoomFinish()
	{
		isZooming = false;
		isFollowing = true;
	}

	#endregion
}



public enum ZoomMode
{
	NormalZoom, ZoomIn, ZoomOut, SoftZoomOut, ZoomInPlus, Custom 
}


[Serializable]
public class ZoomConfigs
{
	public string name = "Focus";
	public float size = 7;
	public float duration = 1.0f;
	public bool isDefault = false;

}

[Serializable]
public class ScreenShakeConfigs
{
	public float duration = 0.25f;
	public float intensity = 1;
	public int vibrato = 10;
	public float randomness = 90;
	public bool fadeOut = true;
}
