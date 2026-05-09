using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WirePlug : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isSocket;
    public Color plugColor;
    public bool isConnected;
    public WirePlug connectedPlug;

    private WiresTaskManager manager;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        manager = GetComponentInParent<WiresTaskManager>();
    }

    public void SetColor(Color color)
    {
        plugColor = color;
        if (image != null) image.color = color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSocket || isConnected) return;
        manager.StartWireDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSocket || isConnected) return;
        manager.UpdateWireDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSocket || isConnected) return;
        manager.EndWireDrag(eventData.position);
    }
}