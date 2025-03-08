using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UIContext uiContext;
    Canvas canvas;

    [SerializeField] Tip tip;
    [SerializeField] Menu menu;
    [SerializeField] Think think;
    [SerializeField] Pause pause;

    public void Ctor(Canvas canvas)
    {
        uiContext = new UIContext();
        this.canvas = canvas;
    }

    #region Tip
    public void Tip_Show(string text)
    {
        var ui = uiContext.tip;
        if (ui == null)
        {
            ui = tip.Spawn(canvas).GetComponent<Tip>();
        }
        ui.Ctor();
        ui.Show(text);
        uiContext.tip = ui;
    }

    public void Tip_Close()
    {
        var ui = uiContext.tip;
        if (ui == null) { return; }
        ui.Close();
        uiContext.tip = null;
    }
    #endregion

    #region Menu
    public void Menu_Show()
    {
        var ui = uiContext.menu;
        if (ui == null)
        {
            ui = menu.Spawn(canvas).GetComponent<Menu>();
            ui.OnMenuClick = () => OnMenuClick(ui);
        }
        ui.Ctor();
        ui.Show();
        uiContext.menu = ui;
    }

    public void Menu_Close()
    {
        var ui = uiContext.menu;
        if (ui == null) { return; }
        ui.Close();
        uiContext.menu = null;
    }
    #endregion

    #region Think
    public void Think_Show()
    {
        var ui = uiContext.think;
        if (ui == null)
        {
            ui = think.Spawn(canvas).GetComponent<Think>();
        }
        ui.Ctor();
        ui.Show();
        uiContext.think = ui;
    }

    public void Think_Close()
    {
        var ui = uiContext.think;
        if (ui == null) { return; }
        ui.Close();
        uiContext.think = null;
    }

    public void Think_SetColor(float dt)
    {
        var ui = uiContext.think;

        if (ui == null) { return; }
        if (ui.isShow) { return; }
        
        ui.SetColor(dt);
    }
    #endregion

    #region Pause
    public void Pause_Show()
    {
        var ui = uiContext.pause;
        if (ui == null)
        {
            ui = pause.Spawn(canvas).GetComponent<Pause>();

            ui.OnBackClick = () => OnBackClick();
            ui.OnBagClick = () => OnBagClick();
            ui.OnQuitClick = () => OnQuitClick();
        }
        ui.Ctor();
        ui.Show();
        uiContext.pause = ui;
    }

    public void Pause_Close()
    {
        var ui = uiContext.pause;
        if (ui == null) { return; }
        ui.Close();
        uiContext.pause = null;
    }
    #endregion

    #region Event
    // Menu
    public void OnMenuClick(Menu ui)
    {
        if (!ui.isOpenPause)
        {
            Time.timeScale = 0;
            Pause_Show();
            ui.isOpenPause = true;
        }
        else
        {
            Time.timeScale = 1;
            Pause_Close();
            ui.isOpenPause = false;
        }
    }

    // Pause
    public void OnBackClick()
    {
        Debug.Log("返回主界面");
    }

    public void OnBagClick()
    {
        Debug.Log("打开背包");
    }

    public void OnQuitClick()
    {
        Debug.Log("退出游戏");
    }
    #endregion
}