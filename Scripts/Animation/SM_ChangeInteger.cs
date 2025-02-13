using UnityEngine;

public enum StateMachineState
{
    OnStateEnter, OnStateUpdate, OnStateExit, OnStateMachineEnter, OnStateMachineExit,
}

public class SM_ChangeInteger : StateMachineBehaviour
{
    [SerializeField]
    private StateMachineState state;
    [SerializeField]
    private string parameter;
    [SerializeField]
    private int value;

    public void ChangeParameter(Animator animator, StateMachineState state)
    {
        if (this.state == state)
        {
            animator.SetInteger(parameter, value);
        }
    }
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateEnter);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateUpdate);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateExit);
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
    
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        ChangeParameter(animator, StateMachineState.OnStateMachineEnter);
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        ChangeParameter(animator, StateMachineState.OnStateMachineExit);
    }
}
