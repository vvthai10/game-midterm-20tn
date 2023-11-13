using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public Item[] shopItems;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
}
