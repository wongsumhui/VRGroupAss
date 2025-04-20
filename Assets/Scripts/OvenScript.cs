using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Data;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Security.Cryptography;

[RequireComponent(typeof(AudioSource))]
public class OvenScript : MonoBehaviour
{
    //Pointer to UIs
    [SerializeField] private GameObject ovenCookUI;
    [SerializeField] private GameObject ovenInteractUI;
    [SerializeField] private GameObject ovenPickupUI;

    //Pointer to items
    [SerializeField] private GameObject oven;
    [SerializeField] private AudioClip ovenClip;
    [SerializeField] private AudioSource ovenSource;
    [SerializeField] private GameObject silverTray;
    [SerializeField] private GameObject roastedChicken;

    [SerializeField] private float distanceTillShowUI;
    [SerializeField] private float audioStartTime;
    [SerializeField] private float timeBeforeEnd = 4;

    [SerializeField] private GameObject pointerManager;
    [SerializeField] private PointerScript pointerScript;
    

    private GameObject currentUI;

    private bool isActivated = false;
    private bool canPickup = false;
    private bool isOvenDoorClosed = true;

    [SerializeField] float bakeTime;
    private float currentTime;

    private List<string> requiredIngredients = new List<string> { "Garlic Butter", "Chicken"};
    private List<string> currentIngredients = new List<string>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUI = ovenInteractUI; //Set currentUI pointer to oven UI
        ovenSource = GetComponent<AudioSource>();
        if (ovenClip != null)
        {
            ovenSource.clip = ovenClip;
            audioStartTime = ovenClip.length - timeBeforeEnd - bakeTime;
        }
        roastedChicken.SetActive(false);
        silverTray.SetActive(false);

       
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
                pointerScript.currentTargetIndex++;
                
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
                    OvenActivated();
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
                OvenPickup();
            }
        }
    }

    void checkDistance()
    {
        if (Vector3.Distance(Player.player.transform.position, oven.transform.position) <= distanceTillShowUI && isOvenDoorClosed)
        {
            currentUI.SetActive(true);
        }
        else
        {
            currentUI.SetActive(false);
        }
    }

    void OvenActivated()
    {
        //Check required materials
        if (!HasAllRequiredIngredients()) return;

        if (ovenInteractUI.activeSelf == true)
        {
            isActivated = true;
            ovenInteractUI.SetActive(false);

            ovenCookUI.SetActive(true);
            ovenSource.time = audioStartTime;
            ovenSource.Play();

            currentIngredients.Clear();
            //Perform blend actions
            BlendItems();

        }
    }

    void OvenPickup()
    {
        if (ovenPickupUI.activeSelf == true)
        {
            currentUI = ovenInteractUI;
            canPickup = false;
            ovenPickupUI.SetActive(false);

            //Make chicken appear
            roastedChicken.SetActive(true);
            silverTray.SetActive(true);
        }
    }
    void BlendItems()
    {
        currentTime += Time.deltaTime;
        float uiValue = currentTime / bakeTime;
        if (uiValue > 1) uiValue = 1;
        ovenCookUI.GetComponent<Slider>().value = uiValue;

        if (uiValue == 1)
        {
            canPickup = true;
            currentUI = ovenPickupUI;

            //Enable interact button again
            ovenCookUI.SetActive(false);
            isActivated = false;
            currentTime = 0;
        }
    }

    public void SetOvenDoor(bool isOvenDoor)
    {
        isOvenDoorClosed = isOvenDoor;
    }
}
