using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapTile : MonoBehaviour
{
    float effectDuration = 0.3f;

    public void SetEffect_WiggleNTwinckle()
    {
        StartCoroutine(IE_Effect_Wiggle());
        StartCoroutine(IE_Effect_RedTwinckle());
    }

    IEnumerator IE_Effect_Wiggle()
    {
        Vector3 originPos = transform.position;
        float wiggleRange = 0.03f;
        for(float t = effectDuration; t >= 0f; t-=Time.deltaTime)
        {
            transform.position = originPos + new Vector3(Random.Range(0f, 1), Random.Range(0f, 1)) * wiggleRange;
            yield return null;
        }
        transform.position = originPos;
        //transform.localPosition = Vector3.zero;
    }

    IEnumerator IE_Effect_RedTwinckle()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.color = Color.red;
        yield return new WaitForSeconds(effectDuration);
        render.color = Color.white;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetEffect_WiggleNTwinckle();
        }
    }
}
