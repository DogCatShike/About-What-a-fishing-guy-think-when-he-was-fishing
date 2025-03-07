using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UIContext uiContext;

    [SerializeField] Tip tip;
    [SerializeField] Menu menu;
    [SerializeField] Think think;

    public UIManager()
    {
        uiContext = new UIContext();
    }

    #region Tip
    public void Tip_Show(Canvas canvas, string text)
    {
        var ui = uiContext.tip;
        if (ui == null)
        {
            ui = tip.Spawn(canvas).GetComponent<Tip>();
            ui.Ctor();
        }
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
    public void Menu_Show(Canvas canvas)
    {
        var ui = uiContext.menu;
        if (ui == null)
        {
            ui = menu.Spawn(canvas).GetComponent<Menu>();
            ui.Ctor();
        }
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
    public void Think_Show(Canvas canvas)
    {
        var ui = uiContext.think;
        if (ui == null)
        {
            ui = think.Spawn(canvas).GetComponent<Think>();
            ui.Ctor();
        }
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
}