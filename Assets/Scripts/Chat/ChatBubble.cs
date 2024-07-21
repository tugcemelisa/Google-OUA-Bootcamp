using System;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        var obj = Instantiate(GameAssets.Instance.prefabOfChatBubble, parent);

        obj.localPosition = localPosition;
        obj.GetComponent<ChatBubble>().Setup(iconType, text);

        Destroy(obj.gameObject, 5f);

    }


    [SerializeField] SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] SpriteRenderer iconSpriteRenderer;
    [SerializeField] TextMeshPro textMeshPro;

    [SerializeField] IconData[] icons;


    [System.Serializable]
    public class IconData
    {
        public IconType IconType;
        public string IconName;
        public Sprite Sprite;
    }

    private void Setup(IconData iconData, string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 padding = new Vector2(7f, 2f);
        backgroundSpriteRenderer.size = textSize + padding;

        Vector3 offset = new Vector3(-4f, 0);//For icon
        backgroundSpriteRenderer.transform.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;

        iconSpriteRenderer.sprite = iconData.Sprite;
        transform.LookAt(Camera.main.transform);

        TextWriter.AddWriter_Static(textMeshPro, text, .05f, true, true, () => { });
    }
    private void Setup(IconType iconType, string text)
    {
        IconData data = null;

        foreach (var item in icons)
        {
            if (item.IconType == iconType)
                data = item;
        }

        Setup(data, text);
    }

}

public enum IconType
{
    Neutral,
    Success,
    Bargain,
    Failure,
    Informative
}
