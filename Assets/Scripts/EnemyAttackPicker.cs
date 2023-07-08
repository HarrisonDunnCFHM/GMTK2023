using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAttackPicker : MonoBehaviour
{
    public List<EnemyAttackAttributes> attackList;
    public GameObject attackCardPrefab;
    public float attackSpacingModifier = 1.4f;

    public BattleSequence battleSequence;

    [SerializeField] private TextMeshProUGUI usedAttackText;
    [SerializeField] private TextMeshProUGUI usedAttackPowerText;
    [SerializeField] private TextMeshProUGUI usedAttackElementText;
    public int lastAttackStrength;
    public Element lastAttackElement;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadNextBattleFromPlayerData();
        DisplayAttacks();
    }

    private void LoadNextBattleFromPlayerData()
    {
        attackList = PlayerData.nextBattle.enemyAttacks;
    }

    public void UseAttack(int strength, Element element)
    {
        lastAttackStrength = strength;
        lastAttackElement = element;
        usedAttackText.text = "Enemy uses " + strength + " power " + element.ToString() + " attack!";
        usedAttackPowerText.text = strength.ToString();
        usedAttackElementText.text = element.ToString();
        StartCoroutine(battleSequence.PlayOutAttack());
    }

    public void DisplayAttacks()
    {
        if(attackList.Count == 0) { return; }
        float cardCount = attackList.Count;
        for (int cardPos = 0; cardPos < cardCount; cardPos++)
        {
            float newXPos = ((cardCount - 1) * -0.5f + cardPos) * attackSpacingModifier;
            GameObject newAttack = Instantiate(attackCardPrefab,transform.position,Quaternion.identity);
            newAttack.transform.position = new Vector3(newXPos, transform.position.y, 0);
            newAttack.GetComponentInChildren<TextMeshProUGUI>().text = attackList[cardPos].element.ToString() + " " + attackList[cardPos].strength.ToString();
            newAttack.GetComponent<EnemyAttack>().myAttackAtributes = attackList[cardPos];
            newAttack.GetComponent<EnemyAttack>().attackPicker = this;
        }
    }


}
