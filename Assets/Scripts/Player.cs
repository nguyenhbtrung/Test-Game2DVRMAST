using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Để quản lý scene
using UnityEngine.UI; // Thư viện để sử dụng UI

public class Player : MonoBehaviour
{
    public int lives = 5; // Số mạng ban đầu
    private Vector3 lastCheckpointPosition; // Vị trí checkpoint gần nhất
    public GameObject gameOverUI; // Tham chiếu tới UI "Game Over"
    public SpriteRenderer playerSprite; // Tham chiếu tới SpriteRenderer của nhân vật
    public Color damagedColor = Color.red; // Màu khi nhân vật va chạm với bẫy
    private Color originalColor; // Màu gốc của nhân vật
    public float flashDuration = 0.1f; // Thời gian nhấp nháy
    public float pauseDuration = 0.2f; // Thời gian tạm dừng trước khi quay lại checkpoint

    public GameObject lifeImagePrefab; // Prefab hình ảnh mạng
    public Transform lifeContainer; // Vị trí để chứa hình ảnh mạng
    private List<Image> lifeImages = new List<Image>(); // Danh sách hình ảnh mạng

    private bool canMove = true; // Biến kiểm soát trạng thái di chuyển
    private bool hasDiedThisHit = false; // Biến kiểm soát việc mất mạng trong lần va chạm này

    // Khởi động game với vị trí checkpoint mặc định (vị trí ban đầu)
    void Start()
    {
        lastCheckpointPosition = transform.position; // Lưu vị trí ban đầu
        gameOverUI.SetActive(false); // Ẩn UI "Game Over" khi game bắt đầu

        // Lưu màu gốc
        originalColor = playerSprite.color;

        // Tạo và thêm hình ảnh mạng vào UI
        for (int i = 0; i < lives; i++)
        {
            Image lifeImage = Instantiate(lifeImagePrefab, lifeContainer).GetComponent<Image>();
            lifeImages.Add(lifeImage);
        }

        UpdateLifeUI(); // Cập nhật UI hiển thị số mạng
    }

    // Hàm này được gọi mỗi khi nhân vật va chạm với một bẫy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap")) // Kiểm tra va chạm với bẫy (Trap)
        {
            if (!hasDiedThisHit) // Kiểm tra xem nhân vật đã mất mạng trong lần va chạm này chưa
            {
                hasDiedThisHit = true; // Đánh dấu rằng nhân vật đã chết trong lần va chạm này
                StartCoroutine(LoseLifeWithEffects()); // Gọi hàm mất mạng với hiệu ứng
                playerSprite.color = damagedColor; // Thay đổi màu khi va chạm
            }
        }

        if (collision.gameObject.CompareTag("Checkpoint")) // Kiểm tra va chạm với checkpoint
        {
            lastCheckpointPosition = collision.transform.position; // Lưu lại vị trí checkpoint
        }
    }

    // Coroutine để giảm số mạng, hiệu ứng nhấp nháy, tạm dừng và quay lại checkpoint
    IEnumerator LoseLifeWithEffects()
    {
        lives--; // Giảm 1 mạng
        Debug.Log("Lives left: " + lives);

        UpdateLifeUI(); // Cập nhật UI hiển thị mạng

        canMove = false;
        yield return new WaitForSeconds(0.1f);
        if (lives > 0)
        {
            // Hiệu ứng nhấp nháy màu trắng
            StartCoroutine(FlashWhite());

            // Tạm dừng trò chơi trong 0.5 giây
            yield return new WaitForSeconds(pauseDuration);

            // Quay lại vị trí checkpoint
            transform.position = lastCheckpointPosition;
            canMove = false;
            yield return new WaitForSeconds(0.5f); // Chờ 1 giây
            canMove = true;
            // Khôi phục lại màu gốc
            playerSprite.color = originalColor;
        }
        else
        {
            // Khi hết mạng, hiển thị UI "Game Over"
            Debug.Log("Game Over!");
            ShowGameOverUI();
        }

        hasDiedThisHit = false; // Đặt lại biến để có thể chết trong lần va chạm tiếp theo
    }

    // Cập nhật UI hiển thị số mạng còn lại
    void UpdateLifeUI()
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < lives)
            {
                lifeImages[i].gameObject.SetActive(true); // Hiển thị hình ảnh mạng
            }
            else
            {
                lifeImages[i].gameObject.SetActive(false); // Ẩn hình ảnh mạng
            }
        }
    }

    // Coroutine để nhấp nháy màu trắng
    IEnumerator FlashWhite()
    {
        for (int i = 0; i < 5; i++) // Lặp lại 5 lần
        {
            playerSprite.color = Color.white; // Đổi màu nhân vật thành trắng
            yield return new WaitForSeconds(flashDuration); // Đợi trong thời gian nhấp nháy
            playerSprite.color = damagedColor; // Đổi màu nhân vật trở lại màu khi va chạm
            yield return new WaitForSeconds(flashDuration);
        }
    }

    // Hàm để hiển thị UI "Game Over"
    void ShowGameOverUI()
    {
        Time.timeScale = 0; // Dừng thời gian trong game
        gameOverUI.SetActive(true); // Hiển thị UI "Game Over"
    }

    // Phương thức để khởi động lại trò chơi
    public void RestartGame()
    {
        Time.timeScale = 1; // Đặt lại thời gian về tốc độ bình thường
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Phương thức để quay về menu chính
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Đặt lại thời gian về tốc độ bình thường
        SceneManager.LoadScene("Menu"); // Đảm bảo "MainMenu" là tên chính xác của scene menu
    }

    // Phương thức để thoát trò chơi
    public void QuitGame()
    {
        Application.Quit(); // Thoát game khi chạy bản build
        Debug.Log("Game is exiting"); // Chỉ hoạt động trong Unity Editor
    }

    public bool CanMove() // Phương thức để kiểm tra trạng thái di chuyển
    {
        return canMove;
    }
}
