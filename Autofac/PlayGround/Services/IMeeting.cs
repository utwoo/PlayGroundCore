using System;

namespace PlayGround.Services
{
    public interface IMeeting
    {
        string GetGreetingMessage(string message);
    }

    public class Meeting : IMeeting
    {
        private IGreeting _greeting;

        public Meeting(
           IGreeting greeting)
        {
            this._greeting = greeting;
        }

        public string GetGreetingMessage(string message)
        {
            return $"{_greeting.GetHashCode()},{_greeting.welcomeGreeting},{message}";
        }
    }
}
