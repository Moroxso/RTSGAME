using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

public class UnitAI : MonoBehaviour
{
    [HideInInspector] public bool isManualControl = false;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float groupFormationDistance = 5f;

    private UnitDo unit;
    private NavMeshAgent agent;
    private UnitDo targetEnemy;
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
            // Проверяем, уничтожена ли текущая цель
            if (targetEnemy == null)
            {
                ResetTarget();
            }
            else if (targetEnemy != null) {
                
                MoveAndAttack();
            }
        }
        FindEnemies();
    }

    public void ResetTarget()
    {
        unit.CanDoDamage = false;
        isManualControl = false; // Возвращаем управление ИИ
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

    void GroupFormation()
    {
        // Поиск союзников в радиусе
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

        // Если союзников больше 3, формируем группы
        if (allies.Count > 3)
        {
            int groupSize = allies.Count / 3;
            int groupIndex = allies.IndexOf(unit);

            if (groupIndex < groupSize)
            {
                // Левый фланг
                Vector3 flankPosition = targetEnemy.transform.position - targetEnemy.transform.right * 5f;
                agent.SetDestination(flankPosition);
            }
            else if (groupIndex < groupSize * 2)
            {
                // Правый фланг
                Vector3 flankPosition = targetEnemy.transform.position + targetEnemy.transform.right * 5f;
                agent.SetDestination(flankPosition);
            }
            else
            {
                // Центр
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
        else
        {
            // Если союзников мало, просто двигаемся к врагу
            agent.SetDestination(targetEnemy.transform.position);
        }
    }
}