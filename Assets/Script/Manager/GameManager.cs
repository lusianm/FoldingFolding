using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private ISceneManagerInterface sceneManager;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.sceneCount);
    }

    
}
