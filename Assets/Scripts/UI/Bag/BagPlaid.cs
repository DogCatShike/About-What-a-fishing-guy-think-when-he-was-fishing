using System;
using UnityEngine;
using UnityEngine.UI;

public class BagPlaid : MonoBehaviour
{
    int id;
    Type type;
    public int number;

    BagElement element;

    [SerializeField] Button button;
    [SerializeField] Image img_Num;

    [SerializeField] Image image;
    [SerializeField] Text txt_Num;

    public void Ctor()
    {
        number = 1;
    }

    public void Init(BagElement element)
    {
        this.element = element;

        SetAll();
    }

    void SetAll()
    {
        element.GetAll(out int id, out Type type, out Sprite sprite, out int number);

        this.id = id;
        this.type = type;
        this.number = number;

        image.sprite = sprite;
        txt_Num.text = number.ToString();
    }
}