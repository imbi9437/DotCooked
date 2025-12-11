using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using UnityEngine;

public class Scheduler : MonoSingleton<Scheduler>
{
    private PriorityQueue<ScheduledEvent> eventQueue;
    private IComparer<ScheduledEvent> eventComparer = new ScheduledEventComparer();

    protected override void Awake()
    {
        base.Awake();
        eventQueue = new PriorityQueue<ScheduledEvent>(eventComparer);
    }
    
    private void Update()
    {
        if (eventQueue.TryPeek(out var scheduledEvent) == false) return;
        if (scheduledEvent.isEnd)
        {
            eventQueue.Dequeue();
            return;
        }
        if (DateTime.Now < scheduledEvent.nextTime) return;
        
        var item = eventQueue.Dequeue();
        item.callback?.Invoke(scheduledEvent.parameter);

        if (item.isRepeat)
        {
            scheduledEvent.nextTime = DateTime.Now + TimeSpan.FromSeconds(scheduledEvent.interval);
            eventQueue.Enqueue(scheduledEvent);
        }
    }

    public static void RegisterEvent(ScheduledEvent scheduledEvent)
    {
        Instance.eventQueue.Enqueue(scheduledEvent);
    }
}
