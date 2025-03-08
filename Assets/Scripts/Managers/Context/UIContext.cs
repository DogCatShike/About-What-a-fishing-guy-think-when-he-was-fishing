using System;
using System.Collections.Generic;
using UnityEngine;

public class UIContext
{
    public Tip tip;
    public Menu menu;
    public Think think;
    public Pause pause;
    public Bag bag;
    public List<BagElement> bagElements;
    public MakeSure makeSure;
    public Letter letter;

    public UIContext()
    {
        bagElements = new List<BagElement>();
    }
}