using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDemo.Models;
using Newtonsoft.Json.Converters;

namespace MongoDemo
{
    public class MongoUtil
    {
        private readonly IMongoCollection<SubmissionInfo> _submissionsInfo;

        public MongoUtil(string connectionString)
        {
            var mongoClient = new MongoClient(connectionString);
            _submissionsInfo = mongoClient.GetDatabase("jarvis").GetCollection<SubmissionInfo>("submission_info");
        }

        public IMongoQueryable<SubmissionInfo> GetQueryable()
        {
            return _submissionsInfo.AsQueryable();
        }

        public async Task InsertAsync(SubmissionInfo submissionInfo)
        {
            await _submissionsInfo.InsertOneAsync(submissionInfo);
        }

        public async Task<SubmissionInfo> GetSubmissionInfoById(string beamDataId)
        {
            var result =
                await _submissionsInfo.AsQueryable()
                    .FirstOrDefaultAsync(c => c.BeamDataId == beamDataId);

            return result;
        }

        public async Task<WorkLine> GetWorkLineAsync(string submissionId, string type, string machineId, string algorithm)
        {
            var submission =
                await _submissionsInfo.AsQueryable()
                    .FirstOrDefaultAsync(c => c.SubmissionId == submissionId);

            var result = submission.WorkLines.SingleOrDefault(w => w.Type == type && w.MachineId == machineId && w.Algorithm == algorithm);
            return result;
        }

        public async Task UpdateStatus(string submissionId, string type, string machineId, string algorithm, JarvisStatus status)
        {
            var filter = Builders<SubmissionInfo>.Filter.Eq(x => x.SubmissionId, submissionId);
            var update = Builders<SubmissionInfo>.Update
                .Set("WorkLines.$[f].Status", status)
                .Set("WorkLines.$[f].Type", "11111");

            var filterDic = new Dictionary<string, object>()
            {
                { "f.Type", type },
                { "f.MachineId", machineId },
                { "f.Algorithm", algorithm }
            };

            var arrayFilters = new[]
            {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument(filterDic))
            };

            await _submissionsInfo.UpdateOneAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters });
        }
    }
}