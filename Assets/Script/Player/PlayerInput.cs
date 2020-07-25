using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;
    float InputXAxis;
    float InputYAxis;

    void Start()
    {
        player = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 클릭 이 되지 않았을 경우 동작
        if(!TileController.instance.isTileSelected)
        {
            if (Input.GetKeyDown(KeyCode.R))
                player.PlayerDie();

            if (player.IsMovable())
            {
                InputXAxis = Input.GetAxisRaw("Horizontal");
                InputYAxis = Input.GetAxisRaw("Vertical");
                player.Move(InputXAxis, InputYAxis);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    player.Jump();
                }
            }
        }
    }
}
