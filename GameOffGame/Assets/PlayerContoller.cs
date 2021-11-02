using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    
    private Rigidbody2D rb2D;

    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject pfVile;
    [SerializeField] private GameObject pfBlob;
    Camera mainCam;

    private float moveSpeed;
    private float jumpForce;
    private bool isJumping;
    private float moveHorizontal;
    private float moveVertical;

    Vector3 handPos;
    private float throwForce;

    public List<GameObject> blobs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        moveSpeed = 2.5f;
        jumpForce = 25f;
        isJumping = false;

        throwForce = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        handPos = firepoint.position;
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0))
        {
            ThrowVile((mainCam.ScreenToWorldPoint(Input.mousePosition) - handPos).normalized);
        }
    }

    private void FixedUpdate()
    {
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            if (!isJumping)
            {
                rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, 0), ForceMode2D.Impulse);
            }
            else if (isJumping)
            {
                rb2D.AddForce(new Vector2(moveHorizontal * (moveSpeed /2), 0), ForceMode2D.Impulse);
            }
        }
        
        if (moveVertical > 0.1f && !isJumping)
        {
            rb2D.AddForce(new Vector2(0, moveVertical * jumpForce), ForceMode2D.Impulse);
        }
    }

    void ThrowVile(Vector3 throwDir)
    {
        GameObject vile = Instantiate(pfVile, firepoint.position, Quaternion.identity);
        vile.GetComponent<Rigidbody2D>().AddForce(throwDir * throwForce, ForceMode2D.Impulse);
        vile.GetComponent<Bullet>().playerContoller = this;
    }

    public void AddBlobToList(Vector3 positionToSpawn)
    {
        GameObject blob = (GameObject)Instantiate(pfBlob, positionToSpawn + new Vector3(0, 0, 1), Quaternion.identity);
        blobs.Add(blob);
        if (blobs.Count >= 3)
        {
            Destroy(blobs[0]);
            blobs.RemoveAt(0);
        }
        Debug.Log(blobs);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }
}
