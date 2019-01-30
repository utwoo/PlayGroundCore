using System;
using DotNetCore.CAP;

namespace ASPNetCoreWithCAP.Services
{
    public class SubscriberService : ICapSubscribe
    {
        [CapSubscribe("xxx.services.show.time", Group = "GroupAlpha")]
        public void CheckReceivedMessageInGroupAlpha(DateTime datetime)
        {
            Console.WriteLine($"Group Alpha: {datetime}");
        }

        [CapSubscribe("xxx.services.show.time", Group = "GroupBeta")]
        public void CheckReceivedMessageInGroupBeta(DateTime datetime)
        {
            Console.WriteLine($"Group Beta: {datetime}");
        }
    }
}
