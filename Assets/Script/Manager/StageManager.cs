using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour, ISceneManagerInterface
{
    MapManager mapManager;
    GameManager gameManager;
    public static StageManager instance;
    [SerializeField] private Vector2 StartingCoordinate;
    [SerializeField] private string nextSceneName;

    public void Awake()
    {
        instance = this;
    }

    //Stage 초기화 함수
    public void Init()
    {
        Debug.Log("Stage Init");
        Player.playerInstance.SetPlayer((int)StartingCoordinate.x, (int)StartingCoordinate.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        mapManager = MapManager.instance;
        gameManager = GameManager.instance;
    }
    public void Restart()
    {
        gameManager.SceneRestart();
    }

    public void StageClear()
    {
        gameManager.SceneChange(nextSceneName);
    }

}
