using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private SortedList<MessagePriority, Queue<IMessage>> messageQueue = new SortedList<MessagePriority, Queue<IMessage>>();

    private const int BatchSize = 10; //�����Ӵ� �޽��� ó���� ����
    private int messageCounter = 0;

    private static MessageManager instance;

    public static MessageManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<MessageManager>();
                if (instance == null) {
                    GameObject messageManagerObject = new GameObject("MessageManager");
                    instance = messageManagerObject.AddComponent<MessageManager>();
                }
            }
            return instance;
        }
    }

    public void ProcessMessages() {
        int processedMessages = 0;

        foreach (var priority in messageQueue.Keys) {
            Queue<IMessage> queue = messageQueue[priority];

            while (queue.Count > 0 && processedMessages < BatchSize) {
                IMessage message = queue.Dequeue();
                LogMessageProcessing(message);
                message.Execute();
                processedMessages++;
            }
        }
    }

    private void LogMessageProcessing(IMessage message) {
        Debug.Log($"{Time.time:F2}s <color=cyan>[�޽��� ó��:{message.Id}]</color> �޽��� ({message.GetType().Name}) ó�� ��.");
    }

    public void EnqueueMessage(IMessage message) {
        message.SetId(messageCounter++);  // �޽��� ID �ο�

        // �켱������ �������� ������ �⺻ ������ ����
        if (!messageQueue.ContainsKey(message.Priority)) {
            messageQueue.Add(message.Priority, new Queue<IMessage>());
        }

        messageQueue[message.Priority].Enqueue(message);
        Debug.Log($"{Time.time:F2}s <color=cyan>[�޽��� �߰�:{message.Id}]</color> �޽��� ({message.GetType().Name}) {message.Priority} �켱���� ť�� �߰�. ť ũ��: {messageQueue.Values.Sum(queue => queue.Count)}");
    }


}
