using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketInteractorFilter : XRSocketInteractor
{
    [SerializeField] private string requiredTag; // Tag for allowed objects

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return interactable.CompareTag(requiredTag);
    }
}
