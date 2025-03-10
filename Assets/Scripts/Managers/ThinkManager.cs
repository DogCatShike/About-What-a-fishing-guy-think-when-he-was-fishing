using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkManager : MonoBehaviour
{
    [SerializeField] Camera thinkCam;
    [SerializeField] Transform think;

    public List<ThinkScene> thinkScenes;
    ThinkScene scene;

    UIManager uiManager;

    public bool isThinking;

    public void Ctor(UIManager uiManager)
    {
        isThinking = false;
        this.uiManager = uiManager;
    }

    public void Think_Start(int progress)
    {
        if (isThinking) { return; }

        thinkCam.gameObject.SetActive(true);

        int len = thinkScenes.Count;
        for (int i = 0; i < len; i++)
        {
            var thinkScene = thinkScenes[i];

            if (thinkScene.sceneID == progress)
            {
                var go = thinkScene.Spawn(think);
                scene = go.GetComponent<ThinkScene>();
            }
        }

        isThinking = true;

        uiManager.Think_SetText(scene.pages[0].text);
    }

    public void Think_Update(float dt)
    {
        CamearTarget();

        if (isThinking)
        {
            SceneClick();
            scene.ThinkGame(dt);

            bool isWin = scene.IsWin();
            if (isWin)
            {
                isThinking = false;
            }
        }
    }

    void CamearTarget()
    {
        if (scene == null) { return; }

        var player = scene.GetPlayer();
        if (player == null) { return; }

        thinkCam.transform.position = player.transform.position;
    }

    void SceneClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            string text = scene.OnClick();

            if (scene.nowPage >= scene.pages.Count - 1)
            {
                uiManager.Tip_Show_Always("之后就没有新内容了");
                return;
            }
            uiManager.Think_SetText(text);
        }
    }
}