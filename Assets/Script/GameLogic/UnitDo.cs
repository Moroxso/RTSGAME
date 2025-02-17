using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitDo : MonoBehaviour
{
    [SerializeField] public TMP_Text ScoreText;
    [SerializeField] private double hp = 15;
    [SerializeField] private double damage = 0.2;
    private int treecount = 0;
    private bool CanDoDamage = false;
    private UnitDo targetUnit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            other.gameObject.SetActive(false);
            treecount++;
            UpdataScoreText();
        }

        if (other.gameObject.CompareTag("Unit"))
        {
                CanDoDamage = true;
            targetUnit = other.GetComponent<UnitDo>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            CanDoDamage = false;
        }
    }


    private void FixedUpdate()
    {
        if (CanDoDamage && targetUnit != null) {
            targetUnit.TakeDamage(damage);
            Debug.Log(hp);
            Debug.Log(targetUnit.hp);
        }
    }

    public void TakeDamage(double damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Die();
        }
    }


    void UpdataScoreText()
    {
        ScoreText.text = Convert.ToString(treecount);
    }

     private void Die()
    {
        Destroy(gameObject);
    }



}
