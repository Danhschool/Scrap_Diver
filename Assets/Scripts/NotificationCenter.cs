using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class NotificationCenter
{
    public static Action OnNotificationChanged;

    public static void TriggerUpdate()
    {
        OnNotificationChanged?.Invoke();
    }
}
