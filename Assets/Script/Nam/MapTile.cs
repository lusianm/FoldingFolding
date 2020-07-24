using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public int currX;
    public int currY;
    public TILE_TYPE myTileType;

    public void Set_Index(int x, int y)
    {
        currX = x;
        currY = y;
    }

    private void OnMouseDown()
    {
        if (MapManager.instance == null) return;

        MapManager.instance.Get_TileIndex(this);
    }
}
