using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceLogic : MonoBehaviour
{
    [SerializeField] public TMP_Text TreeCount;
    [SerializeField] public TMP_Text StoneCount;
    [SerializeField] public TMP_Text GoldCount;
    [SerializeField] public TMP_Text HumanCount;
    [SerializeField] public TMP_Text BreadCount;
    [SerializeField] public TMP_Text IronCount;
    [SerializeField] public int Tree = 0;
    [SerializeField] public int Stone = 0;
    [SerializeField] public int Gold = 0;
    [SerializeField] public int Human = 0;
    [SerializeField] public int Bread = 0;
    [SerializeField] public int Iron = 0;

    private void Start()
    {
        UpdateScoreText();
    }


    public void UpdateScoreText()
    {
        TreeCount.text = Convert.ToString(Tree);
        StoneCount.text = Convert.ToString(Stone);
        GoldCount.text = Convert.ToString(Gold);
        HumanCount.text = Convert.ToString(Human);
        BreadCount.text = Convert.ToString(Bread);
        IronCount.text = Convert.ToString(Iron);
    }




}
