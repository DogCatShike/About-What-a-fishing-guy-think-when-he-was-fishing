using System;
using UnityEngine;

public class ThinkPage : MonoBehaviour
{
    public string Text;

    public bool isGame;

    public void Game_Alien(float dt)
    {
        var enemy = gameObject.transform.Find("Enemy");

        var pos = enemy.position;
        pos.y -= 10 * dt;
    }
}