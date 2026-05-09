using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    // Bu metod Play düyməsinə bağlanacaq
    public void PlayGame()
    {
        // Burada "GameScene" əvəzinə öz səhnə adını yaz
        SceneManager.LoadScene("GameScene");
    }

    // İstəsən çıxış düyməsi də əlavə edə bilərsən
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun bağlandı!");
    }
}
