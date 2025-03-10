using System;
using System.Collections.Generic;
using UnityEngine;

public class ThinkScene : MonoBehaviour
{
    public int sceneID;

    [SerializeField] Transform player;
    public Transform GetPlayer() => player;

    public List<ThinkPage> pages = new List<ThinkPage>();
    int nowPage = 0;

    public GameObject Spawn(Transform think)
    {
        GameObject go = Instantiate(gameObject, think);
        return go;
    }

    public string OnClick()
    {
        nowPage += 1;

        if (nowPage >= pages.Count)
        {
            Debug.Log("当前think结束");
            return null;
        }

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].gameObject.SetActive(i == nowPage);
        }

        return pages[nowPage].Text;
    }

    public void ThinkGame(float dt)
    {
        var page = pages[nowPage];
        if (!page.isGame) { return; }

        if (sceneID == 3)
        {
            page.Game_Alien(dt);
        }
    }
}