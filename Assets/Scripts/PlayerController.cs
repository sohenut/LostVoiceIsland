using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("�̵�/���� ����")]
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
        // �¿� �̵� (�׻�)
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // ���� �Է� ó�� (���� ��������)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Ground �±��� �ݶ��̴��� ó�� ����� ��, �Ʒ����� ������� ���� ����
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
        // Ground �±��� �ݶ��̴��� ��� ��� �ִ� ����
        if (!col.collider.CompareTag("Ground"))
            return;

        // �� ������ ���� ������ ��� ����(normal)�� �˻��ؼ�
        // y �� ������ ����� ū(���� �����) ������ ������ ������ ���� ����
        foreach (var contact in col.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }

        // �� ������ �ϳ��� �������� ������, ������ ������ ������ ����
        isGrounded = false;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // Ground �±��� �ݶ��̴����� ������ �������� ���� ����
        if (col.collider.CompareTag("Ground"))
            isGrounded = false;
    }
}
