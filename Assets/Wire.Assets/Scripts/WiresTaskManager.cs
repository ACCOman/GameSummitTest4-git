using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WiresTaskManager : MonoBehaviour
{
    public WirePlug[] leftPlugs;
    public WirePlug[] rightSockets;
    public GameObject endMessage;
    public AudioSource audioSource;
    public AudioClip connectClip;
    public AudioClip selectClip;
    public AudioClip errorClip;
    public AudioClip successClip;

    [Header("Wire Drawing")]
    public GameObject wirePrefab; 

    private WirePlug activePlug;
    private GameObject activeWire;
    private Dictionary<WirePlug, GameObject> connections = new Dictionary<WirePlug, GameObject>();

    private Color[] colors = { Color.red, Color.blue, Color.yellow, new Color(1, 0, 1) }; // Red, Blue, Yellow, Pink

    void Start()
    {
        InitializeTask();
    }

    public void InitializeTask()
    {
        if (endMessage != null) endMessage.SetActive(false);
        
        foreach (var conn in connections.Values) 
        {
            if (conn != null) Destroy(conn);
        }
        connections.Clear();

        List<Color> leftColors = new List<Color>(colors);
        List<Color> rightColors = new List<Color>(colors);

        // Simple shuffle
        for (int i = 0; i < leftColors.Count; i++)
        {
            int rnd = Random.Range(i, leftColors.Count);
            Color temp = leftColors[i];
            leftColors[i] = leftColors[rnd];
            leftColors[rnd] = temp;
        }

        for (int i = 0; i < rightColors.Count; i++)
        {
            int rnd = Random.Range(i, rightColors.Count);
            Color temp = rightColors[i];
            rightColors[i] = rightColors[rnd];
            rightColors[rnd] = temp;
        }

        for (int i = 0; i < leftPlugs.Length; i++)
        {
            if (i < leftColors.Count)
            {
                leftPlugs[i].gameObject.SetActive(true);
                leftPlugs[i].SetColor(leftColors[i]);
                leftPlugs[i].isConnected = false;
                leftPlugs[i].connectedPlug = null;
            }
            else
            {
                leftPlugs[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < rightSockets.Length; i++)
        {
            if (i < rightColors.Count)
            {
                rightSockets[i].gameObject.SetActive(true);
                rightSockets[i].SetColor(rightColors[i]);
                rightSockets[i].isConnected = false;
                rightSockets[i].connectedPlug = null;
            }
            else
            {
                rightSockets[i].gameObject.SetActive(false);
            }
        }
    }

    public void StartWireDrag(WirePlug plug)
    {
        activePlug = plug;
        activeWire = Instantiate(wirePrefab, transform);
        activeWire.transform.SetSiblingIndex(endMessage.transform.GetSiblingIndex()); // Behind message
        
        if (audioSource && selectClip) audioSource.PlayOneShot(selectClip);

        UpdateWire(activePlug.transform.position, activePlug.transform.position);
    }

    public void UpdateWireDrag(Vector2 screenPos)
    {
        if (activeWire == null) return;
        UpdateWire(activePlug.transform.position, screenPos);
    }

    public void EndWireDrag(Vector2 screenPos)
    {
        if (activeWire == null) return;

        WirePlug targetSocket = null;
        foreach (var socket in rightSockets)
        {
            if (socket.gameObject.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(socket.GetComponent<RectTransform>(), screenPos))
            {
                targetSocket = socket;
                break;
            }
        }

        if (targetSocket != null && !targetSocket.isConnected && targetSocket.plugColor == activePlug.plugColor)
        {
            activePlug.isConnected = true;
            activePlug.connectedPlug = targetSocket;
            targetSocket.isConnected = true;
            targetSocket.connectedPlug = activePlug;
            connections.Add(activePlug, activeWire);
            UpdateWire(activePlug.transform.position, targetSocket.transform.position);
            
            if (audioSource && connectClip) audioSource.PlayOneShot(connectClip);

            CheckCompletion();
        }
        else
        {
            if (audioSource && errorClip) audioSource.PlayOneShot(errorClip);
            Destroy(activeWire);
        }

        activePlug = null;
        activeWire = null;
    }

    private void UpdateWire(Vector3 worldStart, Vector3 worldEnd)
    {
        RectTransform rt = activeWire.GetComponent<RectTransform>();
        
        // Convert world positions to local positions relative to the TaskRoot
        RectTransform taskRootRT = GetComponent<RectTransform>();
        Vector2 localStart, localEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(taskRootRT, worldStart, null, out localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(taskRootRT, worldEnd, null, out localEnd);

        Vector2 dir = localEnd - localStart;
        rt.localPosition = localStart + dir * 0.5f;
        rt.sizeDelta = new Vector2(dir.magnitude, 20f); 
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);
        
        activeWire.GetComponent<Image>().color = activePlug.plugColor;
    }

    private void CheckCompletion()
    {
        if (leftPlugs.Where(p => p.gameObject.activeSelf).All(p => p.isConnected))
        {
            if (audioSource && successClip) audioSource.PlayOneShot(successClip);
            if (endMessage != null) endMessage.SetActive(true);
        }
    }
}