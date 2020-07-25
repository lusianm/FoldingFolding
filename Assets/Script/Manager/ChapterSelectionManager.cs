using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectionManager : MonoBehaviour, ISceneManagerInterface
{
    public GameObject menuPanel;
    public void Init()
    {
        menuPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeSelf)
                menuPanel.SetActive(false);
            else
                menuPanel.SetActive(true);
        }
    }

    public void BGMSoundVoulmeChange(Scrollbar scrollbar)
    {
        SoundManager.instance._MainBGMVolume = scrollbar.value;
    }

    public void SFXSoundVoulmeChange(Scrollbar scrollbar)
    {
        SoundManager.instance._MainSFXVolume = scrollbar.value;
    }

    public void SceneChange(string sceneName)
    {
        GameManager.instance.SceneChange(sceneName);
    }
}
