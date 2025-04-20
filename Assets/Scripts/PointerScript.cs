using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class PointerScript : MonoBehaviour
{
    /*
    [SerializeField] private GameObject butter;
    [SerializeField] private GameObject onion;
    [SerializeField] private GameObject garlic;
    [SerializeField] private GameObject blender;
    [SerializeField] private GameObject chicken;
    [SerializeField] private GameObject oven;
    [SerializeField] private GameObject plate;*/

    [SerializeField] private GameObject[] targets; // butter, onion, garlic, etc.
    [SerializeField] private GameObject pointerArrow; // Your arrow object

    public int currentTargetIndex = 0;

    private void Start()
    {
        if (targets.Length > 0)
        {
            PointToCurrentTarget();
            SubscribeToInteractionEvents();
        }
    }


    public void PointToCurrentTarget()
    {
        if (currentTargetIndex < targets.Length)
        {
            Vector3 newPos = targets[currentTargetIndex].transform.position + Vector3.up * 0.6f; // offset above target
            pointerArrow.transform.parent.transform.position = newPos;
            //pointerArrow.transform.rotation = Quaternion.Euler(90, 0, 0); // Make sure it points down
        }
        else
        {
            pointerArrow.SetActive(false); // Done pointing
        }
    }

    void SubscribeToInteractionEvents()
    {
        foreach (GameObject target in targets)
        {
            XRGrabInteractable grab = target.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.selectEntered.AddListener(OnObjectGrabbed);
            }
        }
    }

    void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        GameObject grabbedObj = args.interactableObject.transform.gameObject;

        if (grabbedObj == targets[currentTargetIndex])
        {
            currentTargetIndex++;
            PointToCurrentTarget();
        }
    }
}
