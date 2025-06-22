using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("이동/점프 세팅")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 좌우 이동 (항상)
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // 점프 입력 처리 (지면 위에서만)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Ground 태그의 콜라이더와 처음 닿았을 때, 아래에서 닿았으면 착지 판정
        if (!col.collider.CompareTag("Ground"))
            return;

        foreach (var contact in col.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Ground 태그의 콜라이더와 계속 닿아 있는 동안
        if (!col.collider.CompareTag("Ground"))
            return;

        // 한 프레임 내에 가능한 모든 접점(normal)을 검사해서
        // y 축 성분이 충분히 큰(거의 수평면) 접점이 있으면 여전히 착지 상태
        foreach (var contact in col.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }

        // 위 조건을 하나도 만족하지 않으면, 땅에서 떨어진 것으로 간주
        isGrounded = false;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // Ground 태그의 콜라이더에서 완전히 떨어지면 착지 해제
        if (col.collider.CompareTag("Ground"))
            isGrounded = false;
    }
}
