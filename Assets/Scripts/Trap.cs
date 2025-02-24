using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý scene

public class Trap : MonoBehaviour
{
    public GameObject gameOverUI; // Giao diện thua cuộc

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Kiểm tra xem có phải là người chơi không
        {
            // Dừng trò chơi
            Time.timeScale = 0;

            // Hiển thị giao diện thua cuộc
            gameOverUI.SetActive(true);
        }
    }
}
