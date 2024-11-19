public enum MessagePriority : int
{
    Urgent = 0,   // �ſ� ����� �켱����
    Critical = 1, // �߿��� �켱����
    High = 2,     // ���� �켱����
    Normal = 3,   // ���� �켱����
    Low = 4       // ���� �켱����
}

public interface IMessage
{
    int Id { get; }
    MessagePriority Priority { get;}  // �޽��� �켱����
    void Execute(); // �޽��� ���� �޼���
    void SetId(int id);  // �޽��� ID ����
}
