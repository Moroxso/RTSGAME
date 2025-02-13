using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public TMP_Text ScoreText;
    private int treecount = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            other.gameObject.SetActive(false);
            treecount++;
            UpdataScoreText();
        }
    }

    void UpdataScoreText()
    {
        ScoreText.text = Convert.ToString(treecount);
    }
}
