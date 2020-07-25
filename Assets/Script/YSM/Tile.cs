using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileValue
{
    PINK, BLUE, BLACK, YELLOW
}

public class Tile : MonoBehaviour
{
    bool isSelected = false;
    Color originColor;
    SpriteRenderer sprite;
    public float x, y;
    public Tile otherTile;
    public TileValue tileValue;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originColor = sprite.color;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Click !");
        TileController.instance.onMouseClick = true;
    }
    private void OnMouseOver()
    {
        //if (TileController.instance.onMouseClick == true && isSelected == false)
        //{
        //    isSelected = true;
        //    sprite.color = new Color(0.5f, 0.9f, 1f);
        //    TileController.instance.selectedTile.Add(this);
        //}
    }

    private void OnMouseUp()
    {
        TileController.instance.onMouseClick = false;
    }

    public void SetTileInfo(Tile tile)
    {
        tileValue = tile.tileValue;
        sprite.color = tile.originColor;
        originColor = tile.originColor;

    }

    public void UnSelectTile()
    {
        sprite.color = originColor;
        isSelected = false;
    }
}
