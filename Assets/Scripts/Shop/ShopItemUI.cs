using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image bg;
    public Button button;
    public GameObject text;

    public void UpdateUI(Item item)
    {
        if (item == null) return;
        if(bg == null)
        {
            return;
        }
        bg.sprite = item.hud;

    }
}
