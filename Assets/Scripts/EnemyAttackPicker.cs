using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAttackPicker : MonoBehaviour
{
    public List<EnemyAttackAttributes> attackList;
    public List<EnemyAttack> generatedAttacksList;
    public GameObject attackCardPrefab;
    public float attackSpacingModifier = 1.4f;

    public BattleSequence battleSequence;

    [SerializeField] private TextMeshProUGUI usedAttackText;
    [SerializeField] private TextMeshProUGUI usedAttackPowerText;
    [SerializeField] private TextMeshProUGUI usedAttackElementText;
    public EnemyAttack lastAttack;
    public int lastAttackStrength;
    public Element lastAttackElement;

    public bool resolvingAttack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadNextBattleFromPlayerData();
        DisplayAttacks();
    }

    private void LoadNextBattleFromPlayerData()
    {
        if(PlayerData.nextBattle == null) { return; }
        attackList = PlayerData.nextBattle.enemyAttacks;
    }

    public void DisplayAttacks()
    {
        if(attackList.Count == 0) { return; }
        generatedAttacksList = new();
        float cardCount = attackList.Count;
        for (int cardPos = 0; cardPos < cardCount; cardPos++)
        {
            float newXPos = ((cardCount - 1) * -0.5f + cardPos) * attackSpacingModifier;
            GameObject newAttackObj = Instantiate(attackCardPrefab,transform.position,Quaternion.identity);
            newAttackObj.transform.position = new Vector3(newXPos, transform.position.y, 0);
            newAttackObj.GetComponentInChildren<TextMeshProUGUI>().text = attackList[cardPos].element.ToString() + " " + attackList[cardPos].strength.ToString();
            EnemyAttack newAttack = newAttackObj.GetComponent<EnemyAttack>();
            newAttack.myAttackAtributes = attackList[cardPos];
            newAttack.attackPicker = this;
            generatedAttacksList.Add(newAttack);
        }
    }

    public void UseAttack(EnemyAttack attack, int strength, Element element)
    {
        if (resolvingAttack) { return; } else { resolvingAttack = true; }
        lastAttack = attack;
        lastAttackStrength = strength;
        lastAttackElement = element;
        usedAttackText.text = "Enemy uses " + strength + " power " + element.ToString() + " attack!";
        usedAttackPowerText.text = strength.ToString();
        usedAttackElementText.text = element.ToString();
        StartCoroutine(battleSequence.PlayOutAttack());
    }

    
}
