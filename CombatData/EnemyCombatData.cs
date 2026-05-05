using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatData : CombatUnit
{
    [SerializeField] float hp;
    [SerializeField] float resistence;
    public float finalDMGReduce = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        selfEnemyCombatData = this;
        _combatUnitData.defense = 100;
        _combatUnitData.hp = hp;
        _combatUnitData.hpMax = hp;
        //hpMax, baseAttack, attack, defense;
        //public List<float> resistences;
        for (int i = 0; i< 5; i++)
        {
            _combatUnitData.resistences.Add(resistence);
        }
        finalDMGReduce = 0;
        NotifyHpChanged();
    }
    protected override void Update()
    {
        base.Update();
    }
}
