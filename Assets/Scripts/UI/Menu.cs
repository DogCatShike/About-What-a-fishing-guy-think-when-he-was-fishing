using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    RectTransform rect;
    Button button;

    public Action OnMenuClick;

    public bool isOpenPause;

    public void Ctor()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(() => OnMenuClick?.Invoke());
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
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
}