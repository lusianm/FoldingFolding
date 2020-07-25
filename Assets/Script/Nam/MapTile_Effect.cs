using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapTile : MonoBehaviour
{
    float effectDuration = 0.3f;
    bool isWiggle = false;
    [SerializeField] Material _hdrMat;

    /// <summary>
    /// [연출] 타일을 일정시간동안 흔들고 빨갛게 만듭니다.
    /// </summary>
    public void SetEffect_WiggleNTwinckle()
    {
        if (isWiggle) return;

        StartCoroutine(IE_Effect_Wiggle());
        StartCoroutine(IE_Effect_RedTwinckle());
    }

    IEnumerator IE_Effect_Wiggle()
    {
        isWiggle = true;

        Vector3 originPos = transform.position;
        float wiggleRange = 0.03f;
        for(float t = effectDuration; t >= 0f; t-=Time.deltaTime)
        {
            transform.position = originPos + new Vector3(Random.Range(0f, 1), Random.Range(0f, 1)) * wiggleRange;
            yield return null;
        }
        transform.position = originPos;
        //transform.localPosition = Vector3.zero;

        isWiggle = false;
    }

    IEnumerator IE_Effect_RedTwinckle()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        Material originMat = render.material;

        render.material = _hdrMat;
        yield return new WaitForSeconds(effectDuration);
        render.material = originMat;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetEffect_WiggleNTwinckle();
        }
    }
}
