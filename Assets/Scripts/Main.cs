using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Player
    [SerializeField] Player player;

    // Food
    [SerializeField] List<Food> foods;
    [SerializeField] List<Fish> fishes;
    [SerializeField] Transform foodGroup;
    List<Vector2> foodPos;
    int foodNum;
    public int foodNumMax;
    float foodTimer;
    public float foodTimerMax;

    // 进度
    int progress;
    float progressProbability; // 到下一进度的概率
    bool isThinking;
    [SerializeField] GameObject thinkMask;
    GameObject mask;

    // 新手教程
    bool hasFishing;
    bool hasFood;
    bool hasOpenBag;
    bool hasBited;
    bool isEnd;

    // 钓鱼
    int foodID;
    float waitTimer;
    public float waitTimerMax; // 等鱼上钩
    bool isBited; // 鱼咬钩
    float biteTimer;
    public float biteTimerMax; // 鱼脱钩

    // UI
    [SerializeField] Canvas canvas;
    [SerializeField] UIManager uiManager;
    [SerializeField] ThinkManager thinkManager;

    void Awake()
    {
        uiManager.Ctor(canvas);
        thinkManager.Ctor(uiManager);

        foodNum = 0;
        foodTimer = foodTimerMax;
        foodPos = new List<Vector2>();

        progress = 0;
        progressProbability = 1;
    }

    void Start()
    {
        uiManager.Menu_Show();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        StartFishing();

        // 进度
        ProgressTip();
        if (uiManager.hasFood())
        {
            hasFood = true;
        }
        if (!hasOpenBag)
        {
            hasOpenBag = uiManager.hasOpenBag;
        }

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
        thinkManager.Think_Update(dt);

        if (isThinking && !thinkManager.isThinking)
        {
            uiManager.Think_Close();
            isThinking = false;
            Destroy(mask);
            player.ExitFishing();
        }

        // 钓鱼
        waitTimer += dt;
        biteTimer += dt;
        foodID = uiManager.foodID;

        if (isBited)
        {
            uiManager.HaveFish_Show();
            CheckFishing();
        }
        else
        {
            uiManager.HaveFish_Close();
        }
    }

    void StartFishing()
    {
        if (player.isFishing) { return; }
        if (uiManager.isUIShow) { return; }
        if (isThinking) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hasFishing) { hasFishing = true; }

            player.EnterFishing();
            bool isChange = ChangeProgress();

            if (isChange)
            {
                uiManager.Think_Show();
                isThinking = true;
                mask = Instantiate(thinkMask, canvas.transform);
                thinkManager.Think_Start(progress);
                isEnd = true;
            }
            else
            {
                Fishing();
            }
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

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, childPos);
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

            foodPos.Add(worldPoint);
        }
    }
    #endregion

    #region 进度
    void ProgressTip()
    {
        if (!hasFishing)
        {
            uiManager.Tip_Show_Always("按E键开始钓鱼");
            return;
        }
        if (hasBited)
        {
            return;
        }
        if (!hasFood)
        {
            uiManager.Tip_Show_Always("树上好像有些吃的");
            return;
        }
        if (!hasOpenBag)
        {
            uiManager.Tip_Show_Always("左上角按钮打开背包, 里面似乎有东西可以挂在鱼钩上");
            return;
        }
        if (isEnd)
        {
            uiManager.Tip_Show_Always("之后就没有其他内容了");
            return;
        }

        // switch (progress)
        // {
        //     case 0:
        //         uiManager.Tip_Show_Always("按E键开始钓鱼");
        //         break;
        //     case 1:
        //         uiManager.Tip_Show_Always("树上好像有些吃的");
        //         break;
        // }
    }

    bool ChangeProgress() // true: 切换进度, false: 保持原进度
    {
        float random = UnityEngine.Random.value;

        if (random < progressProbability)
        {
            progress += 1;
            progressProbability = 0;

            if (progress <= 2)
            {
                return false;
            }

            return true;
        }
        else
        {
            progressProbability += 1f;

            return false;
        }
    }
    #endregion

    #region 钓鱼
    void Fishing()
    {
        waitTimer = 0;
        float maxTime = UnityEngine.Random.Range(1f, waitTimerMax);

        StartCoroutine(FishingIE(maxTime));
    }

    IEnumerator FishingIE(float maxTime)
    {
        while (waitTimer < maxTime)
        {
            yield return null;
        }

        Bite();
    }

    void Bite()
    {
        biteTimer = 0;
        isBited = true;

        if (!hasBited)
        {
            hasBited = true;
            uiManager.Tip_Show_2s("鱼上钩了, 在他跑掉之前按E收线");
        }
    }

    void CheckFishing()
    {
        if (biteTimer <= biteTimerMax)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AddFish();

                isBited = false;
                player.ExitFishing();

                uiManager.SetFoodID(-1);
                uiManager.FishFood_SetImage(-1);
            }
        }
        else
        {
            Debug.Log("脱钩");

            isBited = false;
            player.ExitFishing();

            uiManager.SetFoodID(-1);
            uiManager.FishFood_SetImage(-1);
        }
    }

    void AddFish()
    {
        hasBited = false;
        
        if (foodID == 1)
        {
            uiManager.AddFish_Show("空气", null, "怎么鱼也不爱吃苹果啊");
            return;
        }

        int fishID = foodID + 10;
        int len = fishes.Count;
        for (int i = 0; i < len; i++)
        {
            var fish = fishes[i];
            if (fish.id == fishID)
            {
                var name = fish.name;
                var sprite = fish.sprite;
                var spitOut = fish.spitOut;

                uiManager.AddFish_Show(name, sprite, spitOut);
                uiManager.Bag_Add(fishID, sprite, fish.type);
                return;
            }
        }

        uiManager.AddFish_Show("空气", null, "好像这个游戏的主角不叫姜太公");
    }
    #endregion
}
