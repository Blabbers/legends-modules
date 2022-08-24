using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
	[SerializeField] private Vector2 parallaxArea = Vector2.one * 16;
	[SerializeField] private Vector2 parallaxEffectMultiplier = Vector2.right;
	[SerializeField] private bool infiniteHorizontal = true;
	[SerializeField] private bool infiniteVertical;

	private Transform cameraTransform;
	private Vector3 lastCameraPosition;
	private GameObject extraLeftLayer, extraRightLayer;

	void Start()
	{
		cameraTransform = Camera.main.transform;
		lastCameraPosition = cameraTransform.position;
		SetupExtraLayers();
	}

	private void SetupExtraLayers()
	{
		// Create and set the left side layer
		extraLeftLayer = new GameObject("Left_" + this.name);
		foreach (Transform child in this.transform)
		{
			Instantiate(child.gameObject, extraLeftLayer.transform);
		}
		var leftPos = extraLeftLayer.transform.position;
		leftPos.z += 0.001f;
		leftPos.x -= parallaxArea.x;
		extraLeftLayer.transform.position = leftPos;
		// Create and set the right side layer
		extraRightLayer = new GameObject("Right_" + this.name);
		foreach (Transform child in this.transform)
		{
			Instantiate(child.gameObject, extraRightLayer.transform);
		}
		var rightPos = extraRightLayer.transform.position;
		rightPos.z -= 0.001f;
		rightPos.x += parallaxArea.x;
		extraRightLayer.transform.position = rightPos;
		// Then parent them
		extraLeftLayer.transform.SetParent(this.transform);
		extraRightLayer.transform.SetParent(this.transform);
	}

	void LateUpdate()
	{
		var deltaMovement = cameraTransform.position - lastCameraPosition;
		transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
		lastCameraPosition = cameraTransform.position;

		if (infiniteHorizontal)
		{
			if (Mathf.Abs(cameraTransform.position.x - transform.position.x) > parallaxArea.x * 0.5f)
			{
				var offset = (cameraTransform.position.x - transform.position.x) % parallaxArea.x;
				var sign = Mathf.Sign(offset);
				var diff = Mathf.Abs(offset) - parallaxArea.x * 0.5f;
				var newPos = transform.position;
				newPos.x = cameraTransform.position.x + offset - diff * sign;
				transform.position = newPos;
			}
		}

		if (infiniteVertical)
		{
			if (Mathf.Abs(cameraTransform.position.y - transform.position.y) > parallaxArea.y * 0.5f)
			{
				var offset = (cameraTransform.position.y - transform.position.y) % parallaxArea.y;
				var sign = Mathf.Sign(offset);
				var diff = Mathf.Abs(offset) - parallaxArea.x * 0.5f;
				var newPos = transform.position;
				newPos.y = cameraTransform.position.y + offset - diff * sign;
				transform.position = newPos;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.transform.position, parallaxArea);
	}
}
