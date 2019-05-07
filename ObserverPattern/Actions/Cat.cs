using System;

namespace Actions
{
    public class Cat
    {
        public Action<DateTime> actionWaken;

        public void Wake()
        {
            if (this.actionWaken != null)
            {
                actionWaken(DateTime.Now);
            }
        }
    }
}
