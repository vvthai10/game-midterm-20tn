using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDialog : MonoBehaviour
{

    public Transform gridRoot;
    public GameObject shopItemUIPb;
    public ItemDetail shopItemDetail;
    private void Start()
    {
        UpdateUi();
    }

    private void UpdateUi()
    {
        var items = ShopManager.instance.shopItems;
        
        if (items == null || items.Length <= 0 || !gridRoot || !shopItemUIPb) return;

        for (int i = 0; i < items.Length; i++)
        {
            int idx = i;
            var item = items[i];
            if(item != null)
            {
                var itemUIClone = Instantiate(shopItemUIPb, Vector3.zero, Quaternion.identity);
        
                itemUIClone.transform.SetParent(gridRoot);
                itemUIClone.transform.localScale = Vector3.one;
                itemUIClone.transform.localPosition = Vector3.zero;
                itemUIClone.GetComponent<ShopItemUI>().UpdateUI(item);
                Button btn = itemUIClone.GetComponent<ShopItemUI>().button;
                if(btn)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => { ItemEvent(item, idx);});
                }
            }

        }
    }

    private void ItemEvent(Item item, int idx)
    {
        shopItemDetail.setAttr(item, idx);
    }

}
