using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dichuyen2 : MonoBehaviour
{
    public float runSpeed = 5f;  // Tốc độ chạy
    public float jumpForce = 350f;  // Lực nhảy
    private bool isGrounded = true;  // Kiểm tra xem nhân vật có đang chạm đất không

    private bool moveLeft = false;  // Cờ để kiểm tra di chuyển trái
    private bool moveRight = false; // Cờ để kiểm tra di chuyển phải
    private bool isJumping = false; // Cờ để kiểm tra nhảy
    private bool uiControlActive = false; // Cờ kiểm tra xem UI có đang điều khiển hay không

    private SpriteRenderer spriteRenderer; // Tham chiếu tới SpriteRenderer của nhân vật
    private Rigidbody2D rb2D;
    private Animator ani;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        // Chỉ xử lý điều khiển từ bàn phím nếu không có điều khiển từ UI
        if (!uiControlActive)
        {
            HandleKeyboardInput();
        }

        // Xử lý di chuyển
        HandleMovement();
        // Tính toán giá trị di chuyển và cập nhật animator
        float moveValue = 0;
        if (moveLeft)
        {
            moveValue = -1; // Di chuyển sang trái
        }
        else if (moveRight)
        {
            moveValue = 1; // Di chuyển sang phải
        }

        ani.SetFloat("move", Mathf.Abs(moveValue));
        // Xử lý nhảy
        if (isJumping && isGrounded)
        {
            Jump();
        }
    }

    // Hàm xử lý di chuyển từ bàn phím
    void HandleKeyboardInput()
    {
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft = true;
            moveRight = false;
        }
        else if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            moveRight = true;
            moveLeft = false;
        }
        else
        {
            moveLeft = false;
            moveRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }
    }

    // Hàm xử lý di chuyển từ UI và bàn phím
    void HandleMovement()
    {
        // Nếu không thể di chuyển, trở về ngay
        if (!GetComponent<Player>().CanMove())
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Dừng lại
            return; // Thoát hàm
        }

        // Nếu di chuyển trái/phải thì mới thay đổi velocity
        if (moveLeft)
        {
            rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1); // Lật nhân vật 180 độ
        }
        else if (moveRight)
        {
            rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
            transform.localScale = new Vector3(1, 1, 1); // Khôi phục hướng nhân vật
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Dừng lại
        }
    }

    void Jump()
    {
        rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
        isJumping = false;  // Reset cờ nhảy sau khi thực hiện nhảy
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi chạm đất, cho phép nhảy tiếp
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Các hàm điều khiển di chuyển và nhảy từ UI
    public void MoveLeft()
    {
        moveLeft = true;
        moveRight = false;
        uiControlActive = true; // UI đang điều khiển
    }

    public void StopMoveLeft()
    {
        moveLeft = false;
        uiControlActive = false; // Không còn điều khiển từ UI
    }

    public void MoveRight()
    {
        moveRight = true;
        moveLeft = false;
        uiControlActive = true; // UI đang điều khiển
    }

    public void StopMoveRight()
    {
        moveRight = false;
        uiControlActive = false; // Không còn điều khiển từ UI
    }

    public void JumpButton()
    {
        if (isGrounded)
        {
            isJumping = true;
        }
    }
}
