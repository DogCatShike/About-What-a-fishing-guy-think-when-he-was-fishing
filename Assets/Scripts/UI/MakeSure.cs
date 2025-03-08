using System;
using UnityEngine;
using UnityEngine.UI;

public class MakeSure : MonoBehaviour
{
    [SerializeField] Button btn_Sure;
    [SerializeField] Button btn_No;

    public Action<int> OnSureClick;
    public Action OnNoClick;

    int a;

    public void Ctor(int a)
    {
        this.a = a;
        btn_Sure.onClick.AddListener(() => OnSureClick?.Invoke(a));
        btn_No.onClick.AddListener(() => OnNoClick?.Invoke());
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