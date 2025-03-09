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
    float waitTimer;
    public float waitTimerMax; // 等鱼上钩
    bool isBited; // 鱼咬钩
    float biteTimer;
    public float biteTimerMax; // 鱼脱钩

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
        StartFishing();

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.EnterFishing();
            bool isChange = ChangeProgress();

            // if (isChange)
            // {
            //     Debug.Log("胡思乱想");
            // }
            // else
            // {
            Fishing();
            // }
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
                    uiManager.Tip_Show_Always("按E键开始钓鱼");
                    break;
                case 1:
                    uiManager.Tip_Show_Always("树上好像有些吃的");
                    break;
            }
            progressTip = true;
        }
    }

    bool ChangeProgress() // true: 切换进度, false: 保持原进度
    {
        float random = UnityEngine.Random.value;

        if (random < progressProbability)
        {
            progress += 1;
            progressTip = false;
            progressProbability = 0;

            return true;
        }
        else
        {
            progressProbability += 0.1f;

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

        Debug.Log("鱼咬钩");
    }

    void CheckFishing()
    {
        if (biteTimer <= biteTimerMax)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("钓到鱼");

                isBited = false;
                player.ExitFishing();
            }
        }
        else
        {
            Debug.Log("脱钩");

            isBited = false;
            player.ExitFishing();
        }
    }
    #endregion
}
