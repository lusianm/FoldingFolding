using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;
    float InputXAxis;
    float InputYAxis;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 클릭 될 경우
        //if
        {
            if (player.IsMovable())
            {
                InputXAxis = Input.GetAxisRaw("Horizontal");
                InputYAxis = Input.GetAxisRaw("Vertical");
                player.Move(InputXAxis, InputYAxis);
            }
        }
    }
}
