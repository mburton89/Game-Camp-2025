using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Double Jump Settings")]
    private bool _canDoubleJump;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private bool _isDashing;
    private float _dashTimeLeft;
    private bool _canDash;

    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // State
    private float _horizontalInput;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // 1) Ground check (so we can reset jumps/dash immediately)
        _isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (_isGrounded)
        {
            // Reset double-jump and dash when touching ground
            _canDoubleJump = true;
            _canDash = true;
        }

        // 2) Read horizontal input (−1, 0, +1)
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        // 3) Flip sprite based on moving direction
        if (_horizontalInput > 0.01f)
            _spriteRenderer.flipX = false;
        else if (_horizontalInput < -0.01f)
            _spriteRenderer.flipX = true;

        // 4) Jump logic:
        //    - If grounded → normal jump.
        //    - Else if in-air and double-jump still available → do a second jump.
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
            else if (_canDoubleJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _canDoubleJump = false;
            }
        }

        // 5) Dash logic:
        //    Can dash once (ground or air), then reset when grounded again.
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing && _canDash)
        {
            _isDashing = true;
            _dashTimeLeft = dashDuration;
            _canDash = false;

            // Determine dash direction: 
            // • if player is holding left/right, use that 
            // • otherwise dash in facing direction
            float dir = 0f;
            if (Mathf.Abs(_horizontalInput) > 0.01f)
                dir = Mathf.Sign(_horizontalInput);
            else
                dir = (_spriteRenderer.flipX) ? -1f : +1f;

            // Preserve current vertical velocity; override horizontal
            _rb.velocity = new Vector2(dir * dashSpeed, _rb.velocity.y);
        }

        // 6) Animator updates (if you have an Animator)
        if (_animator)
        {
            _animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("IsDashing", _isDashing);
        }
    }

    private void FixedUpdate()
    {
        // 1) If currently dashing, count down dash timer—
        //    while dashing, we prevent normal horizontal movement.
        if (_isDashing)
        {
            _dashTimeLeft -= Time.fixedDeltaTime;
            if (_dashTimeLeft <= 0f)
            {
                _isDashing = false;
                // Once dash ends, drop back into normal movement. 
                // (We keep whatever vertical velocity the Rigidbody has.)
            }
        }
        else
        {
            // 2) Normal horizontal movement (only when not dashing)
            Vector2 newVel = new Vector2(_horizontalInput * moveSpeed, _rb.velocity.y);
            _rb.velocity = newVel;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground-check circle in the editor
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
