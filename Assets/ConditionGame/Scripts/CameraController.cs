using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform _player;

    [Header("Camera Movement")]
    [SerializeField] private float _followSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = new(_player.position.x, _player.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
    }
}