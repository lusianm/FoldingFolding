﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileController : MonoBehaviour
{
    public static TileController instance = null;
    [HideInInspector] public List<MapTile> selectedTile = new List<MapTile>();
    [HideInInspector] public Saw saw;
    [HideInInspector] public bool isTileSelected = false;
    [HideInInspector] public bool onMouseClick = false;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 endPos;
    [HideInInspector] public float _tilePadValue = 0.33f;
    bool isRotating = false;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
            
        else
            DestroyImmediate(this);
    }

    void Update()
    {
        if (isTileSelected == true && isRotating == false) 
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //Up
            {
                StartCoroutine(RotateObject(PlayerDirection.Up));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) //Down
            {
                StartCoroutine(RotateObject(PlayerDirection.Down));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) //Left
            {
                StartCoroutine(RotateObject(PlayerDirection.Left));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) //Right
            {
                StartCoroutine(RotateObject(PlayerDirection.Right));
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                for(int i=0;i<selectedTile.Count;i++)
                {
                    selectedTile[i].UnSelectTile();
                }
                selectedTile.Clear();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            onMouseClick = false;
        }
    }

    IEnumerator RotateObject(PlayerDirection dir)
    {
        MapTile lastSelectedTile = selectedTile[selectedTile.Count - 1];
        MapTile firstSelectedTile = selectedTile[0];
        List<GameObject> copiedTile = new List<GameObject>();
        Vector2 playerIndex = Player.playerInstance.GetPlayerCoordinate;
        int playerIndexList = 0;
        bool tileHasPlayer = false;
        if (saw != null)
        {
            saw.IsMoving = false;
        }
        for (int i = 0; i < selectedTile.Count; i++)
        {
            copiedTile.Add(Instantiate(selectedTile[i].gameObject));
            if(playerIndex.x==selectedTile[i].currX && playerIndex.y == selectedTile[i].currY)
            {
                playerIndexList = i;
                tileHasPlayer = true;
            }
            for(int j=0;j<copiedTile[i].transform.childCount;j++)
            {
                if (copiedTile[i].transform.GetChild(j).tag == "Finish")
                    Destroy(copiedTile[i].transform.GetChild(j).gameObject);
            }
            MapTile tile = copiedTile[i].GetComponent<MapTile>();
            if (tile != null)
            {
                tile.CopyTileInfo(selectedTile[i]);
            }
        }

        for (int i = 0; i < selectedTile.Count; i++)
        {
            selectedTile[i].UnSelectTile();
        }
        float angle = 0.0f;
        float lastAngle = 0.0f;
        isRotating = true;

        //넘어가는 부분.
        while (lastAngle < 180f)
        {
            angle = 180 * 0.05f;
            lastAngle += angle;

            switch (dir)
            {
                case PlayerDirection.Up:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, -angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, -angle);
                         }
                    }
                    if (tileHasPlayer)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {

                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, -angle);
                        }
                        else
                        {
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, -angle);
                        }
                    }
                    if(saw!=null)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {

                            saw.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, -angle);
                        }
                        else
                        {
                            saw.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, -angle);
                        }
                    }
                    break;
                    
                case PlayerDirection.Down:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, angle);
                        }                            
                        else
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, angle);
                        }
                    }
                    if (tileHasPlayer)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, angle);
                        }
                        else
                        {
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, angle);
                        }
                    }
                    if(saw!=null)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {
                            saw.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, angle);
                        }
                        else
                        {
                            saw.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, angle);
                        }
                    }
                    break;
                case PlayerDirection.Left:
                    for (int i = 0; i < selectedTile.Count; i++)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                        }
                    }
                    if (tileHasPlayer)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                        }
                    }
                    if (saw != null)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            saw.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            saw.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                        }
                    }
                    break;
                case PlayerDirection.Right:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                        }
                    }
                    if (tileHasPlayer)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                        }
                    }
                    if (saw != null)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            saw.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            saw.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                        }
                    }
                    break;
            }

            yield return null;
        }

        isRotating = false;
        bool backRoate = false;
        List<MapTile> hittedTileList = new List<MapTile>();
        for (int i = 0; i < copiedTile.Count; i++)
        {
            MapTile tile = copiedTile[i].GetComponent<MapTile>();
            RaycastHit hit;
            Debug.DrawRay(copiedTile[i].transform.position + Vector3.forward, Vector3.back, Color.red, 2f);
            if (Physics.Raycast(copiedTile[i].transform.position + Vector3.forward, Vector3.back * 2, out hit, 2))
            {
                MapTile hitTile = hit.transform.GetComponent<MapTile>();
                if (hitTile != null)
                {
                    for (int j = 0; j < hitTile.transform.childCount; j++)
                    {
                        if (hitTile.transform.GetChild(j).tag == "Finish")
                            backRoate = true;
                    }
                    if (backRoate == true) break;
                    if (hitTile.myTileType == TILE_TYPE.검정_변형불가 || hitTile.myTileType == TILE_TYPE.노랑_톱니로부숴)
                    {
                        backRoate = true;
                        break;
                    }
                    hittedTileList.Add(hitTile);
                }
                else
                {
                    backRoate = true;
                }
            }
            else
            {
                Debug.Log("No hit!");
            }
        }

        //되돌아 가는 부분.
        if (backRoate == true)
        {
            Debug.Log("Back!!");
            angle = 0.0f;
            lastAngle = 0.0f;
            while (lastAngle < 180f)
            {
                angle = 180 * 0.05f;
                lastAngle += angle;

                switch (dir)
                {
                    case PlayerDirection.Up:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, -angle);
                            }
                                
                            else
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, -angle);
                            }
                        }
                        if (tileHasPlayer)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, -angle);
                            }

                            else
                            {
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, -angle);
                            }
                        }
                        if (saw != null)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                saw.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, -angle);
                            }

                            else
                            {
                                saw.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, -angle);
                            }
                        }
                        break;
                    case PlayerDirection.Down:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, angle);
                            }

                            else
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, angle);
                            }
                        }
                        if (tileHasPlayer)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, angle);
                            }

                            else
                            {
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, angle);
                            }
                        }
                        if (saw != null)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                saw.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, angle);
                            }

                            else
                            {
                                saw.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, angle);
                            }
                        }
                        break;
                    case PlayerDirection.Left:
                        for (int i = 0; i < selectedTile.Count; i++)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                            }
                                
                            else
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                            }
                        }
                        if (tileHasPlayer)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                            }

                            else
                            {
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                            }
                        }
                        if (saw != null)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                saw.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                            }

                            else
                            {
                                saw.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                            }
                        }
                        break;
                    case PlayerDirection.Right:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                            }
                            else
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                            }
                        }
                        if (tileHasPlayer)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                            }
                            else
                            {
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                            }
                        }
                        if (saw != null)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                saw.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                            }
                            else
                            {
                                saw.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                            }
                        }
                        break;
                }

                yield return null;
            }
        }
        else
        {
            
            for (int i = 0; i < hittedTileList.Count; i++)
            {
                hittedTileList[i].SetTileInfo(copiedTile[i].GetComponent<MapTile>());
            }
            Vector2 newPlayerIndex = new Vector2(hittedTileList[playerIndexList].currX, hittedTileList[playerIndexList].currY);
            if (tileHasPlayer == true) Player.playerInstance.playerFolding(newPlayerIndex, (int)dir);
        }

        selectedTile.Clear();
        int loopCount = copiedTile.Count;
        for (int i = 0; i < loopCount; i++)
        {
            GameObject obj = copiedTile[0];
            copiedTile.RemoveAt(0);
            Destroy(obj);
        }
        isTileSelected = false;

        if (saw != null)
        {
            saw.IsMoving = true;
        }
    }

}
