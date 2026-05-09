using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WiresMiniGameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mainPanel;
    public Transform leftTerminalContainer;
    public Transform rightTerminalContainer;
    public RectTransform wireContainer;
    public GameObject winOverlay;
    public Button closeButton;

    [Header("Prefabs")]
    public GameObject terminalPrefab;
    public GameObject wirePrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip grabClip;
    public AudioClip connectClip;
    public AudioClip errorClip;
    public AudioClip winClip;

    [Header("Settings")]
    public Color[] wireColors = { Color.red, Color.blue, Color.yellow, new Color(0.8f, 0.2f, 1f) }; 

    private List<WireTerminal> leftTerminals = new List<WireTerminal>();
    private List<WireTerminal> rightTerminals = new List<WireTerminal>();
    private int completedWires = 0;

    void Start()
    {
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        if (closeButton != null) closeButton.onClick.AddListener(CloseGame);
        InitializeGame();
    }

    public void OpenGame()
    {
        if (mainPanel) mainPanel.SetActive(true);
        InitializeGame();
    }

    public void CloseGame()
    {
        if (mainPanel) mainPanel.SetActive(false);
    }

    public void InitializeGame()
    {
        completedWires = 0;
        if (winOverlay) 
        {
            winOverlay.SetActive(false);
        }

        // Clear existing wires
        foreach (Transform child in wireContainer) Destroy(child.gameObject);
        
        // Clear existing terminals
        foreach (Transform child in leftTerminalContainer) Destroy(child.gameObject);
        foreach (Transform child in rightTerminalContainer) Destroy(child.gameObject);

        leftTerminals.Clear();
        rightTerminals.Clear();

        // Create Left Terminals
        List<Color> leftColors = new List<Color>(wireColors);
        Shuffle(leftColors);

        for (int i = 0; i < leftColors.Count; i++)
        {
            GameObject go = Instantiate(terminalPrefab, leftTerminalContainer);
            WireTerminal terminal = go.GetComponent<WireTerminal>();
            terminal.Initialize(leftColors[i], true, this);
            leftTerminals.Add(terminal);
        }

        // Create Right Terminals
        List<Color> rightColors = new List<Color>(wireColors);
        Shuffle(rightColors);

        for (int i = 0; i < rightColors.Count; i++)
        {
            GameObject go = Instantiate(terminalPrefab, rightTerminalContainer);
            WireTerminal terminal = go.GetComponent<WireTerminal>();
            terminal.Initialize(rightColors[i], false, this);
            rightTerminals.Add(terminal);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    public void OnWireConnected()
    {
        completedWires++;
        PlaySound(connectClip);
        if (completedWires >= wireColors.Length)
        {
            Win();
        }
    }

    public void OnWireError()
    {
        PlaySound(errorClip);
    }

    public void OnWireGrab()
    {
        PlaySound(grabClip);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip) audioSource.PlayOneShot(clip);
    }

    private void Win()
    {
        if (winOverlay) winOverlay.SetActive(true);
        PlaySound(winClip);
        Debug.Log("Task completed");
    }
}
