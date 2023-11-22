using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    [System.Serializable]
    public class TriggeredEvent
    {
        public string ID;
        public UnityEvent Event = new UnityEvent();
    }

    public List<TriggeredEvent> Events = new List<TriggeredEvent>();

    public void InvokeEvent(string eventID)
    {
        TriggeredEvent @event = Events.FirstOrDefault(x => x.ID == eventID);

        if (@event != null)
        {
            @event.Event?.Invoke();
        }
        else
        {
            Debug.LogError($"Trigger '{eventID}' not found!");
        }
    }
}
