using System;
using DialogueEditor;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCConversation _dialog;

    public void Speak()
    {
        ConversationManager.Instance.StartConversation(_dialog);
    }
}