using UnityEngine;

public class MonsterDie : StateMachineBehaviour
{
    SkillManager skillManager;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (skillManager == null)
            skillManager = animator.GetComponent<SkillManager>();
        if (skillManager != null)
            skillManager.StartDisableAllAttackEffect(0.25f);
    }
}
