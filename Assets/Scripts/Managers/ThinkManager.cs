using System;
using System.Collections.Generic;
using UnityEngine;

public class ThinkManager : MonoBehaviour
{
    [SerializeField] Camera thinkCam;
    [SerializeField] Transform think;

    public List<ThinkScene> thinkScenes;
    ThinkScene scene;

    public bool isThinking;

    public void Ctor()
    {
        isThinking = false;
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
    }

    public void Think_Update(float dt)
    {
        CamearTarget(dt);

        if (isThinking)
        {
            // if (scene == null) { return; }
            
            scene.ThinkGame(dt);
        }
    }

    void CamearTarget(float dt)
    {
        if (scene == null) { return; }

        var player = scene.GetPlayer();
        if (player == null) { return; }

        thinkCam.transform.position = player.transform.position;
    }

    void SceneClick()
    {
        scene.OnClick();
    }
}