using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThornInit(Vector2 coordinate, int direction)
    {
        transform.position = MapManager.instance.Get_MapTilePosition((int)coordinate.x, (int)coordinate.y);
        //int convertedDirection = 0;
        ////UP
        //if (direction == 0)
        //    convertedDirection = 0;
        ////Down
        //else if (direction == 1)
        //    convertedDirection = 2;
        ////Left
        //else if (direction == 2)
        //    convertedDirection = 3;
        ////Right
        //else if (direction == 3)
        //    convertedDirection = 1;


    }

    public void ThornInit(int coordinateX, int coordinateY, int direction)
    {
        transform.position = MapManager.instance.Get_MapTilePosition(coordinateX, coordinateY);
        //int convertedDirection = 0;
        ////UP
        //if (direction == 0)
        //    convertedDirection = 0;
        ////Down
        //else if (direction == 1)
        //    convertedDirection = 2;
        ////Left
        //else if (direction == 2)
        //    convertedDirection = 3;
        ////Right
        //else if (direction == 3)
        //    convertedDirection = 1;
        transform.Rotate(new Vector3(0, 0, (direction + 2) * 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("플레이어가 가시에 찔렸다.");
            collision.GetComponent<Player>().PlayerDie();
        }
    }
}
