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

    // UI
    [SerializeField] Canvas canvas;
    [SerializeField] UIManager uiManager;

    // 进度
    int progress;
    bool progressTip;
    float progressProbability; // 到下一进度的概率

    // 钓鱼
    int foodID;

    void Awake()
    {
        uiManager.Ctor(canvas);

        foodNum = 0;
        foodTimer = foodTimerMax;
        foodPos = new List<Vector2>();

        progress = 0;
        progressTip = false;
        progressProbability = 1;
    }

    void Start()
    {
        uiManager.Menu_Show();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        GetKeyDown();

        // 进度
        ProgressTip();

        // 生成食物
        foodNum = foodGroup.childCount;

        if (foodNum >= foodNumMax) { return; }

        foodTimer += dt;
        if (foodTimer >= foodTimerMax)
        {
            SpawnFood();
            foodTimer = 0;
        }

        // 胡思乱想
        uiManager.Think_SetColor(dt);
    }

    void GetKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.EnterFishing();
            ChangeProgress();
        }
    }

    #region 生成食物
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

        uiManager.Food_Show(food, pos, foodGroup);

        foodPos.Clear();
        for (int i = 0; i < foodGroup.childCount; i++)
        {
            var child = foodGroup.GetChild(i);
            var childPos = child.position;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, childPos);
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

            foodPos.Add(worldPoint);
        }
    }
    #endregion

    #region 进度
    void ProgressTip()
    {
        if (!progressTip)
        {
            switch (progress)
            {
                case 0:
                    uiManager.Tip_Show("按E键开始钓鱼");
                    break;
                case 1:
                    uiManager.Tip_Show("树上好像有些吃的");
                    break;
            }
            progressTip = true;
        }
    }

    void ChangeProgress()
    {
        float random = UnityEngine.Random.value;

        if (random < progressProbability)
        {
            progress += 1;
            progressTip = false;
            progressProbability = 0;
        }
        else
        {
            progressProbability += 0.1f;
        }
    }
    #endregion

    #region 钓鱼

    #endregion
}
