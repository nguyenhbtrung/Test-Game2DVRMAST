using System.Collections.Generic;  // Sử dụng List
using UnityEngine;

public class CameraCheckpointController : MonoBehaviour
{
    // Danh sách các checkpoint (vật thể để lấy Collider)
    public List<GameObject> checkpoints = new List<GameObject>();

    // Danh sách các vị trí camera tương ứng
    public List<Transform> cameraPositions = new List<Transform>();

    // Biến để đặt camera cần di chuyển
    public Camera cameraToMove;

    // Vị trí ban đầu của camera
    public Transform initialPosition;

    // Tốc độ di chuyển của camera
    public float moveSpeed = 2f;

    // Biến để theo dõi camera đang di chuyển hay không
    private bool isMoving = false;

    // Biến để lưu vị trí đích của camera
    private Transform targetPosition;

    void Start()
    {
        // Kiểm tra nếu camera đã được gán vị trí ban đầu
        if (initialPosition != null && cameraToMove != null)
        {
            cameraToMove.transform.position = initialPosition.position;
            cameraToMove.transform.rotation = initialPosition.rotation;
        }
    }

    void Update()
    {
        // Nếu camera đang di chuyển, thực hiện di chuyển
        if (isMoving && targetPosition != null)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        // Di chuyển camera tới vị trí đích với tốc độ moveSpeed
        cameraToMove.transform.position = Vector3.Lerp(cameraToMove.transform.position, targetPosition.position, Time.deltaTime * moveSpeed);
        cameraToMove.transform.rotation = Quaternion.Lerp(cameraToMove.transform.rotation, targetPosition.rotation, Time.deltaTime * moveSpeed);

        // Kiểm tra nếu camera đã tới gần vị trí mục tiêu
        if (Vector3.Distance(cameraToMove.transform.position, targetPosition.position) < 0.1f)
        {
            isMoving = false;
            cameraToMove.transform.position = targetPosition.position; // Đảm bảo camera ở đúng vị trí mục tiêu
        }
    }

    // Hàm xử lý khi va chạm với checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sử dụng tham số collision thay vì other
        int checkpointIndex = checkpoints.IndexOf(collision.gameObject);

        if (checkpointIndex != -1 && checkpointIndex < cameraPositions.Count)
        {
            Debug.Log("Checkpoint detected: " + checkpointIndex);
            targetPosition = cameraPositions[checkpointIndex];
            isMoving = true;
        }
        else
        {
            Debug.Log("Checkpoint not found or no camera position.");
        }
    }

}
