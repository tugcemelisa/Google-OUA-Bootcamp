using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BargainBubble : ChatBubble
{
    [SerializeField] RectTransform canvas;
    [SerializeField] List<NeededItem> neededItems = new List<NeededItem>();
    [SerializeField] Transform bargainBackgorund;

    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text, List<ItemData> items)
    {
        var obj = Instantiate(GameAssets.Instance.prefabOfBargainBubble, parent);

        obj.localPosition = localPosition;
        obj.GetComponent<BargainBubble>().Setup(iconType, text, items);

        Destroy(obj.gameObject, 5f);

    }


    private void Setup(IconData iconData, string text, List<ItemData> items)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 padding = new Vector2(7f, 2f);
        backgroundSpriteRenderer.size = textSize + padding;

        Vector3 offset = new Vector3(-4f, 0);//For icon
        backgroundSpriteRenderer.transform.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;
        //canvas.transform.parent.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;
        bargainBackgorund.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset + Vector3.down * 10;


        iconSpriteRenderer.sprite = iconData.Sprite;
        transform.LookAt(Camera.main.transform);

        TextWriter.AddWriter_Static(textMeshPro, text, .05f, true, true, () => { });

        canvas.sizeDelta = (textSize + padding);

        int i = 0;
        foreach (var item in items)
        {
            neededItems[i].gameObject.SetActive(true);
            neededItems[i++].SetItem(item);
        }
    }
    private void Setup(IconType iconType, string text, List<ItemData> items)
    {
        IconData data = null;

        foreach (var item in GameAssets.Instance.icons)
        {
            if (item.IconType == iconType)
                data = item;
        }

        Setup(data, text, items);
    }

}

