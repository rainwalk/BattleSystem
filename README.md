## 프로젝트 소개

대부분의 게임에서 전투 콘텐츠는 지속적으로 서비스가 진행됨에 따라 새로운 요소들과 스킬들이 지속적으로 추가됩니다. 이러한 확장은 시스템의 복잡성을 증가시키고 기존 코드의 재사용성을 저하시킬 위험이 있습니다. 특히, 스킬 간의 상호작용에서 발생하는 예상치 못한 타이밍 이슈나 충돌 문제는 유지보수를 어렵게 만들고, 새로운 기능 추가 시 시스템 안정성에 악영향을 미칠 수 있습니다.

이 프로젝트는 이러한 문제점들의 발생을 최소화하는 데 중점을 두었습니다. 핵심은 **메시지 시스템을 통한 전투 흐름의 관리와 스킬의 확장성과 재사용성**을 최우선으로 고려한 설계입니다.  **메시지 시스템**을 통해 전투 중 발생하는 다양한 행동들을 메시지를 중앙에서 관리하고, 메시지 간 우선순위를 조정을 가능하게 함으로써 동시에 발생하는 메시지들의 타이밍이슈에 효과적으로 대응할 수 있도록 설계했습니다. 이러한 방식은 스킬 실행의 일관성을 유지하고, 충돌이나 타이밍 오류와 같은 예기치 못한 이슈를 최소화하여 보다 안정적이고 효율적인 전투 시스템을 구현 할 수 있도록 해줍니다.

```csharp
public enum MessagePriority : int
{
    Urgent = 0,   // 매우 긴급한 우선순위
    Critical = 1, // 중요한 우선순위
    High = 2,     // 높은 우선순위
    Normal = 3,   // 보통 우선순위
    Low = 4       // 낮은 우선순위
}
```

또한 기존에 만들어진 다양한 이펙트를 모듈화하여, 이를 조합하여 각 스킬에 독립적으로 추가 및 수정할 수 있는 구조로 만들었습니다. 이를 통해 스킬 시스템의 유지보수성과 확장성을 높이고, 재사용성을 높여 새로운 스킬 추가 및 수정 작업이 효율적이고 직관적으로 이루어질 수 있도록 했습니다.

예시) 기존에 만들어진 2가지 즉시 효과와 1가지 상태 효과를 조합하여 스킬 생성:

```csharp
public class EMPShockwaveSkill : SkillBase
{
    public EMPShockwaveSkill() : base("EMP 충격파", SkillTargetType.Single, 10f, 2f) {
        AddEffect(new DamageEffect(30));
        AddEffect(new RepairEffect(20));
        SetStatusEffect(new StunEffect(5));
    }
}

```


이 프로젝트는 기존에 개발했던 전투 시스템을 단순화하여 만들어졌습니다. 이를 위해 각 스킬들의 데이터 설정, 오브젝트 풀 구조, 연출 등의 부가적인 처리들을 제외하고, 앞서 이야기한 부분과 관련한 주요 로직을 넣는데 중점을 두었습니다.

![image](https://github.com/user-attachments/assets/7e0058d4-4dd3-4164-a6af-d4d6e4a469dd)

## 주요 클래스
#### BattleManager
- 게임의 전투를 관리하는 클래스입니다.
- 전투가 진행되는 동안 함대와 메시지 처리를 담당합니다.
- 플레이어와 적의 함대를 초기화하고, 각 함선에 스킬을 추가합니다.
#### BattleLoop 
- 전투의 주요 루프를 담당합니다.
- 플레이어 함대와 적 함대의 함선의 상태를 체크하고 스킬 사용 및 공격을 시도합니다.
#### Fleet
- 함대를 처리하는 클래스입니다. 함선들을 관리합니다
#### Ship
- 함선의 상태를 관리하는 클래스입니다.
- 수리, 공격 및 스킬 처리를 수행하며, 스킬이나 상태 효과에 의해 영향을 받을 수 있습니다.
#### Weapon
- 함선의 무기 종류와 그에 따른 데미지, 공격 속도를 관리하는 클래스입니다.
#### AttackHandler
- 공격 쿨타임을 관리하고, 공격을 실행하는 클래스입니다. 
#### SkillHandler
- 함선의 스킬 처리 시스템입니다. 스킬의 상태를 업데이트하고 스킬을 사용하는 클래스입니다.
#### SkillBase
- 스킬의 기본 클래스입니다.
- 스킬은 쿨다운, 캐스트 시간, 효과 등을 관리합니다
#### EffectBase 및 IEffect
- 상태 효과(예: 피해, 수리)를 적용하는 추상 클래스와 인터페이스입니다.
- `DamageEffect`와 `RepairEffect` 같은 구체적인 상태 효과 클래스를 통해 함선에 영향을 줍니다.
#### IStatusEffect
- 함선에 적용되는 상태 효과를 나타내는 인터페이스입니다.
- `StunEffect`같은 함선을 마비시키는 효과를 제공합니다.
#### MessageManager
- 메시지 큐 시스템을 관리하는 클래스입니다. 각 메시지는 우선순위에 따라 큐에 추가되고 처리됩니다.
- 메시지는 전투 중 발생한 다양한 이벤트(예: 공격, 스킬 사용)를 처리합니다.
#### Message 및 AttackMessage, UseSkillMessage
- 전투 중 발생하는 이벤트 메시지를 처리하는 클래스들입니다.
- `AttackMessage`는 공격을 실행하고, `UseSkillMessage`는 스킬을 사용하는 메시지를 처리합니다.
