using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player가 밟고 있는 칸의 좌표
    Vector2 playerPosition;
    Vector2[] directionVector = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float inputDelay = 1f;

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

    public bool IsIdle()
        => (playerState == PlayerState.Idle);
    public bool IsCrouching()
        => (playerState == PlayerState.Crouching);
    public bool IsMovable()
        => IsIdle() || IsCrouching();
    public bool IsPlyerXFlip => spriteRenderer.flipX;
    public bool IsPlyerYFlip => spriteRenderer.flipY;

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
    private Vector2 GravityDirectionCorrectionVector2()
    {
        Vector2 correctionVector = Vector2.zero;
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

    //Player의 이동처리 함수
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
                Debug.Log("Idle X>0 Move");

                int directionIndex = ((int)playerGravityDirection - 1 + 4) % 4;
                //if(Map.MoveCheck( playerPosition, directionVector[directionIndex]))
                //이동이 가능할 경우
                {
                    //playerPosition += directionVector[directionIndex]
                    //transform.position = Map.MapPosition(playerPosition)
                    //                      +GravityDirectionCorrectionVector2()
                    //spriteRenderer.flipX = false;
                }

                //else
                //이동이 불가능 할 경우
                {
                    playerGravityDirection = playerDirectionEnum[directionIndex];
                    transform.Rotate(new Vector3(0, 0, 90));
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                    //spriteRenderer.flipX = false;
                }
            }
            else
            {
                Debug.Log("Idle X<0 Move");
                int directionIndex = ((int)playerGravityDirection + 1) % 4;
                //if(Map.MoveCheck( playerPosition, directionVector[directionIndex]))
                //이동이 가능할 경우
                {
                    //playerPosition += directionVector[3]
                    //transform.position = Map.MapPosition(playerPosition)
                    //                      +GravityDirectionCorrectionVector2()
                    //spriteRenderer.flipX = false;
                }

                //else
                //이동이 불가능 할 경우
                {
                    playerGravityDirection = playerDirectionEnum[directionIndex];
                    transform.Rotate(new Vector3(0, 0, -90));
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                    //spriteRenderer.flipX = false;
                }
            }
        }
        else
        {

            if (convertedXAxis > 0)
            {
                int directionIndex = ((int)playerGravityDirection - 1 + 4) % 4;
                //공간이 비어 있으면
                //if(Map.MoveCheck2( playerPosition, directionVector[directionIndex]))
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, -90));
                    //transform.position = Map.MapPosition(playerPosition)
                    //                      +GravityDirectionCorrectionVector2()
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
                //공간이 비어 있으면
                //if(Map.MoveCheck2( playerPosition, directionVector[directionIndex]))
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, 90));
                    //transform.position = Map.MapPosition(playerPosition)
                    //                      +GravityDirectionCorrectionVector2()
                    //spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());

                }

                //else
                { }

            }
        }

    }

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

    IEnumerator MovingTime()
    {
        yield return new WaitForSeconds(inputDelay);
        if (playerState == PlayerState.Move)
            playerState = PlayerState.Idle;
    }
}
