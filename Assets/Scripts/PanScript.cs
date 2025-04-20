using UnityEngine;
using UnityEngine.UI;

public class PanScript : MonoBehaviour
{
    // Pointer to UIs
    [SerializeField] GameObject panCookUI;
    [SerializeField] GameObject panInteractUI;
    [SerializeField] GameObject panPickupUI;

    // Pointer to items
    [SerializeField] GameObject pan;
    [SerializeField] AudioClip panClip;
    [SerializeField] AudioSource panSource;
    [SerializeField] float distanceTillShowUI;
    private GameObject currentUI;

    private bool isActivated = false;
    private bool canPickup = false;

    [SerializeField] float grillTime;
    [SerializeField] float timeBeforeOvercook;
    private float currentTime;

    private bool isShaking = false;
    private bool isOvercooked = false;

    // Start is called before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentUI = panInteractUI; // Set currentUI pointer to pan UI
        panSource = GetComponent<AudioSource>();
        if (panClip != null) panSource.clip = panClip;
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
                    panActivated();
                }
            }
            else
            {
                GrillItems();
            }
        }
        else
        {
            // Enable Pickup GUI and interactions
            checkDistance();

            if (Player.leftButtonPressed)
            {
                panPickup();
            }
        }
    }

    void checkDistance()
    {
        if (Vector3.Distance(Player.player.transform.position, pan.transform.position) <= distanceTillShowUI)
        {
            currentUI.SetActive(true);
        }
        else
        {
            currentUI.SetActive(false);
        }
    }

    void panActivated()
    {
        // Check required materials
        if (!CheckRequiredMaterials()) return;

        if (panInteractUI.activeSelf == true)
        {
            isActivated = true;
            panInteractUI.SetActive(false);
            panCookUI.SetActive(true);
            panSource.Play();

            // Start cooking
            GrillItems();
        }
    }

    void panPickup()
    {
        if (panPickupUI.activeSelf == true)
        {
            currentUI = panInteractUI;
            canPickup = false;
            panPickupUI.SetActive(false);

            if (isOvercooked)
            {
                Debug.Log("The food is burnt!");
            }
            else
            {
                Debug.Log("Food is cooked perfectly!");
            }

            // Remove ingredients and add final item
            PlayerInventory.RemoveItem("Onions");
            PlayerInventory.RemoveItem("Garlic");
            PlayerInventory.RemoveItem("Butter");
            PlayerInventory.RemoveItem("LemonJuice");

            // Add cooked or burnt food
            PlayerInventory.AddItem(isOvercooked ? "BurntFood" : "GarlicButter");
        }
    }

    bool CheckRequiredMaterials()
    {
        return PlayerInventory.HasItem("Onions") &&
               PlayerInventory.HasItem("Garlic") &&
               PlayerInventory.HasItem("Butter") &&
               PlayerInventory.HasItem("LemonJuice");
    }

    void GrillItems()
    {
        currentTime += Time.deltaTime;
        float uiValue = currentTime / grillTime;
        panCookUI.GetComponent<Slider>().value = Mathf.Clamp01(uiValue);

        // Start shaking effect when cooking time exceeds grillTime
        if (currentTime > grillTime && !isShaking)
        {
            isShaking = true;
            StartShakeEffect();
        }

        // Overcooked condition
        if (currentTime > (grillTime + timeBeforeOvercook))
        {
            isOvercooked = true;
            canPickup = true;
            currentUI = panPickupUI;

            Debug.Log("Food is overcooked!");

            // Stop UI and reset state
            panCookUI.SetActive(false);
            isActivated = false;
            currentTime = 0;
        }
    }

    void StartShakeEffect()
    {
        Debug.Log("Starting shake effect...");

        // Shake the cooking UI
        LeanTween.moveLocalX(panCookUI, panCookUI.transform.localPosition.x + 10f, 0.1f)
            .setLoopPingPong(5)
            .setEaseShake();
    }

}
