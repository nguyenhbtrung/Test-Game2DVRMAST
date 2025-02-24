using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public Transform pointA; // Tham chiếu đến Transform của điểm A
    public Transform pointB; // Tham chiếu đến Transform của điểm B
    public float speed = 2.0f; // Tốc độ di chuyển

    private bool movingToB = true; // Biến để xác định hướng di chuyển

    void Update()
    {
        // Kiểm tra nếu pointA hoặc pointB chưa được gán
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("Vui lòng gán pointA và pointB trong Inspector.");
            return; // Không thực hiện di chuyển nếu chưa gán
        }

        // Di chuyển đối tượng
        if (movingToB)
        {
            // Di chuyển về điểm B
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            // Kiểm tra nếu đã đến điểm B
            if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToB = false; // Đổi hướng di chuyển
            }
        }
        else
        {
            // Di chuyển về điểm A
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
            // Kiểm tra nếu đã đến điểm A
            if (Vector3.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToB = true; // Đổi hướng di chuyển
            }
        }
    }
}
