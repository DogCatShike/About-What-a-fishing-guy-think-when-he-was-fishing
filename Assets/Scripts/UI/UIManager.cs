using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UIs uis;

    // UIs
    [SerializeField] Tip tip;

    public UIManager()
    {
        uis = new UIs();
    }

    public void Tip_Show(Canvas canvas, string text)
    {
        var ui = uis.tip;
        if (ui == null)
        {
            ui = tip.Spawn(canvas).GetComponent<Tip>();
        }
        ui.Show(canvas, text);
        uis.tip = ui;
    }

    public void Tip_Close()
    {
        var ui = uis.tip;
        if (ui == null) { return; }
        ui.Close();
        uis.tip = null;
    }
}