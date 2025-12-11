using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledEvent
{
    public bool isEnd = false;
    public bool isRepeat = false;
    
    public float interval;
    public DateTime nextTime;

    public Action<object> callback;
    public object parameter;

    public ScheduledEvent(Action<object> callback, object parameter, float interval, bool isRepeat = false)
    {
        this.callback = callback;
        this.parameter = parameter;
        this.interval = interval;
        this.isRepeat = isRepeat;
        
        nextTime = DateTime.Now + TimeSpan.FromSeconds(interval);
    }
}

public class ScheduledEventComparer : IComparer<ScheduledEvent>
{
    public int Compare(ScheduledEvent x, ScheduledEvent y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        return x.nextTime.CompareTo(y.nextTime);
    }
}
