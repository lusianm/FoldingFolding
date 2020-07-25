using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapTile : MonoBehaviour
{
    public int currX;
    public int currY;
    public TILE_TYPE myTileType;
    bool isSelected = false;
    Sprite originSprite;
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
        if (MapManager.instance == null) return;

        MapManager.instance.Get_TileIndex(this);
        TileController.instance.onMouseClick = true;
    }
    private void OnMouseOver()
    {
        if (TileController.instance.onMouseClick == true && isSelected == false)
        {
            isSelected = true;
            spriteRenderer.color = new Color(0.5f, 0.9f, 1f);
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
        for(int i=0;i<tile.transform.childCount;i++)
        {
            tile.transform.GetChild(i).parent = transform;
        }
    }

    public void UnSelectTile()
    {
        spriteRenderer.sprite = originSprite;
        spriteRenderer.color = originColor;
        isSelected = false;
    }
}
