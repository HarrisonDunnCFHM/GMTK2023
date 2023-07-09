using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyAttackPicker : MonoBehaviour
{
    public List<EnemyAttackAttributes> attackList;
    public List<EnemyAttack> generatedAttacksList;
    public GameObject attackCardPrefab;
    public float attackSpacingModifier = 1.4f;

    public BattleSequence battleSequence;

    //[SerializeField] private TextMeshProUGUI usedAttackText;
    //[SerializeField] private TextMeshProUGUI usedAttackPowerText;
    //[SerializeField] private TextMeshProUGUI usedAttackElementText;
    public EnemyAttack lastAttack;
    public int lastAttackStrength;
    public Element lastAttackElement;

    public bool resolvingAttack = false;
    public Canvas mainCanvas;
    public GameObject enemyCardPrefab;
    public GameObject selectButtonPrefab;

    
    // Start is called before the first frame update
    void Start()
    {
        LoadNextBattleFromPlayerData();
        DisplayAttacksUI();
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

    public void DisplayAttacksUI()
    {
        if (attackList.Count == 0) { return; }
        generatedAttacksList = new();
        float cardCount = attackList.Count;
        float cardWidth = enemyCardPrefab.GetComponent<RectTransform>().rect.width;
        for (int cardPos = 0; cardPos < cardCount; cardPos++)
        {
            float newXPos = ((cardWidth * cardPos) - (cardWidth * (cardCount -1) / 2)) * attackSpacingModifier ;
            float newYPos = mainCanvas.GetComponent<RectTransform>().rect.height / -4;
            //((cardCount - 1) * -0.5f + cardPos) * attackSpacingModifier;
            //(((cardCount) * cardWidth) - (cardWidth * (cardPos) / 2)) * attackSpacingModifier
            GameObject newAttackObj = Instantiate(enemyCardPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
            newAttackObj.GetComponent<RectTransform>().localPosition = new Vector3(newXPos, newYPos, 0);
            newAttackObj.GetComponentInChildren<TextMeshProUGUI>().text = attackList[cardPos].strength.ToString();
            EnemyAttack newAttack = newAttackObj.GetComponent<EnemyAttack>();
            int elementIndex = (int)attackList[cardPos].element;
            newAttack.SetSprites(elementIndex);
            newAttack.myAttackAtributes = attackList[cardPos];
            newAttack.attackPicker = this;
            newAttack.myAttackColumnXValue = newXPos;
            generatedAttacksList.Add(newAttack);
            //create button to attack
            GameObject attackButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
            float newButtonYPos = enemyCardPrefab.GetComponent<RectTransform>().rect.height/2 + 
                selectButtonPrefab.GetComponent<RectTransform>().rect.height/2 + newYPos;
            attackButton.GetComponent<RectTransform>().localPosition = new Vector3(newXPos, newButtonYPos, 0);
            newAttack.myAttackColumnYValue = newButtonYPos + selectButtonPrefab.GetComponent<RectTransform>().rect.height / 2;
            TextMeshProUGUI[] allTexts = attackButton.GetComponentsInChildren<TextMeshProUGUI>();
            foreach(TextMeshProUGUI text in allTexts)
            {
                if(text.name == "Choice Details")
                {
                    text.text = "";
                }
                else
                {
                    text.text = "Attack!";
                }
            }
            attackButton.GetComponent<Button>().onClick.AddListener(() => newAttack.GetAttacked());
        }
    }

    public void UseAttack(EnemyAttack attack, int strength, Element element)
    {
        if (resolvingAttack) { return; } else { resolvingAttack = true; }
        lastAttack = attack;
        lastAttackStrength = strength;
        lastAttackElement = element;
        //usedAttackText.text = "Enemy uses " + strength + " power " + element.ToString() + " attack!";
        //usedAttackPowerText.text = strength.ToString();
        //usedAttackElementText.text = element.ToString();
        StartCoroutine(battleSequence.PlayOutAttack());
    }

    
}
