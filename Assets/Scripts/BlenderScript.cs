using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Data;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(AudioSource))]
public class BlenderScript : MonoBehaviour
{
    //Pointer to UIs
    [SerializeField] GameObject blenderCookUI;
    [SerializeField] GameObject blenderInteractUI;
    [SerializeField] GameObject blenderPickupUI;

    //Pointer to items
    [SerializeField] GameObject blender; 
    [SerializeField] AudioClip blenderClip;
    [SerializeField] AudioSource blenderSource;
    [SerializeField] float distanceTillShowUI;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] XRSocketInteractor baseSocketInteractor;
    [SerializeField] XRSocketInteractor jugSocketInteractor;

    [SerializeField] GameObject GarlicButter;

    private GameObject currentUI;

    private bool isActivated = false;
    private bool canPickup = false;
    private bool isJugPlaced = false;
    private bool isJugCoverPlaced = false;

    [SerializeField] float blendTime;
    private float currentTime;

    private List<string> requiredIngredients = new List<string> { "Onion", "Butter" };
    private List<string> currentIngredients = new List<string> ();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUI = blenderInteractUI; //Set currentUI pointer to blender UI
        currentUI.SetActive (false);
        blenderSource = GetComponent<AudioSource>();
        if (blenderClip != null) blenderSource.clip = blenderClip;

        if (baseSocketInteractor != null)
        {
            baseSocketInteractor.selectEntered.AddListener(OnJugPlaced);
            baseSocketInteractor.selectExited.AddListener(OnJugRemoved);
        }

        if (jugSocketInteractor != null)
        {
            jugSocketInteractor.selectEntered.AddListener(OnCoverPlaced);
            jugSocketInteractor.selectExited.AddListener(OnCoverRemoved);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            string ingredientName = other.gameObject.name; // Assuming name is set correctly

            if (!requiredIngredients.Contains(ingredientName)) return;

            if (!currentIngredients.Contains(ingredientName))
            {
                currentIngredients.Add(ingredientName);
            }
            Destroy(other.gameObject); // Remove ingredient from scene
        }
    }

    private bool HasAllRequiredIngredients()
    {
        foreach (string ingredient in requiredIngredients)
        {
            if (!currentIngredients.Contains(ingredient))
            {
                Debug.Log("No have" + ingredient);
                return false;
            }
            Debug.Log("Contains" + ingredient);
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPickup)
        {
            if (!isActivated)
            {
                checkDistance();
                if (Player.leftButtonPressed)
                {
                    BlenderActivated();
                }
            }
            else
            {
                BlendItems();
            }
        }
        else
        {
            //Enable Pickup GUI and interactions
            checkDistance();

            if (Player.leftButtonPressed)
            {
                BlenderPickup();
            }
        }
    }

    void checkDistance()
    {
        if (!isJugCoverPlaced || !isJugPlaced) return;
        Debug.Log("Jug Cover and Jug is true");

        if (Vector3.Distance(Player.player.transform.position, blender.transform.position) <= distanceTillShowUI)
        {
            currentUI.SetActive(true);
        }
        else
        {
            currentUI.SetActive(false);
        }
    }

    void BlenderActivated()
    {
        //Check required materials
        if (!HasAllRequiredIngredients()) return;

        if (blenderInteractUI.activeSelf == true)
        {
            grabInteractable.enabled = false;
            isActivated = true;
            blenderInteractUI.SetActive(false);

            blenderCookUI.SetActive(true);
            blenderSource.Play();

            currentIngredients.Clear();
            //Perform blend actions
            BlendItems();

        }
    }

    void BlenderPickup()
    {
        if (blenderPickupUI.activeSelf == true)
        {
            currentUI = blenderInteractUI;
            canPickup = false;
            blenderPickupUI.SetActive(false);
        }
    }
    void BlendItems()
    {
        currentTime += Time.deltaTime;
        float uiValue = currentTime / blendTime;
        if (uiValue > 1) uiValue = 1;
        blenderCookUI.GetComponent<Slider>().value = uiValue;

        if (uiValue == 1)
        {
            //Maybe player pickup
            canPickup = true;
            currentUI = blenderPickupUI;
            Debug.Log("Hello from the other side");
            //Enable interact button again
            blenderCookUI.SetActive(false);
            isActivated = false;
            currentTime = 0;
            grabInteractable.enabled = true;

            GarlicButter.SetActive(true);
        }
    }

    // Called when the Blender Jug is placed on the Blender Base
    public void OnJugPlaced(SelectEnterEventArgs args)
    {
        Debug.Log("Placed");
        if (args.interactableObject.transform.CompareTag("BlenderJug"))  // Ensure only the jug is detected
        {
            isJugPlaced = true;
        }
    }

    // Called when the Blender Jug is removed from the Blender Base
    public void OnJugRemoved(SelectExitEventArgs args)
    {
        Debug.Log("Byeeee");
        if (args.interactableObject.transform.CompareTag("BlenderJug"))
        {
            isJugPlaced = false;
            blenderInteractUI.SetActive(false); // Hide UI when jug is removed
        }
    }

    // Called when the Blender Jug is placed on the Blender Base
    public void OnCoverPlaced(SelectEnterEventArgs args)
    {
        Debug.Log("Cover placed");
        if (args.interactableObject.transform.CompareTag("BlenderCover"))
        {
            isJugCoverPlaced = true;
        }
    }

    // Called when the Blender Jug is removed from the Blender Base
    public void OnCoverRemoved(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("BlenderCover"))
        {
            isJugCoverPlaced = false;
            blenderInteractUI.SetActive(false);
        }
    }
}
