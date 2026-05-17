using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;

    private Rigidbody2D _rb;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private float _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // WASD移動
        _moveAction = new InputAction(
            name: "Move",
            type: InputActionType.Value
        );

        _moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/d");

        // ジャンプ
        _jumpAction = new InputAction(
            name: "Jump",
            binding: "<Keyboard>/w"
        );
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
    }

    private void Start()
    {
        // 横入力監視
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                _moveInput = _moveAction.ReadValue<float>();
            })
            .AddTo(this);

        // 移動処理
        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                Vector2 velocity = new Vector2();
                velocity.x = _moveInput * moveSpeed;
                _rb.AddForce(velocity, ForceMode2D.Force);
            })
            .AddTo(this);

        // ジャンプ入力
        Observable.EveryUpdate()
            .Where(_ => _jumpAction.triggered)
            .Subscribe(_ =>
            {
                Jump();
            })
            .AddTo(this);
    }

    private void Jump()
    {
        Vector2 velocity = new Vector2();
        velocity.y = jumpForce;
        _rb.AddForce(velocity, ForceMode2D.Impulse);
    }
}