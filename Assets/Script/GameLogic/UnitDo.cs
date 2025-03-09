using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitDo : MonoBehaviour
{
    [SerializeField] public TMP_Text HPbar;
    [SerializeField] public TMP_Text Name;
    [SerializeField] public double hp = 15;
    [SerializeField] public double maxhp = 15;
    [SerializeField] public double damage = 0.2;
    [SerializeField] public int id_unit = 1;
    [SerializeField] public GameObject SelectObject;
    [SerializeField] public GameObject HPColorBar;
    [SerializeField] public float attackInterval = 1f;
    [SerializeField] public int team_id = 1;
    public double repair_count = 10;
    private float lastAttackTime = 0f;
    public string unit_name = string.Empty;
    [HideInInspector] public bool CanDoDamage = false;
    private bool CanDoExtraction = false;
    private bool CanDoRepair = false;
    [HideInInspector] public bool CanDoHeath = false;
    private UnitDo targetUnit;
    private SelectionManager selectionManager;
    private ObjectLogic objectlogic;
    private StructureLogic structurelogic;
    private Image image;
    public bool Select = false;

    private void Start()
    {
        determinant_id();
        selectionManager = SelectObject.GetComponent<SelectionManager>();
        image = HPColorBar.GetComponent<Image>();
        HPColorBar.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }
    private void LoadUnitData(string filePath)
    {
        // Загрузка данных из JSON
        UnitData data = UnitData.LoadFromJson(filePath);

        if (data != null)
        {
            // Применение загруженных данных
            unit_name = data.unitName;
            hp = data.hp;
            maxhp = data.maxhp;
            damage = data.damage;
            id_unit = data.id_unit;
            attackInterval = data.attackInterval;
            repair_count = data.repair_count;
        }
    }

    private void determinant_id()
    {
        switch (id_unit)
        {
            case 0:
                LoadUnitData("Assets/StreamingAssets/unitData_0.json");
                break;
            case 1:
                LoadUnitData("Assets/StreamingAssets/unitData_1.json");
                break;
            case 2:
                LoadUnitData("Assets/StreamingAssets/unitData_2.json");
                break;
            case 3:
                LoadUnitData("Assets/StreamingAssets/unitData_3.json");
                break;
            default:
                unit_name = "unit_id";
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            objectlogic = other.GetComponent<ObjectLogic>();

            CanDoExtraction = true;
        }
        if (other.gameObject.CompareTag("Structure"))
        {
            structurelogic = other.GetComponent<StructureLogic>();

            if (structurelogic.team_id != this.team_id)
            {
                CanDoDamage = true;
            }

            if (structurelogic.team_id == this.team_id && this.id_unit == 0 && structurelogic.hp < structurelogic.maxhp)
            {
                //Механика починки строителем
                CanDoRepair = true;
            }
        }
        if (other.gameObject.CompareTag("Unit"))
        {      
            targetUnit = other.GetComponent<UnitDo>();

            if (targetUnit.team_id != this.team_id)
            {
                CanDoDamage = true;
            }

            if (targetUnit.team_id == this.team_id && this.id_unit == 3 && targetUnit.hp < targetUnit.maxhp)
            {
                //Механика целителя
                CanDoHeath = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            CanDoExtraction = false;
        }
        if (other.gameObject.CompareTag("Structure"))
        {
            if (this.id_unit == 0)
            {
                CanDoRepair = false;
            }
            CanDoDamage = false;
        }
        if (other.gameObject.CompareTag("Unit"))
        {
            CanDoDamage = false;
            CanDoHeath = false;
        }
    }

    private void FixedUpdate()
    {
        if (targetUnit == null && CanDoDamage == true)
        {
            CanDoDamage = false;
        }

        if (CanDoDamage && targetUnit != null && Time.time - lastAttackTime >= attackInterval)
        {
            targetUnit.TakeDamage(damage);
            Debug.Log("Unit hp(d): " + hp);
            Debug.Log("Target Hp(d): " + targetUnit.hp);
            lastAttackTime = Time.time; // Обновляем время последней атаки
        }

        if (CanDoHeath &&  targetUnit != null && Time.time - lastAttackTime >= attackInterval && this.id_unit == 3)
        {
            targetUnit.TakeHeath(damage);
            lastAttackTime = Time.time;
        }

        if (CanDoRepair && structurelogic != null && Time.time - lastAttackTime >= attackInterval && this.id_unit == 0)
        {
            structurelogic.TakeRepair(repair_count);
            lastAttackTime = Time.time;
        }

        if (CanDoExtraction && objectlogic != null && Time.time - lastAttackTime >= attackInterval && objectlogic.gameObject.CompareTag("Tree"))
        {
            objectlogic.TakeDamage(damage);
            lastAttackTime = Time.time; 
        }
    }

    public void TakeDamage(double damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        if (IsSelected())
        {
            UpdateHPBar();
        }
    }

    public void TakeHeath(double damage)
    {
        if (hp < maxhp)
        {
            hp += damage;
        }
        if (!IsSelected())
        {
            UpdateHPBar();
        }
    }


    void UpdateHPBar()
    {
        HPbar.text = Convert.ToString(this.hp) + "/" + Convert.ToString(this.maxhp);
        if (this.hp <= this.maxhp)
        {
            image.color = Color.green;
        }
        if (this.hp <= (this.maxhp / 2))
        {
           image.color = Color.yellow;
        }
        if (this.hp <= (this.maxhp / 3))
        {
            image.color = Color.red; // Реализовать срезание хп (уменьшение панельки)
        }
    }

    void UpdateUnitName()
    {
        Name.text = unit_name;
    }

    private void Die()
    {
        selectionManager.selectedUnits.Remove(this);
        Destroy(gameObject);
        OnDeselect();
        
    }

    private bool IsSelected()
    {
        return selectionManager.selectedUnits.Contains(this);
    }

    public void OnSelect()
    {
        Select = true;
        HPColorBar.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        UpdateHPBar();
        UpdateUnitName();
    }

    public void OnDeselect()
    {
        Select = false;
        HPColorBar.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);
    }

}

