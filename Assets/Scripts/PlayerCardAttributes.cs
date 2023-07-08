using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class PlayerCardAttributes : ScriptableObject
{
    public int power;
    public Element element;
    public string rules;
}

public enum Element { Fire, Water, Earth, Wind }