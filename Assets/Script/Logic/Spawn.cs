using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public void SpawnMetod(GameObject Prefab, Vector3 position, Quaternion rotation, Transform UnitParent)
    {
        GameObject newObject = Instantiate(Prefab, position, rotation, UnitParent);
        newObject.name = Prefab.name;
    }

}
