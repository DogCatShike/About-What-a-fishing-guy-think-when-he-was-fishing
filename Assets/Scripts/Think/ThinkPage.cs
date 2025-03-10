using System;
using System.Collections.Generic;
using UnityEngine;

public class ThinkPage : MonoBehaviour
{
    public string text;

    public bool isGame;

    public void Game_Alien(float dt)
    {
        var enemy = gameObject.transform.Find("Enemy");

        var pos = enemy.position;
        pos.y -= 3 * dt;
        enemy.position = pos;
    }
}