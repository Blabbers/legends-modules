using BeauRoutine;
using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class SM_PlaySFX : StateMachineBehaviour
{
	[SerializeField]
	private StateMachineState state;
	[SerializeField]
	private float delay = 0f;	
	[SerializeField]
	private bool playSelectedIndex = false;
	[ShowIf("playSelectedIndex")]
	[SerializeField]
	private int selectedIndex;
	[BoxGroup("SFX")]
	public AudioSFX sfxClip;

	private void PlaySFX(Animator animator, StateMachineState state)
	{
		if (this.state == state)
		{
			Play();
		}
	}

	public void Play()
	{
		Routine.Start(Run());
		IEnumerator Run()
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			if (playSelectedIndex)
			{
				sfxClip.PlaySelectedIndex(selectedIndex);
			}
			else
			{
				sfxClip.Play();
			}
		}
	}

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlaySFX(animator, StateMachineState.OnStateEnter);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlaySFX(animator, StateMachineState.OnStateUpdate);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		PlaySFX(animator, StateMachineState.OnStateExit);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
