using UnityEngine;
using System.Collections;

public class MeatCooker : MonoBehaviour
{
    [Header("Color Settings")]
    public Color rawColor = new Color(1f, 0.5f, 0.5f);
    public Color cookedColor = new Color(0.4f, 0.2f, 0.1f);
    public Color burntColor = new Color(0.1f, 0.05f, 0.05f);

    [Header("Cooking Time")]
    public float cookTime = 5f;
    public float overCookTime = 3f;

    [Header("Smoke Effect")]
    public ParticleSystem smokeEffect;

    [Header("Cooking Sound")]
    public AudioSource cookSound;

    [Header("Auto Move Settings")]
    public Transform targetAttachPoint; // Target location to move the meat after cooking
    public float delayBeforeMove = 5f;   // Delay before auto-moving after burning
    public float moveDuration = 1f;      // Duration of the movement

    private Coroutine cookingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            Renderer meatRenderer = other.GetComponent<Renderer>();
            MeatStatus status = other.GetComponent<MeatStatus>();

            if (meatRenderer != null && status != null && !status.isCooked)
            {
                cookingCoroutine = StartCoroutine(CookMeat(other.gameObject, meatRenderer, status));

                if (cookSound != null && !cookSound.isPlaying)
                {
                    cookSound.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            if (cookingCoroutine != null)
            {
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null;
            }

            if (smokeEffect != null)
            {
                smokeEffect.Stop();
            }

            if (cookSound != null && cookSound.isPlaying)
            {
                cookSound.Stop();
            }
        }
    }

    private IEnumerator CookMeat(GameObject meatObj, Renderer meatRenderer, MeatStatus status)
    {
        Material mat = meatRenderer.material;
        string colorProperty = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";

        float elapsed = 0f;
        mat.SetColor(colorProperty, rawColor);

        // Phase 1: Raw → Cooked
        while (elapsed < cookTime)
        {
            elapsed += Time.deltaTime;
            Color newColor = Color.Lerp(rawColor, cookedColor, elapsed / cookTime);
            mat.SetColor(colorProperty, newColor);
            yield return null;
        }

        mat.SetColor(colorProperty, cookedColor);
        status.isCooked = true;

        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        // Wait before starting the burn phase
        yield return new WaitForSeconds(overCookTime);

        // Phase 2: Cooked → Burnt
        float burntElapsed = 0f;
        float burnDuration = 2f;

        while (burntElapsed < burnDuration)
        {
            burntElapsed += Time.deltaTime;
            Color burnt = Color.Lerp(cookedColor, burntColor, burntElapsed / burnDuration);
            mat.SetColor(colorProperty, burnt);
            yield return null;
        }

        mat.SetColor(colorProperty, burntColor);

        // Wait before auto-moving the meat
        yield return new WaitForSeconds(delayBeforeMove);

        if (targetAttachPoint != null)
        {
            // Force the meat to be released from the socket if still attached
            var interactable = meatObj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (interactable != null && interactable.firstInteractorSelecting != null)
            {
                interactable.interactionManager.SelectExit(interactable.firstInteractorSelecting, interactable);
            }

            // Start smooth movement to target position
            StartCoroutine(MoveToTarget(meatObj.transform, targetAttachPoint));
        }
    }

    private IEnumerator MoveToTarget(Transform objTransform, Transform target)
    {
        Vector3 startPos = objTransform.position;
        Quaternion startRot = objTransform.rotation;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float lerpT = Mathf.Clamp01(t / moveDuration);
            objTransform.position = Vector3.Lerp(startPos, target.position, lerpT);
            objTransform.rotation = Quaternion.Slerp(startRot, target.rotation, lerpT);
            yield return null;
        }

        // Snap to final position
        objTransform.position = target.position;
        objTransform.rotation = target.rotation;
    }
}
