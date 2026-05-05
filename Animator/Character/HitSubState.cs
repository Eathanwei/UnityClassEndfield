using UnityEngine;

public class HitSubState : StateMachineBehaviour
{
    LocomotionBlend locomotionBlend;
    CharacterCombatLive characterCombatLive;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (locomotionBlend == null)
            locomotionBlend = animator.GetComponent<LocomotionBlend>();
        if (locomotionBlend != null)
        {
            locomotionBlend.IsWsadRotate(false);
            locomotionBlend.IsFacingTarget(false);
        }

        if (characterCombatLive == null)
            characterCombatLive = animator.GetComponent<CharacterCombatLive>();
        if (characterCombatLive != null)
            characterCombatLive.IsSkillable(false);
    }
}
