using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayDisplay : MonoBehaviour
{
    private Image overlayImage;
    private RoundController roundController;

    // Start is called before the first frame update
    void Start()
    {
        // Get image
        overlayImage = GetComponent<Image>();

        // Initialize RoundController
        roundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        while (true)
        {
            // Show on event or game set
            yield return new WaitUntil(() => roundController.PlayerTurn == -1 || roundController.WinLoseStatus != "continue");
            overlayImage.enabled = true;

            yield return new WaitUntil(() => roundController.PlayerTurn != -1 && roundController.WinLoseStatus == "continue");
            overlayImage.enabled = false;
        }
    }
}
