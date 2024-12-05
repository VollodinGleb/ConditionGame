using DialogueEditor;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private NPCConversation _dialog;
    [SerializeField] private bool _isOpen;

    public void Use()
    {
        if (_isOpen) Destroy(gameObject);
        else ConversationManager.Instance.StartConversation(_dialog);
    }
}