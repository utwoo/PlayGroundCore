using LaunchDarkly.Client;
using System;

namespace LaunchDarklyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create LDClient with environment-specific key
            LdClient ldClient = new LdClient("sdk-986ed65e-0b95-406f-97d5-b66637297b05");

            // Create User
            User user = User.WithKey("Utwoo")
                .AndFirstName("Zhu")
                .AndLastName("Xiang")
                .AndCustomAttribute("groups", "testers");

            // Call LaunchDarkly
            bool value = ldClient.BoolVariation("greeting-changes", user, false);
            if (value)
            {
                Console.WriteLine("Showing feature for user " + user.Key);
            }
            else
            {
                Console.WriteLine("Not showing feature for user " + user.Key);
            }

            ldClient.Flush();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
