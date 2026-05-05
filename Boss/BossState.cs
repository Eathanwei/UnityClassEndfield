using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CombatUnit;
public class BossState : MonoBehaviour
{
    Animator animator;
    bool fly = false;
    bool land = false;
    bool inCombat = false;
    bool isDead = false;
    bool movable = true;
    [SerializeField] Transform head;
    [SerializeField] List<GameObject> Walls;
    public bool Fly => fly;

    float attackTimeInterval = 0;
    float attackTimeIntervalMax = 4.5f;
    float skillTimeInterval;
    int skillType = 0;
    List<float> skillTimeIntervalMax = new()
    {
        10f, 15f, 20f
    };
    List<float> skillAttackDistance = new()
    {
        8.5f, 13.5f
    };

    float turnSpeed = 60f;
    float dotThresholdRotation = 0.985f;
    float dotThresholdBasic = 0.97f;
    float dotThresholdSkill = 0.95f;
    float runSpeed = 5f;
    float basicAttackDistance = 1.75f;
    [SerializeField] DragonBreath dragonBreath;
    [SerializeField] MeteorsManager meteorsManager;
    EnemyCombatData enemyCombatData;
    int characterMask;

    [SerializeField] RockmanStateMachine[] rockmanStateMachines;
     List<bool> rockmanIsAlive = new ();
    [SerializeField] DragonState[] dragonStates;
     List<bool> dragonIsAlive = new();
    int deadCount = 0;


