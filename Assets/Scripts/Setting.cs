using UnityEngine;
using UnityEngine.SceneManagement;  // Cần để quản lý Scene

public class Setting : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Biến để lưu UI Panel khi pause game
    private bool isPaused = false;  // Kiểm tra xem game có đang bị tạm dừng không

    // Hàm thoát game
    public void QuitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit();
    }

    // Hàm quay lại Menu (scene tên là "Menu")
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Hàm vào scene Game1 (scene tên là "Game1")
    public void PlayGame()
    {
        SceneManager.LoadScene("Game1");
        Time.timeScale = 1;
    }

    // Hàm chuyển tới level kế tiếp
    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Hàm restart lại level hiện tại
    public void RestartGame()
    {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Hàm tạm dừng và hiện UI Pause
    public void PauseGame()
    {
        if (isPaused)
        {
            ResumeGame();  // Nếu game đang bị tạm dừng thì tiếp tục
        }
        else
        {
            pauseMenuUI.SetActive(true);  // Hiển thị UI Pause
            Time.timeScale = 0f;          // Dừng thời gian trong game
            isPaused = true;              // Đặt cờ là game đang tạm dừng
        }
    }

    // Hàm tiếp tục game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Ẩn UI Pause
        Time.timeScale = 1f;          // Cho game tiếp tục chạy
        isPaused = false;             // Đặt cờ là game đang chạy
    }
    public void PlayGame1()
    {
        SceneManager.LoadScene("Game1");
        Time.timeScale = 1;
    }
    public void PlayGame2()
    {
        SceneManager.LoadScene("Game2");
        Time.timeScale = 1;
    }
    public void PlayGame3()
    {
        SceneManager.LoadScene("Game3");
        Time.timeScale = 1;
    }
    public void PlayGame4()
    {
        SceneManager.LoadScene("Game4");
        Time.timeScale = 1;
    }
    public void Manchoi()
    {
        SceneManager.LoadScene("Manchoi");
    }
}
