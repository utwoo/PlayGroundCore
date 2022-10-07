using System;
using System.Threading.Tasks;
using MongoDemo;
using MongoDemo.Models;
using Newtonsoft.Json;

namespace MongoConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string connStr = "mongodb://elektaadmin:elektaadmin@20.247.126.140:27017";

            var mongoUtil = new MongoUtil(connStr);

            /*await mongoUtil.InsertAsync(new SubmissionInfo()
            {
                BeamDataId = Guid.NewGuid().ToString(),
                SubmissionId = "MR-000000001",
                Revision = 1,
                WorkLines = new List<WorkLine>()
                {
                    new WorkLine()
                    {
                        Type = "Photon",
                        MachineId = "Elekta06",
                        Algorithm = "Monte Carlo",
                        Status = JarvisStatus.DataPulling
                    },
                    new WorkLine()
                    {
                        Type = "Photon",
                        MachineId = "Elekta06",
                        Algorithm = "Cone",
                        Status = JarvisStatus.DataPulling
                    },
                    new WorkLine()
                    {
                        Type = "Photon",
                        MachineId = "Elekta10",
                        Algorithm = "Monte Carlo",
                        Status = JarvisStatus.DataCheck
                    },
                    new WorkLine()
                    {
                        Type = "Photon",
                        MachineId = "Elekta10",
                        Algorithm = "Cone",
                        Status = JarvisStatus.DataCheck
                    }
                }
            });
            */

            // var result = await mongoUtil.GetSubmissionInfoById("e308e469-df0b-4761-a69f-12fe4f4815f9");
            // var result = await mongoUtil.GetWorkLineAsync("MR-00000001", "Photon", "Elekta06", "Monte Carlo");
            await mongoUtil.UpdateStatus("MR-00000001", "Photon", "Elekta06", "Monte Carlo", JarvisStatus.DataCheck);

            //Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}