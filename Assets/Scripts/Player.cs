using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator anim;

    public bool isFishing;

    #region 钓鱼
    public void EnterFishing()
    {
        anim.SetBool("isFishing", true);
        isFishing = true;
    }

    public void ExitFishing()
    {
        anim.SetBool("isFishing", false);
        isFishing = false;
    }
    #endregion
}
