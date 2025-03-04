using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StructureLogic : MonoBehaviour
{
    [SerializeField] public TMP_Text HPbar;
    [SerializeField] public TMP_Text Name;
    [SerializeField] private GameObject HPColorBar;
    [SerializeField] private int structure_id = 0;
    [SerializeField] public double hp = 100;
    [SerializeField] public double maxhp = 100;
    [SerializeField] public int team_id = 1;
    [SerializeField] GameObject SpawnScript;
    private Spawn spawn;
    private string structure_name = string.Empty;
    [SerializeField] GameObject unit;
    [SerializeField] GameObject SpawnPoint;
    private Transform newtransform;
    [SerializeField] GameObject ResObjectLogic;
    private ResourceLogic resourceLogic;

    [SerializeField] private float Interval = 5f;
    private float lastTime = 0f;

    private void Start()
    {
        determinant_id();
        spawn = SpawnScript.GetComponent<Spawn>();
        newtransform = SpawnPoint.GetComponent<Transform>();
        resourceLogic = ResObjectLogic.GetComponent<ResourceLogic>();
    }



    private void determinant_id()
    {
        switch (structure_id)
        {
            case 0:
                structure_name = "Ратуша";
                break;
            case 1:
                structure_name = "Ферма";
                break;
            case 2:
                structure_name = "Лагерь";
                break;
            case 3:
                structure_name = "Кузница";
                break;
            default:
                structure_name = "structure_id";
                break;
        }
    }

    public void StructureDoButton1()
    {
        if (resourceLogic.Tree > 0)
        {
            spawn.SpawnMetod(unit, newtransform.position, newtransform.rotation);
            resourceLogic.Tree--;
            resourceLogic.UpdateScoreText();
        }
    }

    private void FixedUpdate()
    {
        if (structure_id == 0)
        {
        }
        if (structure_id == 1)
        {
            if (Time.time - lastTime >= Interval)
            {
                resourceLogic.Bread++;
                resourceLogic.UpdateScoreText();
                lastTime = Time.time;
            }
        }
    }



    public void TakeRepair(double repair)
    {
        if (hp < maxhp)
        {
            hp += repair;
        }
    }

    public void TakeDamage(double damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    void UpdateHPBar()
    {
        HPbar.text = Convert.ToString(this.hp) + "/" + Convert.ToString(this.maxhp);
    }

    void UpdateUnitName()
    {
        Name.text = structure_name;
    }

    public void OnSelect()
    {
        HPColorBar.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        UpdateHPBar();
        UpdateUnitName();
    }

    public void OnDeselect()
    {
        HPColorBar.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }

}
