using System;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public int id;
    public Type type;

    RectTransform rect;
    Button button;
    public Sprite sprite;

    public Action OnFoodClick;

    public void Ctor()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        sprite = GetComponent<Image>().sprite;

        button.onClick.AddListener(() =>
        {
            OnFoodClick?.Invoke();
            Close();
        });
    }

    public GameObject Spawn(Transform group)
    {
        GameObject go = Instantiate(gameObject, group);
        var food = go.GetComponent<Food>();
        return go;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void SetPos(Vector2 pos, Canvas canvas)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out Vector2 localPos);
        rect.anchoredPosition = localPos;
    }
}