using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerDirection
{
    Up = 0, Right = 1, Down = 2, Left = 3
}
public class Player : MonoBehaviour
{
    PlayerDirection[] playerDirectionEnum = { PlayerDirection.Up, PlayerDirection.Right, PlayerDirection.Down, PlayerDirection.Left };
    [SerializeField] PlayerDirection playerGravityDirection;
    public enum PlayerState
    {
        Idle, Move, Crouching, Jump, Falling, DIe, Damaged, StageClear
    }
    [SerializeField] PlayerState playerState;

    //Player의 칸의 좌표
    [SerializeField] Vector2 playerCoordinate;
    Vector2[] directionVector = { Vector2.down, Vector2.right, Vector2.up, Vector2.left };
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Animator playerAnimator;
    [SerializeField] private float inputDelay = 1f;
    [SerializeField] private float fallingSpeed = 1f;
    [SerializeField] Vector2 PlayerInitialSetCoordinate;

    public static Player playerInstance;
    private void Awake()
    {
        playerInstance = this;
    }


    //Player가 밟고 있는 칸의 좌표
    [SerializeField] public Vector2 playerGroundCoordinate => playerCoordinate + directionVector[(int)playerGravityDirection];

    //Player의 상태를 확인하는 함수
    public bool IsIdle()
        => (playerState == PlayerState.Idle);
    public bool IsCrouching()
        => (playerState == PlayerState.Crouching);

    //Player가 움직일 수 있는지 여부를 반환하는 함수
    public bool IsMovable()
        => IsIdle() || IsCrouching();

    //Player의 Sprite의 X 반전 여부를 반환하는 함수
    public bool IsPlayerXFlip => spriteRenderer.flipX;

    //Player의 좌표값(Index)를 반환하는 함수
    public Vector2 GetPlayerCoordinate => playerCoordinate;

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

