using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour, ISceneManagerInterface
{
    MapManager mapManager;
    GameManager gameManager;

    //Stage 초기화 함수
    public void Init()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        mapManager = MapManager.instance;
        gameManager = GameManager.instance;
    }
    public void SceneRestart()
    {
        gameManager.SceneRestart();
    }


}
