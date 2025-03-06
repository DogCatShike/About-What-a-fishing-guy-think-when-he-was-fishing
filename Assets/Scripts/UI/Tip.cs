using System;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    [SerializeField] Text text;

    public void Show(Canvas canvas, string text)
    {
        this.text.text = text;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
        return go;
    }
}