    //Player의 이동 처리 함수
    public void Move(float xAxis, float yAxis)
    {
        if (!IsMovable())
            return;

        float convertedXAxis = MoveDirectionConvert(xAxis, yAxis);
        playerAnimator.SetBool("IsCrouching", IsCrouching());

        if (convertedXAxis == 0)
            return;

        if (IsIdle())
        {
            if (convertedXAxis > 0)
            {
                int directionIndex = ((int)playerGravityDirection - 1 + 4) % 4;
                Vector2 targetCoordinate = playerCoordinate + directionVector[directionIndex];
                Debug.Log("Idle X>0 Move - TargetPosition : " + targetCoordinate);
                Debug.Log("Idle X>0 Move - directionVector : " + directionVector[directionIndex]);

                //이동이 가능할 경우
                if (MapManager.instance.Get_MapTileType((int)targetCoordinate.x, (int)targetCoordinate.y) == 0)
                {
                    playerCoordinate = targetCoordinate;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
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
                Vector2 targetCoordinate = playerCoordinate + directionVector[directionIndex];
                Debug.Log("Idle X<0 Move - TargetPosition : " + targetCoordinate);
                Debug.Log("Idle X<0 Move - directionVector : " + directionVector[directionIndex]);

                //이동이 가능할 경우
                if (MapManager.instance.Get_MapTileType((int)targetCoordinate.x, (int)targetCoordinate.y) == 0)
                {
                    playerCoordinate = targetCoordinate;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
                    spriteRenderer.flipX = false;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }

                //이동이 불가능 할 경우
                else
                {
                    playerGravityDirection = playerDirectionEnum[directionIndex];
                    transform.Rotate(new Vector3(0, 0, -90));
                    spriteRenderer.flipX = false;
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
                Vector2 targetCoordinate = playerCoordinate + directionVector[directionIndex];
                Vector2 targetGroundCoordinate = playerGroundCoordinate + directionVector[directionIndex];
                Debug.Log("Crunch X>0 Move - TargetPosition : " + targetCoordinate);
                //공간이 비어 있으면
                if (MapManager.instance.Get_MapTileType((int)targetCoordinate.x, (int)targetCoordinate.y) == 0
                    && MapManager.instance.Get_MapTileType((int)targetGroundCoordinate.x, (int)targetGroundCoordinate.y) == 0)
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, -90));
                    playerCoordinate = targetGroundCoordinate;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
                    spriteRenderer.flipX = true;
                    playerState = PlayerState.Move;
                    StartCoroutine(MovingTime());
                }

                //else
                { }
            }
            else
            {
                int directionIndex = ((int)playerGravityDirection + 1) % 4;
                Vector2 targetCoordinate = playerCoordinate + directionVector[directionIndex];
                Vector2 targetGroundCoordinate = playerGroundCoordinate + directionVector[directionIndex];
                Debug.Log("Crunch X<0 Move - TargetPosition : " + targetCoordinate);
                //공간이 비어 있으면
                if (MapManager.instance.Get_MapTileType((int)targetCoordinate.x, (int)targetCoordinate.y) == 0
                    && MapManager.instance.Get_MapTileType((int)targetGroundCoordinate.x, (int)targetGroundCoordinate.y) == 0)
                {
                    playerGravityDirection = playerDirectionEnum[(directionIndex + 2) % 4];
                    transform.Rotate(new Vector3(0, 0, 90));
                    playerCoordinate = targetGroundCoordinate;
                    transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
                    spriteRenderer.flipX = true;
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
        SoundManager.instance.Play_SFX(SFX_LIST.PLAYER_JUMP);
        playerGravityDirection = playerDirectionEnum[((int)playerGravityDirection + 2) % 4];
        transform.Rotate(new Vector3(0, 0, 180));
        spriteRenderer.flipX = !IsPlayerXFlip;
        playerState = PlayerState.Move;
        StartCoroutine(MovingTime());
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        playerAnimator = this.GetComponentInChildren<Animator>();
        playerAnimator.Play("Idle", -1, 0f);
    }

    //Player Setting 함수
    public void SetPlayer(int xPos, int yPos, int direction)
    {
        playerCoordinate = new Vector2(xPos, yPos);
        Debug.Log("Set Player Direction " + direction);
        playerGravityDirection = playerDirectionEnum[direction];
        Debug.Log("Set Player Gravity Direction " + playerGravityDirection);
        playerState = PlayerState.Idle;
        transform.rotation = Quaternion.Euler(0, 0, ((int)playerGravityDirection + 2) * -90f);
        transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
    }

    //접기 기믹 후 Player Setting
    public void playerFolding(Vector2 foldCoordinate, int direction)
    {
        playerCoordinate = foldCoordinate;
        transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
        transform.rotation = Quaternion.Euler(0, 0, ((int)playerGravityDirection+2) * -90f);
        switch (((int)playerGravityDirection - direction + 4) % 4) {
            case 0:
                Jump();
                break;
            case 1:
                spriteRenderer.flipX = !IsPlayerXFlip;
                playerState = PlayerState.Move;
                StartCoroutine(MovingTime());
                break;
            case 2:
                Jump();
                break;
            case 3:
                spriteRenderer.flipX = !IsPlayerXFlip;
                playerState = PlayerState.Move;
                StartCoroutine(MovingTime());
                break;
        }
    }

    // Update is called once per frame


    void Falling()
    {
        playerState = PlayerState.Falling;
        playerAnimator.SetBool("IsFalling", true);
        playerCoordinate = playerGroundCoordinate;
        //transform.position = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
        StartCoroutine(FaillingTime());
        //바닥으로 이동
    }

    public void PlayerDie()
    {
        if (playerState != PlayerState.StageClear)
        {
            playerState = PlayerState.DIe;
            playerAnimator.SetTrigger("PlayerDie");
            StartCoroutine(DieTime());
        }
    }
    
    public void StageClear()
    {
        playerState = PlayerState.StageClear;
    }

    IEnumerator MovingTime()
    {
        SoundManager.instance.Play_SFX(SFX_LIST.PLAYER_MOVE);
        yield return new WaitForSeconds(inputDelay/20);
        //Falling Animation으로 전환
        if (MapManager.instance.Get_MapTileType((int)playerGroundCoordinate.x, (int)playerGroundCoordinate.y) != 0)
        {
            playerAnimator.SetBool("IsFalling", false);
            yield return new WaitForSeconds(inputDelay * 19 / 20);
            playerState = PlayerState.Idle;
        }
        else
        {
            Falling();
        }
    }

    IEnumerator FaillingTime()
    {
        Vector3 targetPosition = MapManager.instance.Get_MapTilePosition((int)playerCoordinate.x, (int)playerCoordinate.y);
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallingSpeed * Time.deltaTime);
            yield return null;
        }
        if (MapManager.instance.Get_MapTileType((int)playerGroundCoordinate.x, (int)playerGroundCoordinate.y) != 0)
        {
            playerAnimator.SetBool("IsFalling", false);
            yield return new WaitForSeconds(inputDelay * 7 / 6);
            playerState = PlayerState.Idle;
        }
        else
        {
            Falling();
        }
    }

    IEnumerator DieTime()
    {
        yield return new WaitForSeconds(1f);
        StageManager.instance.Restart();
    }
}
