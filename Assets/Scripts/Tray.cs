using UnityEngine;

public class Tray : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là người chơi không
        if (other.CompareTag("Player"))
        {
            // Gán người chơi là con của khay để di chuyển theo khay
            other.transform.SetParent(this.transform);
            Debug.Log("Player đã va chạm với khay: YES"); // Ghi log thông báo va chạm
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra xem đối tượng rời khỏi có phải là người chơi không
        if (other.CompareTag("Player"))
        {
            // Gỡ người chơi ra khỏi khay
            other.transform.SetParent(null);
            Debug.Log("Player đã rời khỏi khay: YES"); // Ghi log thông báo rời khỏi
        }
    }
}
