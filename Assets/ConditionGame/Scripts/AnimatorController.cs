using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    private MovementState m_movement;


    private Dictionary<Direction, int> m_anims_move;
    private Dictionary<Direction, int> m_anims_idle;

    private Animator m_animator;

    //===============================================================//
    // Lifecycles 

    private void Awake()
    {
        m_movement = GetComponent<MovementState>();

        m_animator = GetComponent<Animator>();

        m_anims_move = new() {
                { Direction.UP,        Animator.StringToHash($"Move{Direction.UP.ToPrettyString()}")        },
                { Direction.LEFT,      Animator.StringToHash($"Move{Direction.LEFT.ToPrettyString()}")      },
                { Direction.DOWN,      Animator.StringToHash($"Move{Direction.DOWN.ToPrettyString()}")      },
                { Direction.RIGHT,     Animator.StringToHash($"Move{Direction.RIGHT.ToPrettyString()}")     },
            };

        m_anims_idle = new() {
                { Direction.UP,        Animator.StringToHash($"Idle{Direction.UP.ToPrettyString()}")        },
                { Direction.LEFT,      Animator.StringToHash($"Idle{Direction.LEFT.ToPrettyString()}")      },
                { Direction.DOWN,      Animator.StringToHash($"Idle{Direction.DOWN.ToPrettyString()}")      },
                { Direction.RIGHT,     Animator.StringToHash($"Idle{Direction.RIGHT.ToPrettyString()}")     },
            };

        m_movement.OnChangeDirection += ChangeAnimation;
        m_movement.OnChangeMoveState += ChangeAnimation;
    }

    private void OnDestroy()
    {
        m_movement.OnChangeDirection -= ChangeAnimation;
        m_movement.OnChangeMoveState -= ChangeAnimation;
    }

    void ChangeAnimation()
    {
        if (m_movement.IsStay)
            m_animator.Play(m_anims_idle[m_movement.Direction]);
        else
            m_animator.Play(m_anims_move[m_movement.Direction]);
    }
}