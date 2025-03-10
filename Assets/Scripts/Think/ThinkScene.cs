using System;
using System.Collections.Generic;
using UnityEngine;

public class ThinkScene : MonoBehaviour
{
    public int sceneID;

    [SerializeField] Transform player;
    public Transform GetPlayer() => player;

    public List<ThinkPage> pages = new List<ThinkPage>();
    public int nowPage = 0;

    public GameObject winObj;
    bool startLastPage;

    public GameObject Spawn(Transform think)
    {
        GameObject go = Instantiate(gameObject, think);
        return go;
    }

    public string OnClick()
    {
        if (nowPage >= pages.Count - 1)
        {
            return null;
        }

        nowPage += 1;

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].gameObject.SetActive(i == nowPage);
        }

        return pages[nowPage].text;
    }

    public void ThinkGame(float dt)
    {
        var page = pages[nowPage];
        if (!page.isGame) { return; }

        if (sceneID == 3)
        {
            page.Game_Alien(dt);
        }
        
        if (!startLastPage)
        {
            if (winObj.activeSelf)
            {
                startLastPage = true;
            }
        }
    }

    public bool IsWin()
    {
        if (startLastPage)
        {
            if (!winObj.activeSelf)
            {
                return true;
            }
        }

        return false;
    }
}