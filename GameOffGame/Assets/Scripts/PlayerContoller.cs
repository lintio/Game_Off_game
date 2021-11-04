using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerContoller : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * Time.deltaTime * moveSpeed;
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.002)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
