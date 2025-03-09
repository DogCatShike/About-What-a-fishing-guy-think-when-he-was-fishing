using System;
using UnityEngine;

public class BagElement
{
    public int id;
    public Sprite sprite;
    public Type type;

    public int number;

    public void Ctor()
    {
        number = 1;
    }

    public void Init(int id, Sprite sprite, Type type)
    {
        this.id = id;
        this.sprite = sprite;
        this.type = type;
    }

    public void GetAll(out int id, out Type type, out Sprite sprite, out int number)
    {
        id = this.id;
        type = this.type;
        sprite = this.sprite;
        number = this.number;
    }
}