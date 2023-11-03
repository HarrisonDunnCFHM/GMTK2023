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
        float cardWidth = enemyCardPrefab.GetComponent<SpriteRenderer>().size.x;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * screenAspect;
        for (int cardPos = 0; cardPos < cardCount; cardPos++)
        {
            float newXPos = ((cardWidth * cardPos) - (cardWidth * (cardCount -1) / 2)) * attackSpacingModifier ;
            float newStartingYPos = screenHeight / -2.5f;
            //float newEndingYPos = mainCanvas.GetComponent<RectTransform>().rect.height / -3.5f;
            GameObject newAttackObj = Instantiate(enemyCardPrefab, transform.position, Quaternion.identity);
            Vector3 startingPos = new Vector3(newXPos, newStartingYPos, 0);
            //Vector3 endingPos = new Vector3(newXPos, newEndingYPos, 0);
            newAttackObj.transform.localPosition = startingPos;
            newAttackObj.GetComponentInChildren<TextMeshProUGUI>().text = attackList[cardPos].strength.ToString();
            EnemyAttack newAttack = newAttackObj.GetComponent<EnemyAttack>();
            int elementIndex = (int)attackList[cardPos].element;
            newAttack.SetSprites(elementIndex);
            newAttack.myAttackAtributes = attackList[cardPos];
            newAttack.attackPicker = this;
            newAttack.myAttackColumnXValue = newXPos;
            //newAttack.myPowerText.gameObject.transform.parent = mainCanvas.transform;
            //newAttack.myElementIcon.gameObject.transform.parent = mainCanvas.transform;
            generatedAttacksList.Add(newAttack);
            //create button to attack
            //GameObject attackButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
            //float newButtonYPos = enemyCardPrefab.GetComponent<RectTransform>().rect.height/2 + 
            //    selectButtonPrefab.GetComponent<RectTransform>().rect.height*1.5f + newStartingYPos;
            //attackButton.GetComponent<RectTransform>().localPosition = new Vector3(newXPos, newButtonYPos, 0);
            newAttack.myAttackColumnYValue = newStartingYPos ; //+ selectButtonPrefab.GetComponent<RectTransform>().rect.height / 2;
            //TextMeshProUGUI[] allTexts = attackButton.GetComponentsInChildren<TextMeshProUGUI>();
            //foreach(TextMeshProUGUI text in allTexts)
            //{
            //    if(text.name == "Choice Details")
            //    {
            //        text.text = "";
            //    }
            //    else
            //    {
            //        text.text = "Guide";
            //    }
            //}
        }
    }

    public void HideObject(GameObject objectToHide)
    {
        if (resolvingAttack) { return; }
        objectToHide.SetActive(false);
    }

    public void UseAttack(EnemyAttack attack, int strength, Element element)
    {
        if (resolvingAttack) { return; } else { resolvingAttack = true; }
        lastAttack = attack;
        lastAttackStrength = strength;
        lastAttackElement = element;
        StartCoroutine(battleSequence.PlayOutAttack());
    }

    
}
