using System;
using UnityEngine;

public class AlienButton : MonoBehaviour
{
    [SerializeField] KeyCode key;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(key))
        {
            collision.GetComponent<GameObject>().SetActive(false);
        }
    }
}