using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    UIContext uiContext;
    public bool hasFood() => uiContext.bagElements.Count > 0;
    Canvas canvas;
    public bool hasOpenBag;

    [SerializeField] Tip tip;
    [SerializeField] Menu menu;
    [SerializeField] Think think;
    [SerializeField] Pause pause;
    [SerializeField] Bag bag;
    [SerializeField] MakeSure makeSure;
    [SerializeField] FishFood fishFood;
    [SerializeField] HaveFish haveFish;
    [SerializeField] AddFish addFish;

    [SerializeField] Letter letter;

    public int foodID;
    public void SetFoodID(int id) => foodID = id;

    public bool isUIShow;

    public void Ctor(Canvas canvas)
    {
        uiContext = new UIContext();
        this.canvas = canvas;

        foodID = -1;
        isUIShow = false;
    }

    #region Tip
    public void Tip_Show_Always(string text)
    {
        var ui = uiContext.tip;
        if (ui == null)
        {
            ui = tip.Spawn(canvas).GetComponent<Tip>();
        }
        ui.Ctor();
        ui.Show(text);
        uiContext.tip = ui;
    }

    public float Tip_Show_2s(string text)
    {
        Tip_Close();

        var ui = uiContext.tip;
        if (ui == null)
        {
            ui = tip.Spawn(canvas).GetComponent<Tip>();
        }
        ui.Ctor();
        ui.Show(text);
        uiContext.tip = ui;

        float unscaledTime = Time.unscaledTime;
        StartCoroutine(Tip_IE(unscaledTime));
        return unscaledTime;
    }

    IEnumerator Tip_IE(float unscaledTime)
    {
        while (Time.unscaledTime - unscaledTime < 2)
        {
            yield return null;
        }

        Tip_Close();
    }

    public void Tip_Close()
    {
        var ui = uiContext.tip;
        if (ui == null) { return; }
        ui.Close();
        uiContext.tip = null;
    }
    #endregion

    #region Menu
    public void Menu_Show()
    {
        var ui = uiContext.menu;
        if (ui == null)
        {
            ui = menu.Spawn(canvas).GetComponent<Menu>();
            ui.OnMenuClick = () => OnMenuClick(ui);
        }
        ui.Ctor();
        ui.Show();
        uiContext.menu = ui;
    }

    public void Menu_Close()
    {
        var ui = uiContext.menu;
        if (ui == null) { return; }
        ui.Close();
        uiContext.menu = null;
    }
    #endregion

    #region Think
    public void Think_Show()
    {
        var ui = uiContext.think;
        if (ui == null)
        {
            ui = think.Spawn(canvas).GetComponent<Think>();
        }
        ui.Ctor();
        ui.Show();
        uiContext.think = ui;
    }

    public void Think_Close()
    {
        var ui = uiContext.think;
        if (ui == null) { return; }
        ui.Close();
        uiContext.think = null;
    }

    public void Think_SetColor(float dt)
    {
        var ui = uiContext.think;

        if (ui == null) { return; }
        if (ui.isShow) { return; }

        ui.SetColor(dt);
    }

    public void Think_SetText(string text)
    {
        var ui = uiContext.think;
        if (ui == null) { return; }
        ui.SetText(text);
    }
    #endregion

    #region Pause
    public void Pause_Show()
    {
        var ui = uiContext.pause;
        if (ui == null)
        {
            ui = pause.Spawn(canvas).GetComponent<Pause>();

            ui.OnBackClick = () => OnBackClick();
            ui.OnBagClick = () => OnBagClick();
            ui.OnQuitClick = () => OnQuitClick();
        }
        ui.Ctor();
        ui.Show();
        uiContext.pause = ui;
    }

    public void Pause_Close()
    {
        var ui = uiContext.pause;
        if (ui == null) { return; }
        ui.Close();
        uiContext.pause = null;
    }
    #endregion

    #region Bag
    public void Bag_Show()
    {
        var ui = uiContext.bag;
        if (ui == null)
        {
            ui = bag.Spawn(canvas).GetComponent<Bag>();

            ui.OnAllClick = () => OnAllClick();
            ui.OnFoodClick = () => OnFoodClick();
            ui.OnFishClick = () => OnFishClick();
            ui.OnLastPageClick = () => OnLastPageClick();
            ui.OnNextPageClick = () => OnNextPageClick();
            ui.OnCloseClick = () => OnCloseClick();

            ui.OnPlaidClick = (id) => OnPlaidClick(id);

            ui.ChangeClass(0);
        }
        ui.Ctor();
        ui.ClearAll();

        List<BagElement> elements = null;
        switch (ui.classNum)
        {
            case 0:
                elements = uiContext.bagElements;
                break;
            case 1:
                elements = uiContext.foodElements;
                break;
            case 2:
                elements = uiContext.fishElements;
                break;
            default:
                break;
        }

        ui.CheckPage(elements);
        ui.Show(elements);
        uiContext.bag = ui;
        
        hasOpenBag = true;
    }

    public void Bag_Close()
    {
        var ui = uiContext.bag;
        if (ui == null) { return; }
        ui.Close();
        uiContext.bag = null;
    }

    public void Bag_Add(int id, Sprite sprite, Type type)
    {
        int len = uiContext.bagElements.Count;
        for (int i = 0; i < len; i++)
        {
            BagElement element = uiContext.bagElements[i];
            if (element.id == id)
            {
                element.number += 1;
                return;
            }
        }

        BagElement newelement = new BagElement();
        newelement.Ctor();
        newelement.Init(id, sprite, type);
        uiContext.bagElements.Add(newelement);
    }

    public void Bag_Use(int id)
    {
        for (int i = uiContext.bagElements.Count - 1; i >= 0; i--)
        {
            BagElement element = uiContext.bagElements[i];
            if (element.id == id)
            {
                element.number -= 1;

                if (element.number <= 0)
                {
                    uiContext.bagElements.Remove(element);
                }
            }
        }

        Bag_Show();
    }

    public void Bag_ChangeClass(int a)
    {
        var ui = uiContext.bag;
        if (ui == null) { return; }
        ui.ChangeClass(a);
    }
    #endregion

    #region Food
    public void Food_Show(Food food, Vector2 pos, Transform foodGroup)
    {
        var ui = food.Spawn(foodGroup).GetComponent<Food>();
        ui.OnFoodClick = () => OnTreeFoodClick(food.id, food.sprite, food.type);
        ui.Ctor();
        ui.Show();
        ui.SetPos(pos, canvas);
    }
    #endregion

    #region MakeSure
    public void MakeSure_Show(int a)
    {
        var ui = uiContext.makeSure;
        if (ui == null)
        {
            ui = makeSure.Spawn(canvas).GetComponent<MakeSure>();

            ui.OnSureClick += OnSureClick;
            ui.OnNoClick = () => OnNoClick();
        }
        ui.Ctor(a);
        ui.Show();
        uiContext.makeSure = ui;
    }

    public void MakeSure_Close()
    {
        var ui = uiContext.makeSure;
        if (ui == null) { return; }
        ui.Close();
        uiContext.makeSure = null;
    }
    #endregion

    #region FishFood
    public void FishFood_SetImage(int id)
    {
        Sprite sprite = null;
        for (int i = uiContext.bagElements.Count - 1; i >= 0; i--)
        {
            BagElement element = uiContext.bagElements[i];
            if (element.id == id)
            {
                sprite = element.sprite;
            }
        }

        fishFood.SetImage(sprite);
    }
    #endregion

    #region HaveFish
    public void HaveFish_Show()
    {
        var ui = uiContext.haveFish;
        if (ui == null)
        {
            ui = haveFish.Spawn(canvas).GetComponent<HaveFish>();
        }
        ui.Ctor();
        ui.Show();
        uiContext.haveFish = ui;
    }

    public void HaveFish_Close()
    {
        var ui = uiContext.haveFish;
        if (ui == null) { return; }
        ui.Close();
        uiContext.haveFish = null;
    }
    #endregion

    #region AddFish
    public void AddFish_Show(string fishName, Sprite fishImage, string spitOut)
    {
        var ui = uiContext.addFish;
        if (ui == null)
        {
            ui = addFish.Spawn(canvas).GetComponent<AddFish>();

            ui.OnOKClicked = () => OnAddFishOKClicked();
        }
        ui.Ctor();
        ui.Show(fishName, fishImage, spitOut);
        uiContext.addFish = ui;

        isUIShow = true;
    }

    public void AddFish_Close()
    {
        var ui = uiContext.addFish;
        if (ui == null) { return; }
        ui.Close();
        uiContext.addFish = null;

        isUIShow = false;
    }
    #endregion

    #region Letter
    public void Letter_Show()
    {
        var ui = uiContext.letter;
        if (ui == null)
        {
            ui = letter.Spawn(canvas).GetComponent<Letter>();

            ui.OnCloseClick = () => OnLetterCloseClick();
        }
        ui.Ctor();
        ui.Show();
        uiContext.letter = ui;
    }

    public void Letter_Close()
    {
        var ui = uiContext.letter;
        if (ui == null) { return; }
        ui.Close();
        uiContext.letter = null;
    }
    #endregion

    #region Event
    // Menu
    public void OnMenuClick(Menu ui)
    {
        if (!ui.isOpenPause)
        {
            Time.timeScale = 0;
            Pause_Show();
            ui.isOpenPause = true;
        }
        else
        {
            Time.timeScale = 1;
            Pause_Close();
            ui.isOpenPause = false;
        }
    }

    // Pause
    public void OnBackClick()
    {
        MakeSure_Show(0);
    }

    public void OnBagClick()
    {
        Bag_Show();
    }

    public void OnQuitClick()
    {
        MakeSure_Show(1);
    }

    // Bag
    public void OnAllClick()
    {
        Bag_ChangeClass(0);
        Bag_Show();
    }

    public void OnFoodClick()
    {
        Bag_ChangeClass(1);
        uiContext.foodElements.Clear();

        var elements = uiContext.bagElements;
        int len = elements.Count;
        for (int i = 0; i < len; i++)
        {
            BagElement element = elements[i];
            if (element.type == Type.Food)
            {
                uiContext.foodElements.Add(element);
            }
        }

        Bag_Show();
    }

    public void OnFishClick()
    {
        Bag_ChangeClass(2);
        uiContext.fishElements.Clear();

        var elements = uiContext.bagElements;
        int len = elements.Count;
        for (int i = 0; i < len; i++)
        {
            BagElement element = elements[i];
            if (element.type == Type.Fish)
            {
                uiContext.fishElements.Add(element);
            }
        }

        Bag_Show();
    }

    public void OnLastPageClick()
    {
        var ui = uiContext.bag;
        if (ui == null) { return; }
        ui.page -= 1;
        ui.CheckPage(uiContext.bagElements);
        ui.Show(uiContext.bagElements);
    }

    public void OnNextPageClick()
    {
        var ui = uiContext.bag;
        if (ui == null) { return; }
        ui.page += 1;
        ui.CheckPage(uiContext.bagElements);
        ui.Show(uiContext.bagElements);
    }

    public void OnCloseClick()
    {
        Bag_Close();
    }

    public void OnPlaidClick(int id)
    {
        if (foodID != -1)
        {
            Tip_Show_2s("已经有鱼饵了");
            return;
        }
        if (id < 1 || id > 10)
        {
            Tip_Show_2s("该物品不能当作鱼饵");
            return;
        }

        FishFood_SetImage(id);
        Bag_Use(id);
        foodID = id;
    }

    // Food
    void OnTreeFoodClick(int id, Sprite sprite, Type type)
    {
        Bag_Add(id, sprite, type);
    }

    // MakeSure
    public void OnSureClick(int a)
    {
        if (a == 0)
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        else if (a == 1)
        {
            Application.Quit();
        }
    }

    public void OnNoClick()
    {
        MakeSure_Close();
        Pause_Close();
        Time.timeScale = 1;
    }

    // AddFish
    public void OnAddFishOKClicked()
    {
        AddFish_Close();
    }

    // Letter
    public void LoginLetterClick()
    {
        Letter_Show();
    }

    void OnLetterCloseClick()
    {
        Letter_Close();
    }
    #endregion
}