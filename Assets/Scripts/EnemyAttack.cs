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
    private SpriteRenderer myRenderer;

    public TextMeshProUGUI myPowerText;
    public Image myElementIcon;
    public Image myEnemySprite;
    public Image myEnemyShadowSprite;

    public List<Sprite> allEnemySprites;
    public List<Sprite> allElementIcons;
    
    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        DisplayUsedStatus();
    }

    public void SetSprites(int elementIndex)
    {
        myElementIcon.sprite = allElementIcons[elementIndex];
        myEnemySprite.sprite = allEnemySprites[elementIndex];
        myEnemyShadowSprite.sprite = allEnemySprites[elementIndex];
    }
    
    public void GetAttacked()
    {
        Debug.Log(gameObject.name + " was clicked!");
        if (used) { return; }
        if (FindObjectOfType<BattleSequence>().battleOver)
        {
            return;
        }
        attackPicker.UseAttack(this, myAttackAtributes.strength, myAttackAtributes.element);
    }

    private void DisplayUsedStatus()
    {
        if (used)
        {
            myEnemySprite.color = new Color(myEnemySprite.color.r, myEnemySprite.color.g, myEnemySprite.color.b, 0.5f);
        }
        else
        {
            myEnemySprite.color = new Color(myEnemySprite.color.r, myEnemySprite.color.g, myEnemySprite.color.b, 1f);
        }
    }
}
