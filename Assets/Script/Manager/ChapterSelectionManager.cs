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
        SoundManager.instance.Play_SFX(SFX_LIST.FADE_STAGE);
        StartCoroutine(SceneChangeTime(sceneName));
    }

    IEnumerator SceneChangeTime(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.SceneChange(sceneName);
    }
}
