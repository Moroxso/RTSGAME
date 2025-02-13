using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiUnloader : MonoBehaviour
{
    public GameObject targetObject;

    public void ToggleObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

    }
}
