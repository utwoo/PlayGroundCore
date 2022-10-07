using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDemo.Models
{
    public class SubmissionInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string BeamDataId { get; set; }
        public string SubmissionId { get; set; }
        public int Revision { get; set; }
        public List<WorkLine> WorkLines { get; set; }
    }

    public class WorkLine
    {
        public string Type { get; set; }
        public string MachineId { get; set; }
        public string Algorithm { get; set; }
        public JarvisStatus Status { get; set; }
    }

    public enum JarvisStatus
    {
        DataPulling,
        DataPullingDone,
        DataCheck,
        DataCheckDone,
    }
}