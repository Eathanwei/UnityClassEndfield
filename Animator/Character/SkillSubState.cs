using UnityEngine;

public class SkillSubState : StateMachineBehaviour
{
    LocomotionBlend locomotionBlend;
    CharacterCombatLive characterCombatLive;
    SkillManager skillManager;
    EnableWeaponCollider enableWeaponCollider;
    CheckPlayerAttackArea checkPlayerAttackArea;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (locomotionBlend == null)
            locomotionBlend = animator.GetComponent<LocomotionBlend>();
        if (locomotionBlend != null)
        {
            locomotionBlend.IsWsadRotate(false);
            locomotionBlend.IsFacingTarget(true);
        }

        if (characterCombatLive == null)
            characterCombatLive = animator.GetComponent<CharacterCombatLive>();
        if (characterCombatLive != null)
        {
            characterCombatLive.skillNow = true;
            characterCombatLive.IsSkillable(false);
        }
    }
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (characterCombatLive == null)
            characterCombatLive = animator.GetComponent<CharacterCombatLive>();
        if (characterCombatLive != null)
            characterCombatLive.skillNow = false;

        if (skillManager == null)
            skillManager = animator.GetComponent<SkillManager>();
        if (skillManager != null)
            skillManager.StartDisableSkillAttackEffect(0.25f);

        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
        if (!nextInfo.IsTag("basic") && !nextInfo.IsTag("skill"))
        {
            if (enableWeaponCollider == null)
            {
                enableWeaponCollider = animator.GetComponent<EnableWeaponCollider>();
            }
            if (enableWeaponCollider != null)
            {
                enableWeaponCollider.DelayedStopAttack();
            }
            if (checkPlayerAttackArea == null)
            {
                checkPlayerAttackArea = animator.GetComponent<CheckPlayerAttackArea>();
            }
            if (checkPlayerAttackArea != null)
            {
                checkPlayerAttackArea.DelayedStopSkillCast();
            }
        }
    }
}
