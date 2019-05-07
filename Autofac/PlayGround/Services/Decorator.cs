using System;

namespace PlayGround.Services
{
    public class ColorDecorator : IGreeting
    {
        private IGreeting _greeting { get; }

        public string welcomeGreeting => $"Colored: {_greeting.welcomeGreeting}";

        public ColorDecorator(IGreeting greeting)
        {
            _greeting = greeting;
        }
    }

    public class BlackWhiteDecorator : IGreeting
    {
        private IGreeting _greeting { get; }

        public string welcomeGreeting => $"BlackWhite: {_greeting.welcomeGreeting}";

        public BlackWhiteDecorator(IGreeting greeting)
        {
            _greeting = greeting;
        }
    }
}
