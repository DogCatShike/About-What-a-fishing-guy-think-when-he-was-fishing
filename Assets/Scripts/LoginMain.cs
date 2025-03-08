using System;
using UnityEngine;

public class LoginMain : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] UIManager uiManager;

    void Awake()
    {
        uiManager.Ctor(canvas);
    }
}