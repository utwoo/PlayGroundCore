using System;

namespace EventHandlers
{
    public class Cat
    {
        public EventHandler<WakeEventArgs> WakeEventHandler;

        public void Wake()
        {
            if (this.WakeEventHandler != null)
            {
                WakeEventHandler(this, new WakeEventArgs { dateWake = DateTime.Now });
            }
        }
    }
}
