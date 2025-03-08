using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    [SerializeField] Transform group;
    [SerializeField] BagPlaid plaid;

    [SerializeField] Button btn_All;
    [SerializeField] Button btn_Food;
    [SerializeField] Button btn_Fish;

    [SerializeField] Button btn_LastPage;
    [SerializeField] Button btn_NextPage;
    [SerializeField] Button btn_Close;

    public Action OnAllClick;
    public Action OnFoodClick;
    public Action OnFishClick;
    public Action OnLastPageClick;
    public Action OnNextPageClick;
    public Action OnCloseClick;

    public Action<int> OnPlaidClick;

    public void Ctor()
    {
        btn_All.onClick.AddListener(() => OnAllClick?.Invoke());
        btn_Food.onClick.AddListener(() => OnFoodClick?.Invoke());
        btn_Fish.onClick.AddListener(() => OnFishClick?.Invoke());
        btn_LastPage.onClick.AddListener(() => OnLastPageClick?.Invoke());
        btn_NextPage.onClick.AddListener(() => OnNextPageClick?.Invoke());
        btn_Close.onClick.AddListener(() => OnCloseClick?.Invoke());
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
        return go;
    }

    public void Show(List<BagElement> elements)
    {
        gameObject.SetActive(true);

        int len = elements.Count;
        for (int i = 0; i < len; i++)
        {
            BagElement element = elements[i];
            BagPlaid newPlaid = Instantiate(plaid.gameObject, group).GetComponent<BagPlaid>();

            newPlaid.Init(element);
            newPlaid.OnPlaidClick += OnClick;
            newPlaid.Ctor();
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void ClearAll()
    {
        for (int i = 0; i < group.childCount; i++)
        {
            var child = group.GetChild(i);

            Destroy(child.gameObject);
        }
    }

    public void OnClick(int id)
    {
        OnPlaidClick?.Invoke(id);
    }
}