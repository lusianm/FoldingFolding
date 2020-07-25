using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance = null;
    [SerializeField] GameObject tutorials;
    [SerializeField] RectTransform _tutorialBoard;
    [SerializeField] GameObject _prevButton;
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _tutorialButton;
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
        if (SceneManager.GetActiveScene().name == "Stage 1")
            tutorials.SetActive(true);
        currTutorialIndex = 0;
        _prevButton.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "ChapterSelect")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            _tutorialButton.SetActive(true);
        }
    }
    public void X_Button()
    {
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
        tutorials.SetActive(false);
    }

    public void OpenTutorial()
    {
        tutorials.SetActive(true);
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
    }
    public void UI_Button_PrevTutorial()
    {
        if (currTutorialIndex == 0)
        {
            _prevButton.SetActive(false);
            return;
        }
        _nextButton.SetActive(true);
        
        currTutorialIndex--;
        StartCoroutine(PrevPage());

        //_tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        //_anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        //if (currTutorialIndex == 0) _prevButton.SetActive(false);
        //if (!_nextButton.activeInHierarchy) _nextButton.SetActive(true);
    }

    public void UI_Button_NextTutorial()
    {
        if (currTutorialIndex == totalIndex)
        {
            _nextButton.SetActive(false);
            return;
        }
        _prevButton.SetActive(true);

        currTutorialIndex++;
        StartCoroutine(NextPage());
        //_tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        //_anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        //if (currTutorialIndex == _anims.Length - 1) _nextButton.SetActive(false);
        //if (!_prevButton.activeInHierarchy) _prevButton.SetActive(true);
    }

    IEnumerator PrevPage()
    {
        float currentPos = _tutorialBoard.rect.x;
        while (currentPos < -2600 * currTutorialIndex)
        {
            currentPos -= 200f;
            _tutorialBoard.anchoredPosition = new Vector2(currentPos, 0);
            yield return null;
        }
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
        currTutorialIndex--;
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
    }
    IEnumerator NextPage()
    {
        float currentPos = _tutorialBoard.rect.x;
        while (currentPos>2600* currTutorialIndex)
        {
            currentPos += 200f;
            _tutorialBoard.anchoredPosition = new Vector2(currentPos, 0);
            yield return null;
        }
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Stop();
        currTutorialIndex++;
        _tutorialBoard.GetChild(currTutorialIndex).GetComponent<Animation>().Play();
    }


}
