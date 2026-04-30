using System.IO;
using Unity.Mathematics;
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

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * moveSpeed;
        _hInput = Input.GetAxis("Horizontal") * rotateSpeed;
        _isJumping = Input.GetKeyDown(KeyCode.Space);
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
    }
}