using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
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

    void Update()
    {
        if (unit.team_id == 0 || unit.isManualControl) return;

        // Проверяем, уничтожена ли текущая цель
        if (targetEnemy != null && targetEnemy.hp <= 0)
        {
            ResetTarget();
        }

        FindEnemies();
        MoveAndAttack();
    }

    public void ResetTarget()
    {
        targetEnemy = null;
        agent.isStopped = true;
        unit.CanDoDamage = false;
        unit.isManualControl = false; // Возвращаем управление ИИ
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
            agent.isStopped = true;
            unit.CanDoDamage = false;
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.transform.position);

        if (distanceToEnemy <= attackRange)
        {
            agent.isStopped = true;
            unit.CanDoDamage = true;
        }
        else
        {
            agent.isStopped = false;
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