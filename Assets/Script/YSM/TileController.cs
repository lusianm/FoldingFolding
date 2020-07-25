using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    UP, DOWN, LEFT, RIGHT
}
public class TileController : MonoBehaviour
{
    public static TileController instance = null;
    [HideInInspector] public List<Tile> selectedTile = new List<Tile>();
    [HideInInspector] public bool onMouseClick = false;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 endPos;
    bool isRotating = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (selectedTile.Count > 0 && isRotating == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) //Up
            {
                StartCoroutine(RotateObject(Direction.UP));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) //Down
            {
                StartCoroutine(RotateObject(Direction.DOWN));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) //Left
            {
                StartCoroutine(RotateObject(Direction.LEFT));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) //Right
            {
                StartCoroutine(RotateObject(Direction.RIGHT));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            onMouseClick = false;
        }
    }

    IEnumerator RotateObject(Direction dir)
    {
        Tile lastSelectedTile = selectedTile[selectedTile.Count - 1];
        Tile firstSelectedTile = selectedTile[0];
        List<GameObject> copiedTile = new List<GameObject>();

        for (int i = 0; i < selectedTile.Count; i++)
        {
            copiedTile.Add(Instantiate(selectedTile[i].gameObject));
            Tile tile = copiedTile[i].GetComponent<Tile>();
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
                case Direction.UP:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.y > lastSelectedTile.y)
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * 0.07f, -lastSelectedTile.transform.right, -angle);
                        else
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * 0.07f, -firstSelectedTile.transform.right, -angle);
                    }
                    break;
                case Direction.DOWN:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.y > lastSelectedTile.y)
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * 0.07f, -firstSelectedTile.transform.right, angle);
                        else
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * 0.07f, -lastSelectedTile.transform.right, angle);
                    }
                    break;
                case Direction.LEFT:
                    for (int i = 0; i < selectedTile.Count; i++)
                    {
                        if (firstSelectedTile.x < lastSelectedTile.x)
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * 0.07f, firstSelectedTile.transform.up, angle);
                        else
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * 0.07f, lastSelectedTile.transform.up, angle);
                    }
                    break;
                case Direction.RIGHT:
                    for (int i = 0; i < copiedTile.Count; i++)
                    {
                        if (firstSelectedTile.x < lastSelectedTile.x)
                            copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * 0.07f, -lastSelectedTile.transform.up, angle);
                        else
                            copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * 0.07f, -firstSelectedTile.transform.up, angle);
                    }
                    break;
            }

            yield return null;
        }

        isRotating = false;
        bool backRoate = false;
        List<Tile> hittedTileList = new List<Tile>();
        for (int i = 0; i < copiedTile.Count; i++)
        {
            Tile tile = copiedTile[i].GetComponent<Tile>();
            RaycastHit hit;
            Debug.DrawRay(copiedTile[i].transform.position + Vector3.forward, Vector3.back, Color.red, 2f);
            if (Physics.Raycast(copiedTile[i].transform.position + Vector3.forward, Vector3.back * 2, out hit, 2))
            {
                Tile hitTile = hit.transform.GetComponent<Tile>();
                if (hitTile != null)
                {
                    Debug.Log(hitTile.name + " -> " + hitTile.tileValue);
                    if (hitTile.tileValue == TileValue.BLACK || hitTile.tileValue == TileValue.YELLOW)
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
                    case Direction.UP:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.y > lastSelectedTile.y)
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.up * 0.07f, lastSelectedTile.transform.right, -angle);
                            else
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.up * 0.07f, firstSelectedTile.transform.right, -angle);
                        }
                        break;
                    case Direction.DOWN:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.y > lastSelectedTile.y)
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.up * 0.07f, firstSelectedTile.transform.right, angle);
                            else
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.up * 0.07f, lastSelectedTile.transform.right, angle);
                        }
                        break;
                    case Direction.LEFT:
                        for (int i = 0; i < selectedTile.Count; i++)
                        {
                            if (firstSelectedTile.x < lastSelectedTile.x)
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position - firstSelectedTile.transform.right * 0.07f, -firstSelectedTile.transform.up, angle);
                            else
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position - lastSelectedTile.transform.right * 0.07f, -lastSelectedTile.transform.up, angle);
                        }
                        break;
                    case Direction.RIGHT:
                        for (int i = 0; i < copiedTile.Count; i++)
                        {
                            if (firstSelectedTile.x < lastSelectedTile.x)
                                copiedTile[i].transform.RotateAround(lastSelectedTile.transform.position + lastSelectedTile.transform.right * 0.07f, lastSelectedTile.transform.up, angle);
                            else
                                copiedTile[i].transform.RotateAround(firstSelectedTile.transform.position + firstSelectedTile.transform.right * 0.07f, firstSelectedTile.transform.up, angle);
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
                hittedTileList[i].SetTileInfo(copiedTile[i].GetComponent<Tile>());
            }

        }

        selectedTile.Clear();
        int loopCount = copiedTile.Count;
        for (int i = 0; i < loopCount; i++)
        {
            GameObject obj = copiedTile[0];
            copiedTile.RemoveAt(0);
            Destroy(obj);
        }
    }

}
