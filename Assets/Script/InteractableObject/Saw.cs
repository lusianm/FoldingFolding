﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour, IInteractableObject
{
    //속도 = 거리/시간
    private int moveDirection;
    private Vector3 targetPosition;
    private bool IsMoving = false;
    public float moveSpeed = 3f;
    private Vector2 sawCoordinate;
    Vector2[] directionVector = { Vector2.down, Vector2.right, Vector2.up, Vector2.left };

    public void ObjectInit(Vector2 coordinate, int direction)
    {
        sawCoordinate = coordinate;
        moveDirection = direction;
        transform.position = MapManager.instance.Get_MapTilePosition((int)coordinate.x, (int)coordinate.y);
        transform.Rotate(new Vector3(0, 0, (direction + 2) * 90));
        IsMoving = true;
    }

    public void ObjectInit(int coordinateX, int coordinateY, int direction)
    {
        sawCoordinate = new Vector2(coordinateX, coordinateY);
        moveDirection = direction;
        targetPosition = MapManager.instance.Get_MapTilePosition(coordinateX, coordinateY);
        transform.position = targetPosition;
        transform.Rotate(new Vector3(0, 0, (direction + 2) * 90));
        IsMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            if (transform.position == targetPosition)
            {
                if (MapManager.instance.Get_MapTileType((int)sawCoordinate.x, (int)sawCoordinate.y) == 3)
                {
                    //타일 변경 요청
                }
                Vector2 targetCoordinate = sawCoordinate + directionVector[moveDirection];
                int targetTileType = MapManager.instance.Get_MapTileType((int)targetCoordinate.x, (int)targetCoordinate.y);
                if (targetTileType == 0 || targetTileType == 3)
                {
                    sawCoordinate = targetCoordinate;
                    targetPosition = MapManager.instance.Get_MapTilePosition((int)sawCoordinate.x, (int)sawCoordinate.y);
                }
                else
                {
                    moveDirection = (moveDirection + 2) % 4;
                }

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
                
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().PlayerDie();
        }
    }
}
