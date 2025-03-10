using System;
using System.Collections.Generic;
using UnityEngine;

public class AlienButton : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] List<GameObject> ememys;

    void Update()
    {
        int len = ememys.Count;
        for (int i = 0; i < len; i++)
        {
            var enemy = ememys[i];
            if (enemy.activeSelf)
            {
                GetTrigger(enemy);
            }

            if (enemy.transform.position.y < -4f)
            {
                enemy.SetActive(false);
            }
        }
    }

    public void GetTrigger(GameObject enemy)
    {
        var pos = transform.position;
        var enemyPos = enemy.transform.position;
        var dis = Vector2.Distance(pos, enemyPos);

        if (dis < 1f)
        {
            if (Input.GetKeyDown(key))
            {
                enemy.SetActive(false);
            }
        }
    }
}