using System;
using System.Collections.Generic;
using System.Text;

namespace PlayGround.Services
{
    public interface IGreeting
    {
        string welcomeGreeting { get; }
    }

    public class EnglishGreeting : IGreeting
    {

        public EnglishGreeting()
        {
        }

        public string welcomeGreeting
        {
            get => "Welcome";
        }
    }
}
