using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyAttackAttributes myAttackAtributes;
    public EnemyAttackPicker attackPicker;
    public bool used;
    private SpriteRenderer myRenderer;
    
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

    private void OnMouseDown()
    {
        used = true;
        attackPicker.UseAttack(myAttackAtributes.strength, myAttackAtributes.element);
    }

    private void DisplayUsedStatus()
    {
        if (used)
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0.5f);
        }
        else
        {
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1f);
        }
    }
}
