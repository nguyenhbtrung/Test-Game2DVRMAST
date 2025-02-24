using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý scene

public class Win : MonoBehaviour
{
    public GameObject gameWinIU; // Giao diện thắng

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Dừng trò chơi
            Time.timeScale = 0;
            gameWinIU.SetActive(true); // Hiển thị giao diện thắng
        }
    }

    // Phương thức để khởi động lại trò chơi
    public void RestartGame()
    {
        Time.timeScale = 1; // Đặt lại thời gian
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại scene hiện tại
    }

    // Phương thức để quay về menu chính
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Đặt lại thời gian
        SceneManager.LoadScene("Menu"); // Thay "Menu" bằng tên của scene menu chính của bạn
    }

    // Phương thức để chuyển tới màn chơi mới
    public void GoToNextLevel()
    {
        Time.timeScale = 1; // Đặt lại thời gian
        // Giả sử màn chơi tiếp theo có index là hiện tại + 1, điều chỉnh nếu cần
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
