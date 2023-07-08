using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battle")]
public class BattleAttributes : ScriptableObject
{
    public List<EnemyAttackAttributes> enemyAttacks;
}
