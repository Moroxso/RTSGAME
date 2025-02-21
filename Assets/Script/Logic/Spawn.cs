using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public void SpawnMetod(GameObject Prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate(Prefab, position, rotation);
        newObject.name = Prefab.name;
    }

}
