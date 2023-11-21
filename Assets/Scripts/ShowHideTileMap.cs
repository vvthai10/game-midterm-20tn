using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideTileMap : MonoBehaviour
{
    public static ShowHideTileMap Instance;

    void Start () {
        if(ShowHideTileMap.Instance == null) {
            Instance = this;
        }
    }

    public GameObject TileMapNormal, TileMapBoss;

    public void TurnNormal() {
        TileMapNormal.SetActive(true);
        TileMapBoss.SetActive(false);
    }
    
    public void TurnBoss() {
        TileMapNormal.SetActive(false);
        TileMapBoss.SetActive(true);
    }
}
