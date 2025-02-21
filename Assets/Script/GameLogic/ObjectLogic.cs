using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectLogic : MonoBehaviour
{

    private ResourceLogic resourceLogic;
    [SerializeField] private GameObject ResObjectLogic;
    [SerializeField] private double HPObject = 4;
    [SerializeField] private int object_id = 1;

    private void Start()
    {
        resourceLogic = ResObjectLogic.GetComponent<ResourceLogic>();
    }


    public void TakeDamage(double damage)
    {
        HPObject -= damage;
        if (HPObject <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        switch (object_id) {
            case 1:
                resourceLogic.Tree++;
                break;
            case 2:
                resourceLogic.Stone++;
                break;
            case 3:
                resourceLogic.Gold++;
                break;
            case 4:
                resourceLogic.Iron++;
                break;
            default:
                //...
                break;
        }
        resourceLogic.UpdateScoreText();
    }
}
