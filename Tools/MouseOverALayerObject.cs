using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MouseOverALayerObject
{
    /// <summary>
    /// Returns true if the mouse is over an object with the layer putted as a parameter
    /// </summary>
    public static bool IsPointerOverLayerObject(string layerName) {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++) {
            if (results[ i ].gameObject.layer == LayerMask.NameToLayer(layerName))
                return true;
        }

        return false;
    }
}