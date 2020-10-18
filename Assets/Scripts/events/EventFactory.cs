using UnityEngine.Events;

public class EventFactory
{
    public static GameEvent Create(EventNames name, UnityAction action)
    {
        return new GameEvent(name, action);
    }
}