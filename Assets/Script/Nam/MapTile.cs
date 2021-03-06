﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MapTile : MonoBehaviour
{
    public int currX;
    public int currY;
    public TILE_TYPE myTileType;
    bool isSelected = false;
    public Sprite originSprite;
    Color originColor;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originSprite = spriteRenderer.sprite;
        originColor = spriteRenderer.color;
    }

    public void Set_Index(int x, int y)
    {
        currX = x;
        currY = y;
    }

    private void OnMouseDown()
    {
        if (MapManager.instance == null || UIController.instance.opneTutorial==true || UIController.instance._menuPanel.activeInHierarchy ) return;

        MapManager.instance.Get_TileIndex(this);
        TileController.instance.onMouseClick = true;
        TileController.instance.isTileSelected = true;
    }
    private void OnMouseOver()
    {
        if (TileController.instance.onMouseClick == true && isSelected == false)
        {
            isSelected = true;
            spriteRenderer.color = new Color(0.7f, 1f, 0.2f);
            TileController.instance.selectedTile.Add(this);
        }
    }

    private void OnMouseUp()
    {
        TileController.instance.onMouseClick = false;
    }

    public void SetTileInfo(MapTile tile)
    {
        //tileValue = tile.tileValue;
        myTileType = tile.myTileType;
        spriteRenderer.sprite = tile.originSprite;
        originSprite = spriteRenderer.sprite;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag!="Finish")
                Destroy(transform.GetChild(i).gameObject);
        }
        for (int i=0;i<tile.transform.childCount;i++)
        {
            if(tile.transform.GetChild(i).tag!="Finish")
                tile.transform.GetChild(i).parent = transform;
        }
    }

    public void CopyTileInfo(MapTile tile)
    {
        //tileValue = tile.tileValue;
        myTileType = tile.myTileType;
        spriteRenderer.sprite = tile.originSprite;
        originSprite = spriteRenderer.sprite;
    }

    public void UnSelectTile()
    {
        spriteRenderer.sprite = originSprite;
        spriteRenderer.color = originColor;
        isSelected = false;
    }
}
