using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private bool responsiveWidth;
    public TMP_Text h1Text;
    public Image comboArrow;
    public TMP_Text prevStratText;
    public TMP_Text nextStratText;
    public TMP_Text contentText;

    private LayoutElement layoutElement;
    private RectTransform rectTransform;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Set the headings and contents
    public void SetText(string content, string h1 = "", string prevStrat = "", string nextStrat = "")
    {
        // H1
        if (string.IsNullOrEmpty(h1))
        {
            h1Text.gameObject.SetActive(false);
        }
        else
        {
            h1Text.gameObject.SetActive(true);
            h1Text.text = h1;
        }

        // H2
        if (string.IsNullOrEmpty(prevStrat) || string.IsNullOrEmpty(nextStrat))
        {
            comboArrow.gameObject.SetActive(false);
            prevStratText.gameObject.SetActive(false);
            nextStratText.gameObject.SetActive(false);
        }
        else
        {
            comboArrow.gameObject.SetActive(true);

            prevStratText.gameObject.SetActive(true);
            prevStratText.text = prevStrat;

            nextStratText.gameObject.SetActive(true);
            nextStratText.text = nextStrat;
        }

        // Content
        contentText.text = content;
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce width if no wrap
        if (Application.isEditor && responsiveWidth)
        {
            layoutElement.enabled = Mathf.Max(h1Text.preferredWidth, contentText.preferredWidth) >= layoutElement.preferredWidth;
        }

        // Follow cursor
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width - 0.25f;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}

/* ------------------------------ Tooltip Types ----------------------------- */
public enum TooltipType {NPCGoal, NPCEvent, NPCStrategy, Trait, InOrderCombo, AtLeastCombo};