using System;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    [SerializeField] Button btn_All;
    [SerializeField] Button btn_Food;
    [SerializeField] Button btn_Fish;

    [SerializeField] Button btn_LastPage;
    [SerializeField] Button btn_NextPage;

    [SerializeField] Transform group;

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