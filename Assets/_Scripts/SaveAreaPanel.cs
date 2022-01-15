using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAreaPanel : MonoBehaviour
{
    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 safe_area_min = Screen.safeArea.position;
        Vector2 safe_area_max = safe_area_min + Screen.safeArea.size;

        safe_area_min.x /= Screen.width;
        safe_area_min.y /= Screen.height;
        safe_area_max.x /= Screen.width;
        safe_area_max.y /= Screen.height;
        
        rect.anchorMin = safe_area_min;
        rect.anchorMax = safe_area_max;
    }
}
