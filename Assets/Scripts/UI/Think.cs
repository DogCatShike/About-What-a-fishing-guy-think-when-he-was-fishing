using System;
using UnityEngine;
using UnityEngine.UI;

public class Think : MonoBehaviour
{
    [SerializeField] Image think1;
    [SerializeField] Image think2;
    [SerializeField] Image think3;
    [SerializeField] Image think4;
    Color color1;
    Color color2;
    Color color3;
    Color color4;

    [SerializeField] Text text;

    public bool isShow;

    public void Ctor()
    {
        color1 = new Color(1, 1, 1, 0);
        color2 = new Color(1, 1, 1, 0);
        color3 = new Color(1, 1, 1, 0);
        color4 = new Color(1, 1, 1, 0);

        think1.color = color1;
        think2.color = color2;
        think3.color = color3;
        think4.color = color4;

        isShow = false;
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

    public void SetColor(float dt)
    {
        if (color4.a >= 1)
        {
            isShow = true;
            return;
        }

        color1.a += dt;
        if (color1.a > 0.3)
        {
            color2.a += dt;
        }
        if (color2.a > 0.3)
        {
            color3.a += dt;
        }
        if (color3.a > 0.3)
        {
            color4.a += dt;
        }

        think1.color = color1;
        think2.color = color2;
        think3.color = color3;
        think4.color = color4;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}