using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCrawl : MonoBehaviour
{
    public float scrollSpeed;
    public UnityEngine.UI.Image background;
    private RectTransform rt, bg;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
        bg = background.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rectOverlaps(rt, bg))
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0.25f);
        }
        else
        {
            background.color = new Color(background.color.r, background.color.g, background.color.b, 1f);
        }
        transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
    }

    // Thanks to https://stackoverflow.com/questions/42043017/check-if-ui-elements-recttransform-are-overlapping
    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
