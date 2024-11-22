using NaughtyAttributes;
using UnityEngine;

public class SM_RandomInteger : StateMachineBehaviour
{
    [SerializeField]
    private StateMachineState state;
    [SerializeField]
    private string parameter;
    [SerializeField, Header("Random Inclusive")]
    private int minValue;
    [SerializeField]
    private int maxValue;
    
    public void ChangeParameter(Animator animator, StateMachineState state)
    {
        if (this.state == state)
        {
            var randomValue = Random.Range(minValue, maxValue+1);
            animator.SetInteger(parameter, randomValue);
        }
    }
    
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateEnter);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateUpdate);
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChangeParameter(animator, StateMachineState.OnStateExit);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
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
