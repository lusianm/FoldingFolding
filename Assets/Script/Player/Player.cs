using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player의 칸의 좌표
    Vector2 playerPosition;
    Vector2[] directionVector = { Vector2.down, Vector2.right, Vector2.up, Vector2.left };
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float inputDelay = 1f;
    [SerializeField] Vector2 PlayerInitialSetPosition;
    public enum PlayerDirection
    {
        Up = 0, Right = 1, Down = 2, Left = 3
    }
    PlayerDirection[] playerDirectionEnum = { PlayerDirection.Up, PlayerDirection.Right, PlayerDirection.Down, PlayerDirection.Left };
    [SerializeField] PlayerDirection playerGravityDirection;
    public enum PlayerState
    {
        Idle, Move, Crouching, Jump, Falling, DIe, Damaged, StageClear
    }
    [SerializeField] PlayerState playerState;

    //Player가 밟고 있는 칸의 좌표
    [SerializeField] public Vector2 playerGroundPosition => playerPosition + directionVector[(int)playerGravityDirection];

    //Player의 상태를 확인하는 함수
    public bool IsIdle()
        => (playerState == PlayerState.Idle);
    public bool IsCrouching()
        => (playerState == PlayerState.Crouching);

    //Player가 움직일 수 있는지 여부를 반환하는 함수
    public bool IsMovable()
        => IsIdle() || IsCrouching();

    //Player의 Sprite의 X 반전 여부를 반환하는 함수
    public bool IsPlyerXFlip => spriteRenderer.flipX;

    //Gravity의 방향에 따라 Input값을 Player기준으로 변환시켜주는 함수
    private float MoveDirectionConvert(float xAxis, float yAxis)
    {
        
        if (xAxis == 0 && yAxis == 0)
        {
            if (IsCrouching())
                playerState = PlayerState.Idle;
            return 0;
        }

        switch (playerGravityDirection)
        {
            case PlayerDirection.Up:
                {
                    if (yAxis > 0 && IsIdle())
                    {
                        playerState = PlayerState.Crouching;
                    }
                    else if (yAxis <= 0 && IsCrouching())
                    {
                        playerState = PlayerState.Idle;
                    }
                    return xAxis * (-1);
                }
            case PlayerDirection.Right:
                {
                    if (xAxis > 0 && IsIdle())
                    {
                        playerState = PlayerState.Crouching;
                    }
                    else if (xAxis <= 0 && IsCrouching())
                    {
                        playerState = PlayerState.Idle;
                    }
                    return yAxis;
                }
            case PlayerDirection.Down:
                {
                    if (yAxis < 0 && IsIdle())
                    {
                        playerState = PlayerState.Crouching;
                    }
                    else if (yAxis >= 0 && IsCrouching())
                    {
                        playerState = PlayerState.Idle;
                    }
                    return xAxis;
                }
            case PlayerDirection.Left:
                {
                    if (xAxis < 0 && IsIdle())
                    {
                        playerState = PlayerState.Crouching;
                    }
                    else if (xAxis >= 0 && IsCrouching())
                    {
                        playerState = PlayerState.Idle;
                    }
                    return yAxis * (-1);
                }
        }
        Debug.Log("Player Gravity Direction is wrong");
        return 0;
    }

    //Gravity의 방향에 따라 Player의 Position값을 보정해주는 함수
    private Vector3 GravityDirectionCorrectionVector()
    {
        Vector3 correctionVector = Vector3.zero;
        correctionVector.z = transform.position.z;
        switch (playerGravityDirection)
        {
            case PlayerDirection.Up:
                {
                    correctionVector.x = 0;
                    correctionVector.y = -0.5f;
                    break;
                }
            case PlayerDirection.Right:
                {
                    correctionVector.x = -0.5f;
                    correctionVector.y = 0;
                    break;
                }
            case PlayerDirection.Down:
                {
                    correctionVector.x = 0;
                    correctionVector.y = 0.5f;
                    break;
                }
            case PlayerDirection.Left:
                {
                    correctionVector.x = 0.5f;
                    correctionVector.y = 0;
                    break;
                }
        }

        return correctionVector;

    }

    //Player의 이동 처리 함수
    public void Move(float xAxis, float yAxis)
    {
        if (!IsMovable())
            return;

        float convertedXAxis = MoveDirectionConvert(xAxis, yAxis);

        if (convertedXAxis == 0)
            return;

        if (IsIdle())
        {
            if (convertedXAxis > 0)
            {
                int directionIndex = ((int)playerGravityDirection - 1 + 4) % 4;
                Vector2 targetPosition = playerPosition + directionVector[directionIndex];
                Debug.Log("Idle X>0 Move - TargetPosition : " + targetPosition);
                Debug.Log("Idle X>0 Move - directionVector : " + directionVector[directionIndex]);

                //이동이 가능할 경우
                if (MapManager.instance.Get_MapTileType((int)targetPosition.x, (int)targetPosition.y) == 0)
                {
                    playerPosition = targetPosition;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerPosition.x, (int)playerPosition.y);
                    //spriteRenderer.flipX = false;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }

                //이동이 불가능 할 경우
                else
                {
                    playerGravityDirection = playerDirectionEnum[directionIndex];
                    transform.Rotate(new Vector3(0, 0, 90));
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }
            }
            else
            {
                int directionIndex = ((int)playerGravityDirection + 1) % 4;
                Vector2 targetPosition = playerPosition + directionVector[directionIndex];
                Debug.Log("Idle X<0 Move - TargetPosition : " + targetPosition);
                Debug.Log("Idle X<0 Move - directionVector : " + directionVector[directionIndex]);

                //이동이 가능할 경우
                if (MapManager.instance.Get_MapTileType((int)targetPosition.x, (int)targetPosition.y) == 0)
                {
                    playerPosition = targetPosition;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerPosition.x, (int)playerPosition.y);
                    //transform.position = MapManager.instance.Get_MapTilePosition((int)playerGroundPosition.x, (int)playerGroundPosition.y) + GravityDirectionCorrectionVector();
                    //spriteRenderer.flipX = false;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }

                //이동이 불가능 할 경우
                else
                {
                    playerGravityDirection = playerDirectionEnum[directionIndex];
                    transform.Rotate(new Vector3(0, 0, -90));
                    //spriteRenderer.flipX = false;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }
            }
        }

        //웅크리고 있는 상태
        else
        {
            if (convertedXAxis > 0)
            {
                int directionIndex = ((int)playerGravityDirection - 1 + 4) % 4;
                Vector2 targetPosition = playerPosition + directionVector[directionIndex];
                Vector2 targetGroundPosition = playerGroundPosition + directionVector[directionIndex];
                Debug.Log("Crunch X>0 Move - TargetPosition : " + targetPosition);
                //공간이 비어 있으면
                if (MapManager.instance.Get_MapTileType((int)targetPosition.x, (int)targetPosition.y) == 0
                    && MapManager.instance.Get_MapTileType((int)targetGroundPosition.x, (int)targetGroundPosition.y) == 0)
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, -90));
                    playerPosition = targetGroundPosition;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerPosition.x, (int)playerPosition.y);
                    //transform.position = MapManager.instance.Get_MapTilePosition((int)playerGroundPosition.x, (int)playerGroundPosition.y) + GravityDirectionCorrectionVector();
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }

                //else
                { }
            }
            else
            {
                int directionIndex = ((int)playerGravityDirection + 1) % 4;
                Vector2 targetPosition = playerPosition + directionVector[directionIndex];
                Vector2 targetGroundPosition = playerGroundPosition + directionVector[directionIndex];
                Debug.Log("Crunch X<0 Move - TargetPosition : " + targetPosition);
                //공간이 비어 있으면
                if (MapManager.instance.Get_MapTileType((int)targetPosition.x, (int)targetPosition.y) == 0
                    && MapManager.instance.Get_MapTileType((int)targetGroundPosition.x, (int)targetGroundPosition.y) == 0)
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, 90));
                    playerPosition = targetGroundPosition;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerPosition.x, (int)playerPosition.y);
                    //transform.position = MapManager.instance.Get_MapTilePosition((int)playerGroundPosition.x, (int)playerGroundPosition.y) + GravityDirectionCorrectionVector();
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());

                }

                //else
                { }

            }
        }

    }

    //Player의 점프 처리 함수
    public void Jump()
    {
        //if (Map.MoveCheck(playerPosition, directionVector[(playerGravityDirection + 2 + 4) % 4]))
        {
            playerGravityDirection = playerDirectionEnum[((int)playerGravityDirection + 2) % 4];
            transform.Rotate(new Vector3(0, 0, 180));
            //spriteRenderer.flipX = !IsPlayerXFilp;
            playerState = PlayerState.Move;
            StartCoroutine(MovingTime());
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer = this.GetComponent<SpriteRenderer>();
        //animator = this.GetComponent<Animator>();
        playerState = PlayerState.Idle;
        playerGravityDirection = PlayerDirection.Down;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayer()
    {
        playerPosition = PlayerInitialSetPosition;
        playerGravityDirection = PlayerDirection.Down;
        playerState = PlayerState.Idle;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MapManager.instance.Get_MapTilePosition((int)playerPosition.x, (int)playerPosition.y);
    }

    IEnumerator MovingTime()
    {
        yield return new WaitForSeconds(inputDelay);
        if (playerState == PlayerState.Move)
            playerState = PlayerState.Idle;
    }
}
