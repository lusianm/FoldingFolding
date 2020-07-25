using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public void SceneChange()
    {
        GameManager.instance.SceneChange("ChapterSelect");
    }
}
