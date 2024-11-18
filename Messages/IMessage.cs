public enum MessagePriority : int
{
    Urgent = 0,   // 매우 긴급한 우선순위
    Critical = 1, // 중요한 우선순위
    High = 2,     // 높은 우선순위
    Normal = 3,   // 보통 우선순위
    Low = 4       // 낮은 우선순위
}

public interface IMessage
{
    int Id { get; }
    MessagePriority Priority { get;}  // 메시지 우선순위
    void Execute(); // 메시지 실행 메서드
    void SetId(int id);  // 메시지 ID 설정
}
