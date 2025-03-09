using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    // ���� ��� �������� ������������� �����
    public string unitName;
    public double hp;
    public double maxhp;
    public double damage;
    public int id_unit;
    public float attackInterval;
    public int team_id;
    public double repair_count;

    // ����� ��� �������� ������ ����� �� JSON
    public static UnitData LoadFromJson(string filePath)
    {
        try
        {
            // �������� ������������� �����
            if (File.Exists(filePath))
            {
                // ������ JSON �� �����
                string json = File.ReadAllText(filePath);

                // �������������� JSON � ������ UnitData
                UnitData data = JsonUtility.FromJson<UnitData>(json);

                Debug.Log("Unit data loaded from: " + filePath);
                return data;
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load unit data: " + e.Message);
            return null;
        }
    }
}