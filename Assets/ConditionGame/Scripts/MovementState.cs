using System;
using UnityEngine;

public class MovementState : MonoBehaviour
{
    public event Action OnChangeMoveState = null;
    public event Action OnChangeDirection = null;

    [field: SerializeField] public float MoveSpeed { get; set; }

    public bool IsStay => !IsMove;

    public Direction Direction
    {
        get => m_direction;
        set
        {
            if (m_direction == value)
                return;

            m_direction = value;
            OnChangeDirection?.Invoke();
        }
    }

    public bool IsMove
    {
        get => m_isMove;
        set
        {
            if (m_isMove == value)
                return;

            m_isMove = value;
            OnChangeMoveState?.Invoke();
        }
    }

    private Direction m_direction = Direction.DOWN;
    private bool m_isMove = false;
}

public enum Direction
{
    RIGHT,
    UP,
    LEFT,
    DOWN,
}

public static class DirectionUtils
{
    public static Direction Get4DirectionFromVector(Vector2 _vector)
    {
        float angle = Mathf.Atan2(_vector.y, _vector.x);
        int octant = (int)Mathf.Round(4 * angle / (2 * Mathf.PI) + 4) % 4;
        return (Direction)octant;
    }
    public static Direction Get8DirectionFromVector(Vector2 _vector)
    {
        float angle = Mathf.Atan2(_vector.y, _vector.x);
        int octant = (int)Mathf.Round(8 * angle / (2 * Mathf.PI) + 8) % 8;
        return (Direction)octant;
    }

    public static string ToPrettyString(this Direction _direction)
    {
        return _direction switch
        {
            Direction.UP => "Up",
            Direction.RIGHT => "Right",
            Direction.DOWN => "Down",
            Direction.LEFT => "Left",
            _ => throw new ArgumentException($"Can't convert to pretty string {nameof(_direction)}: '{_direction}'"),
        };
    }
}