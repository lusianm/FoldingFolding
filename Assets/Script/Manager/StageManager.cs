using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour, ISceneManagerInterface
{
    MapManager mapManager;
    GameManager gameManager;
    public static StageManager instance;
    [SerializeField] private string nextSceneName;

    public void Awake()
    {
        instance = this;
    }

    //Stage 초기화 함수
    public void Init()
    {
        Debug.Log("Stage Init");
        SoundManager.instance.Play_BGM(BGM_LIST.GAME_LV1);
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
        SoundManager.instance.Play_BGM(BGM_LIST.WIN_1);
        StartCoroutine(StageClearTime());
    }

    IEnumerator StageClearTime()
    {
        yield return new WaitForSeconds(2f);
        gameManager.SceneChange(nextSceneName);
    }
}
