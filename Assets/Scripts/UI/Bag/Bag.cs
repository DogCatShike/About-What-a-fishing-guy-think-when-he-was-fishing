using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    public int page;
    public int classNum; // 0: all, 1: food, 2: fish

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
        page = 1;

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
        var childCount = group.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = group.GetChild(i);
            Destroy(child.gameObject);
        }

        gameObject.SetActive(true);

        int len = elements.Count;
        int pageLen = len;
        int startNum = (page - 1) * 15;

        if (len > 15)
        {
            pageLen = (page - 1) * 15 + len % 15;

            if (page == 1)
            {
                pageLen = 15;
            }
        }

        for (int i = startNum; i < pageLen; i++)
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

    public void CheckPage(List<BagElement> elements)
    {
        if (page == 1)
        {
            btn_LastPage.interactable = false;
        }

        if (page > 1)
        {
            btn_LastPage.interactable = true;
        }

        int len = elements.Count;
        int num = len / 15 + 1;
        if (num > page)
        {
            btn_NextPage.interactable = true;
        }
        else
        {
            btn_NextPage.interactable = false;
        }
    }

    public void ChangeClass(int a) // 0: all, 1: food, 2: fish
    {
        classNum = a;

        if (a == 0)
        {
            btn_All.interactable = false;
            btn_Food.interactable = true;
            btn_Fish.interactable = true;
        }
        else if (a == 1)
        {
            btn_All.interactable = true;
            btn_Food.interactable = false;
            btn_Fish.interactable = true;
        }
        else if (a == 2)
        {
            btn_All.interactable = true;
            btn_Food.interactable = true;
            btn_Fish.interactable = false;
        }
    }
}