using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private float _vInput;
    private float _hInput;
    private bool _isJumping;
    private Rigidbody _rb;

    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float JumpVelocity = 5f;

    public bool IsOnGround = true;
    public float GroundCheckRadius = 1.05f;
    public LayerMask GroundLayer;
    public Transform GroundCheck;

    public GameObject Bullet;
    public float BulletSpeed = 100f;
    private bool _isShooting;

    public GameBehavior GameManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * moveSpeed;
        _hInput = Input.GetAxis("Horizontal") * rotateSpeed;
        IsOnGround = Physics.CheckSphere(transform.position, GroundCheckRadius, GroundLayer);

        IsOnGround = Physics.CheckSphere(
            GroundCheck.position,
            GroundCheckRadius,
            GroundLayer
        );

        Debug.Log("Grounded: " + IsOnGround);
        Debug.Log("Ground Layer Mask: " + GroundLayer.value);


        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround)
        {
            _isJumping = true;
        }

        _isShooting |= Input.GetKeyDown(KeyCode.Return);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * _vInput * Time.fixedDeltaTime);
        Quaternion angleRot = Quaternion.Euler(Vector3.up * _hInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
        
        if (_isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
            _isJumping = false;
        }

        if (_isShooting)
        {
            Vector3 spawnPos = transform.position + transform.forward * 1f;
            GameObject newBullet = Instantiate(Bullet, spawnPos, this.transform.rotation);
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.linearVelocity = this.transform.forward * BulletSpeed;
            _isShooting = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
    }
}