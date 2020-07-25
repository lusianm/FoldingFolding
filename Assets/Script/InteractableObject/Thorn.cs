using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour, IInteractableObject
{
    public void ObjectInit(Vector2 coordinate, int direction)
    {
        transform.position = MapManager.instance.Get_MapTilePosition((int)coordinate.x, (int)coordinate.y);
        transform.Rotate(new Vector3(0, 0, (direction + 2) * 90));
    }

    public void ObjectInit(int coordinateX, int coordinateY, int direction)
    {
        transform.position = MapManager.instance.Get_MapTilePosition(coordinateX, coordinateY);
        transform.Rotate(new Vector3(0, 0, (direction + 2) * 90));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().PlayerDie();
        }
    }
}
