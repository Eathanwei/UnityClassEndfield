using UnityEngine;

public class BossGetHit : StateMachineBehaviour
{
    BossState bossState;
    SkillManager skillManager;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (bossState == null)
            bossState = animator.GetComponent<BossState>();
        if (bossState != null)
            bossState.Movable(false);
        if(skillManager == null)
            skillManager = animator.GetComponent<SkillManager>();
        if (skillManager != null)
            skillManager.StartDisableBasicAttackEffect(0.25f);

        animator.ResetTrigger("Basic");
        animator.ResetTrigger("Claw");
        animator.ResetTrigger("Flame");
        animator.ResetTrigger("Hit");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

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
