using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    RectTransform rect;
    Button button;

    public void Ctor()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
        return go;
    }

    void OnButtonClick()
    {
        Debug.Log("Menu Clicked");
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