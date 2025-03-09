using System;
using UnityEngine;
using UnityEngine.UI;

public class AddFish : MonoBehaviour
{
    [SerializeField] Text txt_Fish;
    [SerializeField] Image img_Fish;
    [SerializeField] Button btn_OK;

    public Action OnOKClicked;

    public void Ctor()
    {
        btn_OK.onClick.AddListener(() => OnOKClicked?.Invoke());
    }

    public GameObject Spawn(Canvas canvas)
    {
        GameObject go = Instantiate(gameObject, canvas.transform);
        return go;
    }

    public void Show(string fishName, Sprite fishImage)
    {
        SetFishName(fishName);
        SetFishImage(fishImage);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    void SetFishName(string fishName)
    {
        txt_Fish.text = string.Format("钓上了 <color=#990000><b>{0}</b></color>", fishName);
    }

    void SetFishImage(Sprite fishImage)
    {
        if (fishImage == null)
        {
            img_Fish.enabled = false;
            return;
        }

        img_Fish.sprite = fishImage;
        img_Fish.SetNativeSize();
    }
}