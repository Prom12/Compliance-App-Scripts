using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingTransitions : StateMachineBehaviour
{

    [SerializeField] private float _timeUntilSitting;
    [SerializeField] private int _numberOfSittingAnimation;

    private bool _isSitting;
    private float _idleTime;

    private int _sittingAnimation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_isSitting == false){
            _idleTime += Time.deltaTime;

            if(_idleTime > _timeUntilSitting && stateInfo.normalizedTime % 1 < 0.02f){
                _isSitting = true;
                _sittingAnimation = Random.Range(1, _numberOfSittingAnimation + 1);

            }
       }
       else if(stateInfo.normalizedTime % 1 > 0.98){
            ResetIdle();
       }

        animator.SetFloat("SittingTrans",_sittingAnimation, 0.2f, Time.deltaTime);

    }

    private void ResetIdle(){
        _isSitting = false;
        _idleTime = 0;
        _sittingAnimation = 0;
    }
    
}
