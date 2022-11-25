using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public enum InputComparison
{
	EqualsTo,
	BiggerThan,
	SmallerThan,
}

public enum InputType
{
	GetButtonDown,
	GetAxis,
}

public class InputPressCheck : MonoBehaviour
{
	public InputType inputType;	
	
	// Get Button Down
	private bool editorShowButtonDown => inputType == InputType.GetButtonDown;	
	[ShowIf(nameof(editorShowButtonDown))]
	public string inputName;

	// Get Axis
	private bool editorShowGetAxis => inputType == InputType.GetAxis;
	[ShowIf(nameof(editorShowGetAxis))]
	public float axisValue;
	[ShowIf(nameof(editorShowGetAxis))]
	public InputComparison axisCheck;

	// On Pressed generic event
	public UnityEvent OnPressed;

	void Update()
	{
		if (inputType == InputType.GetButtonDown)
		{
			if (Input.GetButtonDown(inputName))
			{
				OnPressedInvoke();
			}
		}

		if (inputType == InputType.GetAxis)
		{
			var axis = Input.GetAxis(inputName);
			switch (axisCheck)
			{
				case InputComparison.EqualsTo:
					if (axis == axisValue) OnPressedInvoke();
					break;
				case InputComparison.BiggerThan:
					if (axis > axisValue) OnPressedInvoke();
					break;
				case InputComparison.SmallerThan:
					if (axis < axisValue) OnPressedInvoke();
					break;
			}
		}
	}

	// Public for the possibility of external inspector re-use
	public void OnPressedInvoke()
	{
		if (!this.isActiveAndEnabled) return;
		OnPressed?.Invoke();
	}
}
