using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject resourcemanager;
    private ResourceLogic resourceLogic;
    public Button button1;


    private void Start()
    {
        resourceLogic = resourcemanager.GetComponent<ResourceLogic>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (resourceLogic.Tree > 0)
        {
            button1.interactable = true;
        }
        else if (resourceLogic.Tree <= 0)
        {
            button1.interactable = false;
        }
    }
}
