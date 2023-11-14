using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    public Text name;
    public Text description;
    public Text price;
    public Button buyBtn;
    public ItemEffect effect;


    public void setAttr(Item item, int idx)
    {
        name.text = item.name;
        description.text = item.description; 
        price.text = "Price: " + item.price;
        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(() => { BuySuccess(main_character.instance.souls, item, idx); });
    }

    bool EnoughMoney(float curMoney, Item item) 
    {
        return curMoney >= item.price;
    }

    bool BuySuccess(float curMoney ,Item item, int idx)
    {
        Debug.Log("curMoney:" + curMoney);
        Debug.Log("item price: " + item.price);
        if (!item.HasItem()) return false;
        if (!EnoughMoney(curMoney, item)) return false;

        item.BuyItem();
        main_character.instance.BoughtItem(item.price);
        //Hieu qua khi mua cua loai thuoc
        effect.OnItemBuy(idx);
        
        return true;

    }
}
