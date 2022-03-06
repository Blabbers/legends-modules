using System.Collections.Generic;

public class Locker
{
    private readonly HashSet<object> locks = new HashSet<object>();
    public bool IsLocked => locks.Count > 0;
    
    public static implicit operator bool(Locker @lock) {
        return !@lock.IsLocked;
    }

    public void AddLock(object @lock)
    {
        locks.Add(@lock);
    }

    public void RemoveLock(object @lock)
    {
        locks.Remove(@lock);
    }
}