using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Attack", menuName = "Enemy")]

public class EnemyAttackAttributes : ScriptableObject
{
    public int strength;
    public Element element;
    public bool used = false;
}
