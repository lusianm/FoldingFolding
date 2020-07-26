using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform[] prefabList;
    public GameObject[] tilePrefabList;
    [Range(0, 1)]
    public float tilePadValue = 0.33f;

    public ParticleSystem boxEffect;

    [SerializeField] private ISceneManagerInterface sceneManager;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void SceneRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Start()
    {
        sceneManager = transform.parent.GetComponentInChildren<ISceneManagerInterface>();
        if (sceneManager == null)
            Debug.Log("sceneManager is null");
        else
            Init();
    }

    public void Init()
    {
        sceneManager.Init();
    }

    public void Update()
    {

    }


}
