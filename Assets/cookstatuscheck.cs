using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class CookedOnlySocket : XRSocketInteractor
{
    public GameObject invalidUI; // 拖 UI 提示对象进来（比如 Text Canvas）
    public float uiDisplayDuration = 2f;

    private Coroutine uiCoroutine;

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (!base.CanSelect(interactable))
            return false;

        var meatObj = interactable.transform.gameObject;
        MeatStatus meatStatus = meatObj.GetComponent<MeatStatus>();

        if (meatStatus != null && !meatStatus.isCooked)
        {
            ShowInvalidUI();
            return false;
        }

        return meatStatus != null && meatStatus.isCooked;
    }

    private void ShowInvalidUI()
    {
        if (invalidUI == null)
            return;

        // 避免重复开启 coroutine
        if (uiCoroutine != null)
            StopCoroutine(uiCoroutine);

        uiCoroutine = StartCoroutine(ShowUIRoutine());
    }

    private IEnumerator ShowUIRoutine()
    {
        invalidUI.SetActive(true);
        yield return new WaitForSeconds(uiDisplayDuration);
        invalidUI.SetActive(false);
        uiCoroutine = null;
    }
}