    [SerializeField] ParticleSystem shield;
    [SerializeField] MarbleManager marbleManager;
    Coroutine marbleCoroutine;
    [SerializeField] GameObject UI;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterMask = LayerMask.GetMask("Character");
        enemyCombatData = GetComponent<EnemyCombatData>();
        foreach(var rockman in rockmanStateMachines)
        {
            rockmanIsAlive.Add(true);
        }
        foreach(var dragon in dragonStates)
        {
            dragonIsAlive.Add(true);
        }
    }
    private void Update()
    {
        if (!inCombat || isDead || !movable)
            return;
        if(!fly && !land && enemyCombatData.CombatUnitData.hp * 2 < enemyCombatData.CombatUnitData.hpMax)
        {
            fly = true;
            animator.SetTrigger("Fly");
            attackTimeInterval = 0;
            skillTimeInterval = 1000;
            StartFly();
            return;
        }
        attackTimeInterval += Time.deltaTime;
        skillTimeInterval += Time.deltaTime;
        if (fly && !land)
        {
            if (attackTimeInterval > attackTimeIntervalMax && skillTimeInterval > skillTimeIntervalMax[2])
            {
                attackTimeInterval = -1000f;
                skillTimeInterval = -1000f;
                StartCoroutine(MeteorsCoroutine());
            }
            CheckFly();
        }
        else if (fly)
        {
            return;
        }
        TryAttack();
    }
    public void CombatTrigger()
    {
        inCombat = true;
        animator.SetTrigger("Scream");
        foreach(var go in Walls)
        {
            go.SetActive(true);
        }
        UI.SetActive(true);
    }
    public void Hit(int attackNum)
    {
        if (attackNum > 3 && !isDead && !fly)
        {
            animator.SetTrigger("Hit");
        }
    }
    public void Death()
    {
        isDead = true;
        shield.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        StopCoroutine(marbleCoroutine);
        marbleManager.gameObject.SetActive(false);
        foreach(var go in Walls)
        {
            go.SetActive(false);
        }
        UI.SetActive(false);
        StartCoroutine(FadeInGame.instance.FadeOutRoutine());
    }
    public void LeaveAttack(int attackNum)
    {
        attackTimeInterval = 0;
        if(attackNum == 1 || attackNum == 2 || attackNum == 3)
            skillTimeInterval = 0;
    }
    public void EnterAttack(int attackNum)
    {
        switch (attackNum)
        {
            case 0:
                Vector3 startPos0 = head.position - head.forward * basicAttackDistance * 0.25f;
                Vector3 endPos0 = head.position + head.forward * basicAttackDistance * 1.75f;
                Collider[] hitColliders0 = Physics.OverlapCapsule(startPos0, endPos0, 0.75f, characterMask);
                foreach (var hit in hitColliders0)
                {
                    hit.gameObject.GetComponent<CharacterCombatLive>().Hit(transform.position, Physical, 500);
                    Debug.LogWarning("0");
                }
                break;
            case 1:
                StartCoroutine(ClawCoroutine());
                break;
            case 2:
                //dragonBreath.ExecuteBreath();
                break;
            case 3:
                //meteorsManager.PlayMeteor();
                break;
            default:
                break;

        }
    }
    public void Movable(bool movable)
    {
        this.movable = movable;
    }
    void Rotation()
    {
        GameObject player = PlaceManager.Instance.MainControlCharacter;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        targetDir.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        float dot = Vector3.Dot(transform.forward, targetDir);
        float headDot = Vector3.Dot((player.transform.position - head.transform.position).normalized, targetDir);
        //Debug.Log("Dot: " + dot + " HeadDot: " + headDot + " Distance: " + Vector3.Distance(player.transform.position, head.position));
        if (dot < dotThresholdRotation)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            //Debug.Log("Rotating");
        }
        else if ((headDot > 0 && Vector3.Distance(player.transform.position, head.position) < basicAttackDistance * 0.75) || headDot < 0) 
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
            transform.position -= transform.forward * runSpeed * Time.deltaTime;
            //Debug.Log("Backwards");
        }
        else if (headDot > 0 && Vector3.Distance(player.transform.position, head.position) > basicAttackDistance * 1.75) 
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
            transform.position += transform.forward * runSpeed * Time.deltaTime * 2;
            //Debug.Log("Forwards");
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            //Debug.Log("idle");
        }
        return;
    }
    void TryAttack()
    {
        float minDis = 1000;
        float maxDot = 0;
        if (attackTimeInterval > attackTimeIntervalMax)
        {
            foreach (GameObject go in PlaceManager.Instance.Characters)
            {
                Vector3 targetDir = (go.transform.position - transform.position).normalized;
                targetDir.y = 0;
                float dot = Vector3.Dot(transform.forward, targetDir);
                if (dot > dotThresholdSkill)
                {
                    maxDot = Mathf.Max(maxDot, dot);
                    minDis = Mathf.Min(minDis, Vector3.Distance(head.position, go.transform.position));
                }
            }
        }
        if(minDis < 100 && Attack(maxDot, minDis))
        {
            attackTimeInterval = -1000f;
            return;
        }
        Rotation();
    }
    bool Attack(float dot,float distance)
    {
        if(skillTimeInterval > skillTimeIntervalMax[skillType] && distance < skillAttackDistance[skillType])
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            if (skillType == 0)
            {
                animator.SetTrigger("Claw");
            }
            else if(skillType == 1)
            {
                animator.SetTrigger("Flame");
            }
            skillTimeInterval = -1000f;
            skillType++;
            skillType %= skillTimeIntervalMax.Count - 1;
            return true;
        }
        Vector3 startPos = head.position - head.forward * basicAttackDistance * 0.25f;
        Vector3 endPos = head.position + head.forward * basicAttackDistance * 1.75f;
        Collider[] hitColliders = Physics.OverlapCapsule(startPos, endPos, 0.8f, characterMask);
        if (hitColliders.Length > 0 && dot > dotThresholdBasic)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetTrigger("Basic");
            return true;
        }
        return false;
    }
    IEnumerator ClawCoroutine()
    {
        Vector3 startPos1 = head.position - head.forward * 6.5f;
        Vector3 endPos1 = head.position + head.forward * 8.5f;
        Collider[] hitCollider = Physics.OverlapCapsule(startPos1, endPos1, 1.5f, characterMask);
        yield return new WaitForSeconds(1.6f);
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Claw Attack"))
        {
            foreach (var hit in hitCollider)
            {
                hit.gameObject.GetComponent<CharacterCombatLive>().KnockDown(Physical, 750);
                Debug.LogWarning("1");
            }
        }
    }
    IEnumerator MeteorsCoroutine()
    {
        meteorsManager.PlayMeteor();
        yield return new WaitForSeconds(4f);
        animator.SetTrigger("Meteor");
    }
    void StartFly()
    {
        PlaceManager.Instance.BossFlyChangeCombatNum();
        foreach (var rockman in rockmanStateMachines)
        {
            rockman.gameObject.SetActive(true);
            rockman.CombatTrigger();
        }
        foreach(var dragon in dragonStates)
        {
            dragon.gameObject.SetActive(true);
            dragon.CombatTrigger();
        }
    }
    void CheckFly()
    {
        for (int i = 0; i < rockmanStateMachines.Length; i++)
        {
            if (rockmanIsAlive[i] && rockmanStateMachines[i].GetComponent<EnemyCombatData>().CombatUnitData.hp <= 0)
            {
                rockmanIsAlive[i] = false;
                deadCount++;
                StartCoroutine(MonsterDeadCoroutine(rockmanStateMachines[i].gameObject));
            }
        }
        for(int i = 0; i < dragonStates.Length; i++)
        {
            if (dragonIsAlive[i] && dragonStates[i].GetComponent<EnemyCombatData>().CombatUnitData.hp <= 0)
            {
                dragonIsAlive[i] = false;
                deadCount++;
                StartCoroutine(MonsterDeadCoroutine(dragonStates[i].gameObject));
            }
        }
        if (deadCount >= rockmanStateMachines.Length + dragonStates.Length)
        {
            StartCoroutine(LandCoroutine());
            land = true;
            animator.SetTrigger("Land");
            foreach (var character in PlaceManager.Instance.Characters )
            {
                if (character != PlaceManager.Instance.MainControlCharacter) character.GetComponent<NPCBehavior>().CombatTrigger();
            }
        }
    }
    IEnumerator LandCoroutine()
    {
        yield return new WaitForSeconds(4f);
        fly = false;
        marbleCoroutine = StartCoroutine(MarbleCoroutine());
    }
    IEnumerator MonsterDeadCoroutine(GameObject monster)
    {
        yield return new WaitForSeconds(1.3f);
        monster.SetActive(false);
    }
    IEnumerator MarbleCoroutine()
    {
        while (true)
        {
            animator.SetTrigger("Defend");
            shield.gameObject.SetActive(true);
            shield.Play(true);
            marbleManager.PlayMarble();
            GetComponent<EnemyCombatData>().finalDMGReduce = 0.9f; 
            yield return new WaitForSeconds(90f);
        }
    }
    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.blue;

    //    Vector3 center = head.position + head.forward;
    //    Vector3 size = new Vector3(15f, 3f, 3f);
    //    Vector3 center1 = head.position + head.forward * basicAttackDistance * 0.75f;
    //    Vector3 size1 = new Vector3(basicAttackDistance * 2, 1.5f, 1.5f);
    //    Vector3 center2 = head.position + head.forward * 7;
    //    Vector3 size2 = new Vector3(14, 1.5f, 1.5f);

    //    Gizmos.DrawWireCube(center, size);
    //    Gizmos.DrawWireCube(center1, size1);
    //    Gizmos.DrawWireCube(center2, size2);

    //}
}
