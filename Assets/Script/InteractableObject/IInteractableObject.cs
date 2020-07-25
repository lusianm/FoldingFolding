using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObject
{
    void ObjectInit(Vector2 coordinate, int direction);

    void ObjectInit(int coordinateX, int coordinateY, int direction);
}
