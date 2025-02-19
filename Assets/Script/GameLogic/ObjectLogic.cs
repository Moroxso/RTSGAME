using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectLogic : MonoBehaviour
{

    [SerializeField] public TMP_Text ScoreText;
    [SerializeField] private double HPObject = 4;
    private int treecount = 0;

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
        treecount++;
        UpdataScoreText();
    }

    void UpdataScoreText()
    {
        ScoreText.text = Convert.ToString(treecount);
    }
}
