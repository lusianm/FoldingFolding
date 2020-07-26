using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectionManager : MonoBehaviour, ISceneManagerInterface
{
    public void Init()
    {
    }

    // Update is called once per frame
    

    public void SceneChange(string sceneName)
    {
        GameManager.instance.SceneChange(sceneName);
    }
}
