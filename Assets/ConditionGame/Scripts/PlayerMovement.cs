using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private MovementState _movementState;
    [SerializeField] private LayerMask _layerMask;

    private Vector3 _lastInteractDirection;

    private void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, inputVector.y, 0f);

        HandleMovement(moveDirection);
        HandleInteractions(moveDirection);
    }

    private void HandleInteractions(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero) _lastInteractDirection = moveDirection;

        float interactDistance = 1f;
        if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, interactDistance, _layerMask))
        {
            // if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            // {
            //     clearCounter.Interact();
            // }
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
        float playerRadius = 0.2f;
        Vector2 playerPosition = transform.position;

        RaycastHit2D hit = Physics2D.CircleCast(playerPosition, playerRadius, moveDirection, moveDistance, _layerMask);
        return hit.collider == null;
    }
}