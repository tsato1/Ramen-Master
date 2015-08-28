using UnityEngine;
using System.Collections;

public enum ItemName
{
    Apple,
    Banana,
    Bear,
    Chasyu,
    Coffee,
    Corn,
    Crown,
    Cucumber,
    Egg,
    Leek,
    Memma,
    Pencil,
    Pikachu,
    Seaweed,
    Sunglasses,
    Tabacco,
    Toiletpaper,
    Tomato,
    Toothbrush,
    Volcano,
    Wine,
    Girl
}

[System.Serializable]
public class Item : MonoBehaviour {
    public ItemName itemName;
    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int score;
    public string description;
}
