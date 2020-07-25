using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    private Text text;
    public float fadingAlpha = 0.5f;
    public float fadingTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        StartCoroutine(FadingIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadingIn()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        yield return new WaitForSeconds(fadingTime);
        StartCoroutine(FadingOut());

    }

    IEnumerator FadingOut()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, fadingAlpha);
        yield return new WaitForSeconds(fadingTime);
        StartCoroutine(FadingIn());
    }
}
