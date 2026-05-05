using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    static public PlaceManager Instance;

    [SerializeField] List<GameObject> characters;
    [SerializeField] List<GameObject> enemies;
    public IReadOnlyList<GameObject> Characters => characters;
    //public IReadOnlyList<GameObject> Enemies => enemies;
    static public int MainControl = 0;
    public GameObject MainControlCharacter => Characters[MainControl];

    [SerializeField] List<int> enemiesGroup;
    [SerializeField] int groupAmount;
    List<List<GameObject>> groupEnemies = new();
    [SerializeField] List<Transform> groupInitTransfrom = new();
    List<Vector3> groupInitPos = new();
    [SerializeField] List<float> groupEnterCombatRadius;
    [SerializeField] List<float> groupLeaveCombatRadius;
    public IReadOnlyList<List<GameObject>> GroupEnemies => groupEnemies;
    List<bool> groupEnemyIsDead = new();
    int combatNum = -1;
    public int CombatNum => combatNum;
    int combatNumTemp = -1;
    bool bossFly = false;

    //List<Vector3> enemyInitPos = new();
    //bool inCombat = false;
    //public bool InCombat = false;
    //[SerializeField] private List<float> enterCombatRadius;
    //[SerializeField] private List<float> leaveCombatRadius;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < groupAmount; i++)
        {
            groupEnemies.Add(new List<GameObject>());
            groupEnemyIsDead.Add(false);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            groupEnemies[enemiesGroup[i]].Add(enemies[i]);
        }
        for (int i = 0; i < groupAmount; i++)
        {
            if (groupInitTransfrom[i] != null)
            {
                groupInitPos.Add(groupInitTransfrom[i].position);
            }
            else if (groupEnemies[i].Count > 0)
            {
                Vector3 sumPosition = Vector3.zero;
                foreach (GameObject enemy in groupEnemies[i])
                {
                    sumPosition += enemy.transform.position;
                }
                 groupInitPos.Add(sumPosition / groupEnemies[i].Count);
            }
        }

        //enemyInitPos.Add(enemies[0].transform.position);
        //enemyInitPos.Add((enemies[1].transform.position + enemies[2].transform.position) / 2);
        //enemyInitPos.Add((enemies[1].transform.position + enemies[2].transform.position) / 2);
        //for (int i = 3; i < enemies.Count; i++)
        //{
        //    enemyInitPos.Add(enemies[i].transform.position);
        //}
    }
    private void Update()
    {
        if(combatNum == -1)
        {
            for (int i = 0; i < groupInitPos.Count; i++)
            {
                if (Vector3.Distance(groupInitPos[i], characters[MainControl].transform.position) < groupEnterCombatRadius[i] && !CheckGroupEnemyIsDead(i))
                {
                    combatNum = i;
                    CombatEnter(combatNum);
                    break;
                }
            }
        }
        else
        {
            if (GroupEnemies[0][0].GetComponent<BossState>().Fly)
            {
                return;
            }
            combatNumTemp = -1;
            for (int i = 0; i < groupInitPos.Count; i++)
            {
                if (Vector3.Distance(groupInitPos[i], characters[MainControl].transform.position) < groupLeaveCombatRadius[i] && !CheckGroupEnemyIsDead(i))
                {
                    combatNumTemp = i;
                    if(combatNum != combatNumTemp)
                    {
                        CombatChange(combatNum, combatNumTemp);
                        combatNum = combatNumTemp;
                    }
                    break;
                }
            }
            if(combatNumTemp == -1)
            {
                CombatLeave(combatNum);
                combatNum = combatNumTemp;
            }
        }
        //if (inCombat)
        //{
        //    inCombat = false;
        //    for (int i = 0; i < enemyInitPos.Count; i++)
        //    {
        //        if (Vector3.Distance(enemyInitPos[i], characters[mainControl].transform.position) < leaveCombatRadius[i] && !enemies[i].GetComponent<EnemyCombatData>().CombatUnitData.isDead)
        //        {
        //            inCombat = true;
        //            break;
        //        }
        //    }
        //    if (!inCombat)
        //    {
        //        InCombat = false;
        //        for (int j = 0; j < characters.Count; j++)
        //        {
        //            if (j == mainControl)
        //            {
        //                Animator animator = characters[j].GetComponent<Animator>();
        //                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("NoWeapon"))
        //                {
        //                    animator.SetTrigger("outCombat");
        //                    animator.SetTrigger("Exit");
        //                }
        //            }
        //            else
        //            {
        //                characters[j].GetComponent<NPCBehavior>().CombatLeave();
        //            }
        //        }
        //        for (int j = 0; j < enemies.Count; j++)
        //        {
        //            if (enemies[j].TryGetComponent<RockmanStateMachine>(out var stateMachine))
        //            {
        //                stateMachine.CombatLeave();
        //            }
        //            else if (enemies[j].TryGetComponent<DragonState>(out var dragonState))
        //            {
        //                dragonState.CombatLeave();
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < enemyInitPos.Count; i++)
        //    {
        //        if (Vector3.Distance(enemyInitPos[i], characters[mainControl].transform.position) < enterCombatRadius[i] && !enemies[i].GetComponent<EnemyCombatData>().CombatUnitData.isDead)
        //        {
        //            inCombat = true;
        //            InCombat = true;
        //            for (int j = 0; j < characters.Count; j++)
        //            {
        //                characters[j].GetComponent<CharacterCombatLive>().EnterComBat();
        //                if (j != mainControl) characters[j].GetComponent<NPCBehavior>().CombatTrigger();
        //            }
        //            if (enemies[i].TryGetComponent<RockmanStateMachine>(out var stateMachine))
        //            {
        //                stateMachine.CombatTrigger();
        //            }
        //            else if (enemies[i].TryGetComponent<DragonState>(out var dragonState))
        //            {
        //                dragonState.CombatTrigger();
        //            }
        //            else if (enemies[i].TryGetComponent<BossState>(out var bossState))
        //            {
        //                bossState.CombatTrigger();
        //                for (int j = 0; j < characters.Count; j++)
        //                {
        //                    characters[j].GetComponent<CharacterCombatLive>().EnterComBat();
        //                    if (j != mainControl) characters[j].GetComponent<NPCBehavior>().BossCombatTrigger();
        //                }
        //            }
        //        }
        //    }
        //}
    }
    void OnDrawGizmos()
    {
        if (groupInitPos == null) return;

        for (int i = 0; i < groupInitPos.Count; i++)
        {
            if (groupInitPos[i] == null) continue;

            // 進入戰鬥範圍 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groupInitPos[i], groupEnterCombatRadius[i]);

            // 離開戰鬥範圍
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groupInitPos[i], groupLeaveCombatRadius[i]);
        }
    }
    bool CheckGroupEnemyIsDead(int i)
    {
        if(groupEnemyIsDead[i])
        {
            return true;
        }
        foreach (GameObject enemy in groupEnemies[i])
        {
            if (!enemy.GetComponent<EnemyCombatData>().CombatUnitData.isDead)
            {
                return false;
            }
        }
        groupEnemyIsDead[i] = true;
        return true;
    }
    void CombatLeave(int i)
    {
        for (int j = 0; j < characters.Count; j++)
        {
            if (j == MainControl)
            {
                Animator animator = characters[j].GetComponent<Animator>();
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("NoWeapon"))
                {
                    animator.SetTrigger("outCombat");
                    animator.SetTrigger("Exit");
                }
            }
            else
            {
                characters[j].GetComponent<NPCBehavior>().CombatLeave();
            }
        }
        foreach(GameObject enemy in groupEnemies[i])
        {
            if (enemy.TryGetComponent<RockmanStateMachine>(out var rockmanState))
            {
                rockmanState.CombatLeave();
            }
            else if (enemy.TryGetComponent<DragonState>(out var dragonState))
            {
                dragonState.CombatLeave();
            }
        }
    }
    void CombatChange(int i, int j)
    {
        foreach (GameObject enemy in groupEnemies[i])
        {
            if (enemy.TryGetComponent<RockmanStateMachine>(out var rockmanState))
            {
                rockmanState.CombatLeave();
            }
            else if (enemy.TryGetComponent<DragonState>(out var dragonState))
            {
                dragonState.CombatLeave();
            }
        }
        foreach (GameObject enemy in groupEnemies[j])
        {
            if (enemy.TryGetComponent<RockmanStateMachine>(out var rockmanState))
            {
                rockmanState.CombatTrigger();
            }
            else if (enemy.TryGetComponent<DragonState>(out var dragonState))
            {
                dragonState.CombatTrigger();
            }
            else if (enemy.TryGetComponent<BossState>(out var bossState))
            {
                bossState.CombatTrigger();
                for (int k = 0; k < characters.Count; k++)
                {
                    characters[j].GetComponent<CharacterCombatLive>().EnterComBat();
                    if (k != MainControl) characters[k].GetComponent<NPCBehavior>().BossCombatTrigger();
                }
            }
        }
    }
    void CombatEnter(int i)
    {
        for (int j = 0; j < characters.Count; j++)
        {
            characters[j].GetComponent<CharacterCombatLive>().EnterComBat();
            if (j != MainControl) characters[j].GetComponent<NPCBehavior>().CombatTrigger();
        }
        foreach (GameObject enemy in groupEnemies[i])
        {
            if (enemy.TryGetComponent<RockmanStateMachine>(out var rockmanState))
            {
                rockmanState.CombatTrigger();
            }
            else if (enemy.TryGetComponent<DragonState>(out var dragonState))
            {
                dragonState.CombatTrigger();
            }
            else if (enemy.TryGetComponent<BossState>(out var bossState))
            {
                bossState.CombatTrigger();
                for (int j = 0; j < characters.Count; j++)
                {
                    characters[j].GetComponent<CharacterCombatLive>().EnterComBat();
                    if (j != MainControl && !bossFly) characters[j].GetComponent<NPCBehavior>().BossCombatTrigger();
                }
            }
        }
    }
    public void BossFlyChangeCombatNum()
    {
        combatNum = groupAmount - 1;
        bossFly = true;
    }
}
