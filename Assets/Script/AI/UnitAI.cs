using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    [HideInInspector] public bool isManualControl = false;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float groupFormationDistance = 5f;

    private UnitDo unit;
    private NavMeshAgent agent;
    private UnitDo targetEnemy;
    private UnitDo targetAlien;
    private List<UnitDo> allies = new List<UnitDo>();

    void Start()
    {
        unit = GetComponent<UnitDo>();
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (isManualControl == false)
        {
            // ���������, ���������� �� ������� ����
            if (targetEnemy == null)
            {
                ResetTarget();
            }
            else if (targetEnemy != null) {
                
                MoveAndAttack();
            }

            if (unit.id_unit == 3)
            {
                if (targetAlien == null && targetEnemy == null)
                {
                    ResetTarget();
                    FindAliens();
                }
                else if (targetAlien != null && targetEnemy == null && targetAlien.hp < targetAlien.maxhp)
                {
                    MoveAndHeath();
                }
            }
        }
        FindEnemies();
    }

    public void ResetTarget()
    {
        unit.CanDoDamage = false;
        unit.CanDoHeath = false;
        isManualControl = false; // ���������� ���������� ��
    }

    void FindEnemies()
    {
        UnitDo[] allUnits = FindObjectsOfType<UnitDo>();
        float closestDistance = detectionRange;
        targetEnemy = null;

        foreach (UnitDo enemy in allUnits)
        {
            if (enemy.team_id != unit.team_id && enemy.hp > 0)
            {
                isManualControl = false;
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }
    }

    void FindAliens()
    {
        UnitDo[] allUnits = FindObjectsOfType<UnitDo>();
        float closestDistance = detectionRange;
        targetAlien = null;

        foreach (UnitDo alien in allUnits)
        {
            if (alien.team_id == unit.team_id && alien.hp > 0 && alien.hp < alien.maxhp && targetEnemy == null)
            {
                isManualControl = false;
                float distance = Vector3.Distance(transform.position, alien.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetAlien = alien;
                }
            }
        }
    }

    void MoveAndAttack()
    {
        if (targetEnemy == null)
        {
            unit.CanDoDamage = false;
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.transform.position);

        if (distanceToEnemy <= attackRange)
        {
            unit.CanDoDamage = true;
        }
        else
        {
            agent.SetDestination(targetEnemy.transform.position);
            unit.CanDoDamage = false;
        }

        GroupFormation();
    }

    void MoveAndHeath()
    {
        if (targetAlien == null)
        {
            unit.CanDoHeath = false;
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, targetAlien.transform.position);

        if (distanceToEnemy <= attackRange)
        {
            unit.CanDoHeath = true;
        }
        else
        {
            agent.SetDestination(targetAlien.transform.position);
            unit.CanDoHeath = false;
        }
    }

    void GroupFormation()
    {
        // ����� ��������� � �������
        UnitDo[] allUnits = FindObjectsOfType<UnitDo>();
        allies.Clear();

        foreach (UnitDo ally in allUnits)
        {
            if (ally.team_id == unit.team_id && ally != unit)
            {
                float distance = Vector3.Distance(transform.position, ally.transform.position);
                if (distance < groupFormationDistance)
                {
                    allies.Add(ally);
                }
            }
        }

        // ���� ��������� ������ 3, ��������� ������
        if (allies.Count > 3)
        {
            int groupSize = allies.Count / 3;
            int groupIndex = allies.IndexOf(unit);

            if (groupIndex < groupSize)
            {
                // ����� �����
                Vector3 flankPosition = targetEnemy.transform.position - targetEnemy.transform.right * 5f;
                agent.SetDestination(flankPosition);
            }
            else if (groupIndex < groupSize * 2)
            {
                // ������ �����
                Vector3 flankPosition = targetEnemy.transform.position + targetEnemy.transform.right * 5f;
                agent.SetDestination(flankPosition);
            }
            else
            {
                // �����
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
        else
        {
            // ���� ��������� ����, ������ ��������� � �����
            agent.SetDestination(targetEnemy.transform.position);
        }
    }
}