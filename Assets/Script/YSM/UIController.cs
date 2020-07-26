using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance = null;
    public bool opneTutorial = false;
    [SerializeField] GameObject tutorials;
    [SerializeField] RectTransform _tutorialBoard;
    [SerializeField] GameObject _prevButton;
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _tutorialButton;
    public GameObject _menuPanel;
    int currTutorialIndex = 0;
    int totalIndex;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
        }

        totalIndex = _tutorialBoard.childCount;

        //_anims = _tutorialBoard.GetComponentsInChildren<Animation>();
    }

    private void Start()
    {            
        currTutorialIndex = 0;
        _prevButton.SetActive(false);
        _tutorialBoard.anchoredPosition = Vector2.zero;
    }
    
    public void X_Button()
    {
        opneTutorial = false;
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
        tutorials.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorials.SetActive(true);
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
        opneTutorial = true;
    }
    public void UI_Button_PrevTutorial()
    {
        if (currTutorialIndex == 0)
        {
            _prevButton.SetActive(false);
            return;
        }
        _nextButton.SetActive(true);

        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
        currTutorialIndex--;
        StartCoroutine(PrevPage());

        //_tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        //_anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        //if (currTutorialIndex == 0) _prevButton.SetActive(false);
        //if (!_nextButton.activeInHierarchy) _nextButton.SetActive(true);
    }

    public void UI_Button_NextTutorial()
    {
        if (currTutorialIndex < totalIndex-1)
        {
            _prevButton.SetActive(true);

            _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
            currTutorialIndex++;
            StartCoroutine(NextPage());
        }
        else
        {
            _nextButton.SetActive(false);
        }
        //_tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        //_anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        //if (currTutorialIndex == _anims.Length - 1) _nextButton.SetActive(false);
        //if (!_prevButton.activeInHierarchy) _prevButton.SetActive(true);
    }

    IEnumerator PrevPage()
    {
        float currentPos = _tutorialBoard.localPosition.x;
        while (currentPos < -1300 * currTutorialIndex)
        {
            currentPos += 100f;
            _tutorialBoard.localPosition = new Vector2(currentPos, 0);
            yield return null;
        }
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
    }
    IEnumerator NextPage()
    {
        float currentPos = _tutorialBoard.localPosition.x;
        while (currentPos>-1300* currTutorialIndex)
        {
            currentPos -= 100f;
            _tutorialBoard.localPosition = new Vector2(currentPos, 0);
            yield return null;
        }
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
        if(currTutorialIndex== totalIndex - 1) _nextButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StageManager.instance != null)
            {
                if (!TileController.instance.isTileSelected)
                {
                    if (_menuPanel.activeSelf)
                        _menuPanel.SetActive(false);
                    else
                        _menuPanel.SetActive(true);
                }
            }
            else
            {
                if (_menuPanel.activeSelf)
                    _menuPanel.SetActive(false);
                else
                    _menuPanel.SetActive(true);
            }
        }
    }
       
    public void BGMSoundVoulmeChange(Scrollbar scrollbar)
    {
        //SoundManager.instance._MainBGMVolume = scrollbar.value;
        SoundManager.instance.SetMainVolume_BGM(scrollbar.value);
    }

    public void SFXSoundVoulmeChange(Scrollbar scrollbar)
    {
        //SoundManager.instance._MainSFXVolume = scrollbar.value;
        SoundManager.instance.SetMainVolume_SFX(scrollbar.value);
    }

    public void MoveToLobby()
    {
        X_Button();
        SoundManager.instance.Play_BGM(BGM_LIST.ACTION_RETRO);

        GameManager.instance.SceneChange("ChapterSelect");
        _menuPanel.SetActive(false);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void MoveToNextScene()
    {
        if(StageManager.instance != null)
            StageManager.instance.StageClear();
        else
            GameManager.instance.SceneChange("Stage 1");
        _menuPanel.SetActive(false);

    }


}
