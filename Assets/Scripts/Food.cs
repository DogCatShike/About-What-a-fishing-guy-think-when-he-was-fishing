using System;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public void SpawnFood(Transform foodGroup, Vector2 pos)
    {
        GameObject food = Instantiate(gameObject, foodGroup);
        food.transform.position = pos;
    }
}