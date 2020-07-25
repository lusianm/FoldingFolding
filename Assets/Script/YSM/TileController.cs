using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileController : MonoBehaviour
{
    public static TileController instance = null;
    //[HideInInspector] public List<Tile> selectedTile = new List<Tile>();
    [HideInInspector] public List<MapTile> selectedTile = new List<MapTile>();
    [HideInInspector] public bool isTileSelected = false;
    [HideInInspector] public bool onMouseClick = false;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 endPos;
    [HideInInspector] public float _tilePadValue = 0.33f;
    bool isRotating = false;
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    void FixedUpdate()
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
        }
        if (Input.GetMouseButtonUp(0))
        {
            onMouseClick = false;
        }
    }

    IEnumerator RotateObject(PlayerDirection dir)
    {
        //Tile lastSelectedTile = selectedTile[selectedTile.Count - 1];
        //Tile firstSelectedTile = selectedTile[0];
        MapTile lastSelectedTile = selectedTile[selectedTile.Count - 1];
        MapTile firstSelectedTile = selectedTile[0];
        List<GameObject> copiedTile = new List<GameObject>();
        Vector2 playerIndex = Player.playerInstance.GetPlayerCoordinate;
        int playerIndexList = 0;
        bool tileHasPlayer = false;
        for (int i = 0; i < selectedTile.Count; i++)
        {
            copiedTile.Add(Instantiate(selectedTile[i].gameObject));
            if(playerIndex.x==selectedTile[i].currX && playerIndex.y == selectedTile[i].currY)
            {
                playerIndexList = i;
                tileHasPlayer = true;
            }
                //Tile tile = copiedTile[i].GetComponent<Tile>();
                MapTile tile = copiedTile[i].GetComponent<MapTile>();
            if (tile != null)
            {
                tile.SetTileInfo(selectedTile[i]);
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
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, -angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, -angle);
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, -angle);
                        }                            
                    }
                    break;
                case PlayerDirection.Down:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.currY > lastSelectedTile.currY)
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, angle);
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, -firstSelectedTile.transform.right, angle);
                        }                            
                        else
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, angle);
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, -lastSelectedTile.transform.right, angle);
                        }                            
                    }
                    break;
                case PlayerDirection.Left:
                    for (int i = 0; i < selectedTile.Count; i++)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                        }
                    }
                    break;
                case PlayerDirection.Right:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.currX < lastSelectedTile.currX)
                        {
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                            Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                        }
                        else
                        {
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                            Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                        }
                            
                    }
                    break;
            }

            yield return null;
        }

        isRotating = false;
        bool backRoate = false;
        //List<Tile> hittedTileList = new List<Tile>();
        List<MapTile> hittedTileList = new List<MapTile>();
        for (int i = 0; i < copiedTile.Count; i++)
        {
            //Tile tile = copiedTile[i].GetComponent<Tile>();
            MapTile tile = copiedTile[i].GetComponent<MapTile>();
            RaycastHit hit;
            Debug.DrawRay(copiedTile[i].transform.position + Vector3.forward, Vector3.back, Color.red, 2f);
            if (Physics.Raycast(copiedTile[i].transform.position + Vector3.forward, Vector3.back * 2, out hit, 2))
            {
                //Tile hitTile = hit.transform.GetComponent<Tile>();
                MapTile hitTile = hit.transform.GetComponent<MapTile>();
                if (hitTile != null)
                {
                    //if (hitTile.tileValue == TileValue.BLACK || hitTile.tileValue == TileValue.YELLOW)
                    if (hitTile.myTileType == TILE_TYPE.검정_변형불가 || hitTile.myTileType == TILE_TYPE.노랑_톱니로부숴)
                    {
                        backRoate = true;
                        break;
                    }
                    hittedTileList.Add(hitTile);
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
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, -angle);
                            }
                                
                            else
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, -angle);
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, -angle);
                            }
                        }
                        break;
                    case PlayerDirection.Down:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.currY > lastSelectedTile.currY)
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, angle);
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * _tilePadValue / 2f, firstSelectedTile.transform.right, angle);
                            }

                            else
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, angle);
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * _tilePadValue / 2f, lastSelectedTile.transform.right, angle);
                            }
                        }
                        break;
                    case PlayerDirection.Left:
                        for (int i = 0; i < selectedTile.Count; i++)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * _tilePadValue / 2f, -firstSelectedTile.transform.up, angle);
                            }
                                
                            else
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * _tilePadValue / 2f, -lastSelectedTile.transform.up, angle);
                            }
                                
                        }
                        break;
                    case PlayerDirection.Right:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.currX < lastSelectedTile.currX)
                            {
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                                Player.playerInstance.transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * _tilePadValue / 2f, lastSelectedTile.transform.up, angle);
                            }
                                
                            else
                            {
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
                                Player.playerInstance.transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * _tilePadValue / 2f, firstSelectedTile.transform.up, angle);
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
                //hittedTileList[i].SetTileInfo(copiedTile[i].GetComponent<Tile>());
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
    }

}
