using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Item 
{
    public float price;
    public float amount;
    public string name;
    public string description;
    public Sprite hud;

    public Item(float price, float amount, string name, string description, Sprite hud)
    {
        this.price = price;
        this.amount = amount;
        this.name = name;
        this.description = description;
        this.hud = hud;
    }

    public bool HasItem()
    {

        return this.amount > 0;
    }

    public void BuyItem()
    {
        this.amount -= 1;
    }
}
