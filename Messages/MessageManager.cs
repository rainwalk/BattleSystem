using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private SortedList<MessagePriority, Queue<IMessage>> messageQueue = new SortedList<MessagePriority, Queue<IMessage>>();

    private const int BatchSize = 10; //프레임당 메시지 처리량 제한
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
        Debug.Log($"{Time.time:F2}s <color=cyan>[메시지 처리:{message.Id}]</color> 메시지 ({message.GetType().Name}) 처리 중.");
    }

    public void EnqueueMessage(IMessage message) {
        message.SetId(messageCounter++);  // 메시지 ID 부여

        // 우선순위가 설정되지 않으면 기본 값으로 설정
        if (!messageQueue.ContainsKey(message.Priority)) {
            messageQueue.Add(message.Priority, new Queue<IMessage>());
        }

        messageQueue[message.Priority].Enqueue(message);
        Debug.Log($"{Time.time:F2}s <color=cyan>[메시지 추가:{message.Id}]</color> 메시지 ({message.GetType().Name}) {message.Priority} 우선순위 큐에 추가. 큐 크기: {messageQueue.Values.Sum(queue => queue.Count)}");
    }


}
