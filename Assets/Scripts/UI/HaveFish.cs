using System;
using UnityEngine;
using UnityEngine.UI;

public class HaveFish : MonoBehaviour
{
    public void Ctor()
    {

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