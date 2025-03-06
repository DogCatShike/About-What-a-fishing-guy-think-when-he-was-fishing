using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public void SpawnFood(Transform foodGroup, Vector2 pos)
    {
        GameObject food = Instantiate(gameObject, foodGroup);
        food.transform.position = pos;
    }
}