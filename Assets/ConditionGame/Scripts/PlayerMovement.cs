using DialogueEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private MovementState _movementState;
    [SerializeField] private LayerMask _layerMask;

    private Vector3 _lastInteractDirection;
    private bool _isSpeaking = false;

    private void Start()
    {
        ConversationManager.OnConversationEnded += HandleEndDialog;
    }

    private void HandleEndDialog()
    {
        _isSpeaking = false;
    }

    private void Update()
    {
        if (_isSpeaking) return;
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, inputVector.y, 0f);

        HandleMovement(moveDirection);
        HandleInteractions(moveDirection);
    }

    private void OnDestroy()
    {
        ConversationManager.OnConversationEnded -= HandleEndDialog;
    }

    private void HandleInteractions(Vector2 moveDirection)
    {
        if (_isSpeaking) return;
        if (moveDirection != Vector2.zero)
            _lastInteractDirection = moveDirection;

        float interactDistance = 0.5f;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _lastInteractDirection, interactDistance, _layerMask);

            if (hit.collider != null && hit.collider.TryGetComponent(out NPC npc))
            {
                npc.Speak();
                _isSpeaking = true;
            }
        }
    }


    private void HandleMovement(Vector3 moveDirection)
    {
        float moveDistance = _moveSpeed * Time.deltaTime;
        bool canMove = CanMove(moveDirection, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = CanMove(moveDirectionX, moveDistance);

            if (canMove) moveDirection = moveDirectionX;
            else
            {
                Vector3 moveDirectionY = new Vector3(0, moveDirection.y, 0).normalized;
                canMove = CanMove(moveDirectionY, moveDistance);

                if (canMove) moveDirection = moveDirectionY;
            }
        }

        if (canMove) transform.position += moveDirection * moveDistance;

        _movementState.IsMove = moveDirection != Vector3.zero;
        if (_movementState.IsMove)
            _movementState.Direction = DirectionUtils.Get4DirectionFromVector(moveDirection);
    }

    private bool CanMove(Vector2 moveDirection, float moveDistance)
    {
        float playerRadius = 0.4f;
        Vector2 playerPosition = new(transform.position.x, transform.position.y - 0.6f);

        RaycastHit2D hit = Physics2D.CircleCast(playerPosition, playerRadius, moveDirection, moveDistance, _layerMask);
        return hit.collider == null;
    }
}