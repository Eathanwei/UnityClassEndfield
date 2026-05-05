using UnityEngine;
public class AttackCollider : StateMachineBehaviour
{
    // Unity Animation Behaviour, trigger weapon
    EnableWeaponCollider enableWeaponCollider;

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (nextInfo.IsTag("basic") || nextInfo.IsTag("skill"))
        {
            return;
        }
        if (enableWeaponCollider == null)
        {
            enableWeaponCollider = animator.GetComponent<EnableWeaponCollider>();
        }
        if (enableWeaponCollider != null)
        {
            enableWeaponCollider.DelayedStopAttack();
        }
    }
}
