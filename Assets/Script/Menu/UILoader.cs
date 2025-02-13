using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoader : MonoBehaviour
{

    public GameObject targetObject;

    public void ToggleObject()
    {
        if(targetObject != null)
        {
            targetObject.SetActive(true);
        }

    }

}
