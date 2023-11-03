using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EnemyAttack : MonoBehaviour
{
    public EnemyAttackAttributes myAttackAtributes;
    public EnemyAttackPicker attackPicker;
    public bool used;
    public float moveUpAmount;
    public float moveUpTime; 

    public TextMeshProUGUI myPowerText;
    public Image myElementIcon;
    public SpriteRenderer myEnemySprite;
    public SpriteRenderer myEnemyShadowSprite;
    public GameObject mySleepIcon;

    public List<Sprite> allEnemySprites;
    public List<Sprite> allElementIcons;

    public float myAttackColumnXValue;
    public float myAttackColumnYValue;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayUsedStatus();
    }

    private void OnMouseUpAsButton()
    {
        GetAttacked();
    }



    public void SetSprites(int elementIndex)
    {
        myElementIcon.sprite = allElementIcons[elementIndex];
        myEnemySprite.sprite = allEnemySprites[elementIndex];
        myEnemyShadowSprite.sprite = allEnemySprites[elementIndex];
    }
    
    public void GetAttacked()
    {
        if (attackPicker.resolvingAttack) { return; }
        if (used) { return; }
        if (FindObjectOfType<BattleSequence>().battleOver)
        {
            return;
        }
        attackPicker.UseAttack(this, myAttackAtributes.strength, myAttackAtributes.element);
        StartCoroutine(MoveCard());
    }

    private void DisplayUsedStatus()
    {
        if (used)
        {
            myEnemySprite.color = new Color(myEnemySprite.color.r, 1f, 1f, 0.25f);
        }
        else
        {
            myEnemySprite.color = new Color(myEnemySprite.color.r, 1f, 1f, 1f);
        }
    }

    public IEnumerator MoveCard()
    {
        float elapsed = 0f; 
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = new(transform.localPosition.x, transform.localPosition.y + moveUpAmount, transform.localPosition.z);
        while (elapsed < moveUpTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / moveUpTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
    }
}
