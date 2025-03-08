using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    UIContext uiContext;
    Canvas canvas;

    [SerializeField] Tip tip;
    [SerializeField] Menu menu;
    [SerializeField] Think think;
    [SerializeField] Pause pause;
    [SerializeField] Bag bag;
    [SerializeField] MakeSure makeSure;

    [SerializeField] Letter letter;

    public int foodID;

    public void Ctor(Canvas canvas)
    {
        uiContext = new UIContext();
        this.canvas = canvas;

        foodID = -1;
    }

    #region Tip
    public void Tip_Show(string text)
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
        }
        ui.Ctor();
        ui.ClearAll();

        var elements = uiContext.bagElements;
        ui.Show(elements);
        uiContext.bag = ui;
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
        Debug.Log("全部");
    }

    public void OnFoodClick()
    {
        Debug.Log("食物");
    }

    public void OnFishClick()
    {
        Debug.Log("鱼");
    }

    public void OnLastPageClick()
    {
        Debug.Log("上一页");
    }

    public void OnNextPageClick()
    {
        Debug.Log("下一页");
    }

    public void OnCloseClick()
    {
        Bag_Close();
    }

    public void OnPlaidClick(int id)
    {
        if (foodID != -1)
        {
            Debug.Log("已经有鱼饵了");
            return;
        }

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