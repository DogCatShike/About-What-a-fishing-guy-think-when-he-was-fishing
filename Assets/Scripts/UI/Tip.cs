using System;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    Text text;

    public void Ctor()
    {
        text = GetComponent<Text>();
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
        return go;
    }

    public void Show(string text)
    {
        this.text.text = text;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}