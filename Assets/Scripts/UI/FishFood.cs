using System;
using UnityEngine;
using UnityEngine.UI;

public class FishFood : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;

        if (image.sprite == null)
        {
            image.color = Color.clear;
        }
        else
        {
            image.color = Color.white;
        }
    }
}