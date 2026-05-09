using UnityEngine;

public class WireTaskTrigger : MonoBehaviour
{
    public WiresMiniGameController miniGame;

    void OnMouseDown()
    {
        if (miniGame != null)
        {
            miniGame.OpenGame();
        }
    }

    // Optional: for UI buttons
    public void OpenTask()
    {
        if (miniGame != null)
        {
            miniGame.OpenGame();
        }
    }
}
