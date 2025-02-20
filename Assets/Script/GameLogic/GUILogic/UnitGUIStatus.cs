using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGUIStatus : MonoBehaviour
{
    [SerializeField] public GameObject unite;
    private UnitDo unit;


    private void Start()
    {
        unit = unite.GetComponent<UnitDo>();
        determinate_team_id();
    }

    private void determinate_team_id()
    {
        Renderer render = gameObject.GetComponent<Renderer>();
        switch(unit.team_id)
        {
            case 1:
                render.material.color = Color.green;
                break;
            case 2:
                render.material.color = Color.red;
                break;
            default:
                render.material.color = Color.white;
                break;
        }
    }




}
