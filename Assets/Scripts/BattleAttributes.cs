using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle", menuName = "Battle")]
public class BattleAttributes : ScriptableObject
{
    public List<EnemyAttackAttributes> enemyAttacks;
    public Element preferredElement;
    public int preferredAttacks;
    public int offElementAttacks;
    public int totalAttacks;
    public int totalPower;
   
}
