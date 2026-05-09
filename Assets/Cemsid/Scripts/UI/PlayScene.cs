using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
        // Burada "GameScene" səhnənin adı olmalıdır
    }

    public void QuitGame()
    {
        Application.Quit();
        // Editor-da işləməyəcək, amma build-də oyun bağlanacaq
    }
}
