using System;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    RectTransform rect;
    Button button;

    void Ctor()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    public void SpawnFood(Transform foodGroup, Vector2 pos, Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, foodGroup);
        
        var food = go.GetComponent<Food>();
        food.Ctor();
        food.SetPos(pos, canvas);
    }

    void SetPos(Vector2 pos, Canvas canvas)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 localPos);
        rect.anchoredPosition = localPos;
    }

    void OnButtonClick()
    {
        Debug.Log("Food Clicked");
    }
}