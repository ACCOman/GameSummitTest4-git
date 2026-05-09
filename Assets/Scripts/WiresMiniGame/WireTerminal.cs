using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WireTerminal : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public Color terminalColor;
    public bool isLeft;
    public bool isConnected;

    [Header("UI Components")]
    public Image colorIndicator;
    public RectTransform wireHandle; // Point where the wire starts/ends

    private WiresMiniGameController controller;
    private GameObject currentWire;
    private RectTransform wireRT;
    private Canvas canvas;

    public void Initialize(Color color, bool left, WiresMiniGameController ctrl)
    {
        terminalColor = color;
        isLeft = left;
        controller = ctrl;
        
        if (colorIndicator) colorIndicator.color = color;
        isConnected = false;
        
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isLeft || isConnected) return;

        // Start creating a wire
        currentWire = Instantiate(controller.wirePrefab, controller.wireContainer); 
        wireRT = currentWire.GetComponent<RectTransform>();
        currentWire.GetComponent<Image>().color = terminalColor;
        
        controller.OnWireGrab();
        UpdateWire(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isLeft || isConnected || currentWire == null) return;
        UpdateWire(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isLeft || isConnected || currentWire == null) return;

        // Check if dropped over a valid right terminal
        WireTerminal target = null;
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        foreach (var result in results)
        {
            var t = result.gameObject.GetComponent<WireTerminal>();
            if (t != null && !t.isLeft && !t.isConnected && t.terminalColor == this.terminalColor)
            {
                target = t;
                break;
            }
        }

        if (target != null)
        {
            // Connect!
            isConnected = true;
            target.isConnected = true;
            
            // Snap wire to target's handle
            UpdateWire(target.wireHandle.position, true);
            controller.OnWireConnected();
        }
        else
        {
            controller.OnWireError();
            Destroy(currentWire);
        }

        currentWire = null;
    }

    private void UpdateWire(Vector2 targetPos, bool isWorldPos = false)
    {
        if (currentWire == null) return;

        Vector2 startPos = wireHandle.position;
        Vector2 endPos = targetPos;

        Camera cam = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera;

        Vector2 localStart, localEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(wireRT.parent as RectTransform, RectTransformUtility.WorldToScreenPoint(cam, startPos), cam, out localStart);
        
        if (isWorldPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(wireRT.parent as RectTransform, RectTransformUtility.WorldToScreenPoint(cam, endPos), cam, out localEnd);
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(wireRT.parent as RectTransform, endPos, cam, out localEnd);
        }

        Vector2 direction = localEnd - localStart;
        wireRT.localPosition = localStart + direction * 0.5f;
        wireRT.sizeDelta = new Vector2(direction.magnitude, 20f); // 20f thickness
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        wireRT.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
