using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] Button btn_Back;
    [SerializeField] Button btn_Bag;
    [SerializeField] Button btn_Quit;

    public Action OnBackClick;
    public Action OnBagClick;
    public Action OnQuitClick;


    public void Ctor()
    {
        btn_Back.onClick.AddListener(() => OnBackClick?.Invoke());
        btn_Bag.onClick.AddListener(() => OnBagClick?.Invoke());
        btn_Quit.onClick.AddListener(() => OnQuitClick?.Invoke());
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