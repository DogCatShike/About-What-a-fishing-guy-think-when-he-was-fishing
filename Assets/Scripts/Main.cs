using System;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Player
    [SerializeField] Player player;

    // Food
    [SerializeField] List<Food> foods;
    [SerializeField] Transform foodGroup;
    List<Vector2> foodPos;
    int foodNum;
    public int foodNumMax;
    float foodTimer;
    public float foodTimerMax;

    void Awake()
    {
        foodNum = 0;
        foodTimer = foodTimerMax;
        foodPos = new List<Vector2>();
    }

    void Start()
    {

    }

    void Update()
    {
        float dt = Time.deltaTime;
        GetKeyDown();

        // 食物生成
        if (foodNum >= foodNumMax) { return; }

        foodTimer += dt;
        if (foodTimer >= foodTimerMax)
        {
            SpawnFood();
            foodTimer = 0;
            foodNum += 1;
        }
    }

    void GetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.EnterFishing();
        }
    }

    void SpawnFood()
    {
        int len = foods.Count - 1;
        int index = UnityEngine.Random.Range(0, len);
        var food = foods[index];

        Vector2 pos = new Vector2();
        bool canSpawn = false;

        while (!canSpawn)
        {
            int treeCount = UnityEngine.Random.Range(1, 3);

            if (treeCount == 1)
            {
                pos.x = UnityEngine.Random.Range(-6f, -4f);
                pos.y = UnityEngine.Random.Range(-3f, -2f);
            }
            else
            {
                pos.x = UnityEngine.Random.Range(4f, 6.7f);
                pos.y = UnityEngine.Random.Range(-2f, -0.7f);
            }

            canSpawn = true;

            for (int i = 0; i < foodPos.Count; i++)
            {
                var beforePos = foodPos[i];
                var dis = Vector2.Distance(pos, beforePos);
                if (dis < 1)
                {
                    canSpawn = false;
                    break;
                }
            }
        }

        food.SpawnFood(foodGroup, pos);
        foodPos.Add(pos);
    }
}
