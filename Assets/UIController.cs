using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] RectTransform _tutorialBoard;
    [SerializeField] GameObject _prevButton;
    [SerializeField] GameObject _nextButton;

    Animation[] _anims;
    string[] Clips = { "Tutorial1-1_Anim", "Tutorial2-1_Anim", "Tutorial3-1_Anim" };
    int currTutorialIndex = 0;

    private void Awake()
    {
        _anims = _tutorialBoard.GetComponentsInChildren<Animation>();
    }

    private void Start()
    {
        currTutorialIndex = 0;
        _tutorialBoard.transform.localPosition = Vector3.zero;
        _anims[currTutorialIndex].Play(Clips[currTutorialIndex]);
        _prevButton.SetActive(false);
    }

    public void UI_Button_PrevTutorial()
    {
        if (currTutorialIndex == 0) return;

        currTutorialIndex--;

        _tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        _anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        if (currTutorialIndex == 0) _prevButton.SetActive(false);
        if (!_nextButton.activeInHierarchy) _nextButton.SetActive(true);
    }

    public void UI_Button_NextTutorial()
    {
        if (currTutorialIndex == _anims.Length - 1) return; 

        currTutorialIndex++;

        _tutorialBoard.transform.localPosition = new Vector2(-1300 * currTutorialIndex, 0);
        _anims[currTutorialIndex].Play(Clips[currTutorialIndex]);

        if (currTutorialIndex == _anims.Length - 1) _nextButton.SetActive(false);
        if (!_prevButton.activeInHierarchy) _prevButton.SetActive(true);
    }
}
