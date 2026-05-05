using System.Globalization;
using UnityEngine;

public class BasicAttackSubState : StateMachineBehaviour
{
    LocomotionBlend locomotionBlend;
    CharacterCombatLive characterCombatLive;
    SkillManager skillManager;
    EnableWeaponCollider enableWeaponCollider;
    CheckPlayerAttackArea checkPlayerAttackArea;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (skillManager == null)
        //    skillManager = animator.GetComponent<SkillManager>();
        //if (skillManager != null)
        //    skillManager.StopDisableBasicAttackEffect();

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
            characterCombatLive.IsSkillable(true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (characterCombatLive == null)
        {
            characterCombatLive = animator.GetComponent<CharacterCombatLive>();
        }
        if (characterCombatLive != null)
        {
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
            if (nextInfo.IsTag("basic") || nextInfo.IsTag("Evade"))
                characterCombatLive.AttackCountAdd();
            else
                characterCombatLive.AttackCountZero();
        }
    }
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (skillManager == null)
            skillManager = animator.GetComponent<SkillManager>();
        if (skillManager != null)
            skillManager.StartDisableBasicAttackEffect(0.25f);


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
