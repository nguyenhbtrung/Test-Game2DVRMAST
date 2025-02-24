using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện để quản lý scene

public class You : MonoBehaviour
{
    // Phương thức để khởi động lại trò chơi
    public void RestartGame()
    {
        Time.timeScale = 1; // Đặt lại thời gian về tốc độ bình thường
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại scene hiện tại
    }

    // Phương thức để quay về menu chính
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Đặt lại thời gian về tốc độ bình thường
        SceneManager.LoadScene("MainMenu"); // Đảm bảo "MainMenu" là tên chính xác của scene menu
    }

    // Phương thức để thoát trò chơi
    public void QuitGame()
    {
        Application.Quit(); // Thoát game khi chạy bản build
        Debug.Log("Game is exiting"); // Chỉ hoạt động trong Unity Editor
    }
}
