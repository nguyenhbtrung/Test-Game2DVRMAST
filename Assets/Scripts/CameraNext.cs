using UnityEngine;

public class CameraNext : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = false;

    // Tham chiếu đến camera
    public Camera cameraToMove; // Thêm biến này

    public GameObject fistPosition;
    public GameObject secondPosition;
    private bool movingToSecond = true; // Track direction of movement

    void Start()
    {
        // Đặt camera ở vị trí fistPosition
        if (cameraToMove != null)
        {
            cameraToMove.transform.position = fistPosition.transform.position;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        if (cameraToMove != null) // Kiểm tra xem camera đã được gán chưa
        {
            Vector3 targetPosition = movingToSecond ? secondPosition.transform.position : fistPosition.transform.position;
            cameraToMove.transform.position = Vector3.Lerp(cameraToMove.transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(cameraToMove.transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                cameraToMove.transform.position = targetPosition; // Đảm bảo camera ở vị trí mục tiêu
                movingToSecond = !movingToSecond; // Chuyển hướng
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Next"))
        {
            isMoving = true; // Bắt đầu di chuyển camera
        }
    }
}
