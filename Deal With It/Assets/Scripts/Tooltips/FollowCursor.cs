using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float pivotXOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Follow cursor
        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width - pivotXOffset;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
