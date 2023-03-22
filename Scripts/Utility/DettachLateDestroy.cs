using UnityEngine;

public class DettachLateDestroy : MonoBehaviour
{
	[SerializeField]
	private Transform _parentToFollow;
	[SerializeField]
	private float _delayToDestroyAfterDettach = 1f;
	private ParticleSystem[] _allParticles;
	private GameObject _tempParent;
	[SerializeField]
	private bool followPosition = true;
	[SerializeField]
	private bool followRotation = true;
	[SerializeField]
	private bool followScale = true;

	private void Start()
	{
		_allParticles = GetComponentsInChildren<ParticleSystem>();
		_tempParent = new GameObject("[VirtualParent] " + this.name);
		UpdateAttachmentPosition();
		this.transform.SetParent(_tempParent.transform, true);
	}

	private void Update()
	{
		UpdateAttachmentPosition();
	}

	private bool _parentWasDestroyed;
	void UpdateAttachmentPosition()
	{
		if (_parentWasDestroyed)
			return;

		if (_parentToFollow)
		{
			if (followPosition) _tempParent.transform.position = _parentToFollow.position;
			if (followRotation) _tempParent.transform.rotation = _parentToFollow.rotation;
			if (followScale) _tempParent.transform.localScale = _parentToFollow.localScale;
		}
		else
		{
			_parentWasDestroyed = true;
			// Sets this to be destroyed with a delay and turn of emissions of all subparticles
			Destroy(_tempParent, _delayToDestroyAfterDettach);
			foreach (var particle in _allParticles)
			{
				var emission = particle.emission;
				emission.enabled = false;
			}
		}
	}
}
