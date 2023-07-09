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
    public GameObject mySleepIcon;

    public List<Sprite> allEnemySprites;
    public List<Sprite> allElementIcons;

    public float myAttackColumnXValue;
    public float myAttackColumnYValue;
    
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
            myEnemySprite.color = new Color(myEnemySprite.color.r, 1f, 1f, 0.25f);
        }
        else
        {
            myEnemySprite.color = new Color(myEnemySprite.color.r, 1f, 1f, 1f);
        }
    }

    public IEnumerator MoveCard(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPos, endPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        GetComponent<RectTransform>().localPosition = endPos;
    }
}
