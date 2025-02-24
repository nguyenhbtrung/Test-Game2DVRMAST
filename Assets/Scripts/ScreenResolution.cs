using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    void Start()
    {
        // Đặt tỷ lệ cố định cho màn hình (ví dụ: 16:9)
        int targetWidth = 1920;
        int targetHeight = 1080;
        int currentWidth = Screen.width;
        int currentHeight = Screen.height;

        // Tính toán tỷ lệ của màn hình hiện tại
        float targetAspect = (float)targetWidth / targetHeight;
        float currentAspect = (float)currentWidth / currentHeight;

        // Nếu tỷ lệ hiện tại không khớp với tỷ lệ mục tiêu, thay đổi kích thước
        if (currentAspect > targetAspect)
        {
            // Nếu tỷ lệ hiện tại quá rộng, điều chỉnh chiều rộng
            int width = Mathf.RoundToInt(targetHeight * currentAspect);
            Screen.SetResolution(width, targetHeight, true);
        }
        else
        {
            // Nếu tỷ lệ hiện tại quá hẹp, điều chỉnh chiều cao
            int height = Mathf.RoundToInt(targetWidth / currentAspect);
            Screen.SetResolution(targetWidth, height, true);
        }
    }
}
