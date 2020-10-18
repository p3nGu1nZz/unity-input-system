using UnityEngine.Events;

public class GameEvent
{
    public string Name { get; set; }
    public UnityAction Action { get; set; }
    public GameEvent(EventNames name, UnityAction action)
    {
        Name = name.ToString();
        Action = action;
    }

    public void Start()
    {
        EventManager.StartListening(Name, Action);
    }

    public void Stop()
    {
        EventManager.StopListening(Name, Action);
    }

    public void Trigger()
    {
        EventManager.TriggerEvent(Name);
    }
}