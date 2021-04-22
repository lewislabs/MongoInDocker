using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Client
{
    public class DbContext
    {
        internal readonly MongoClient ClientContext;
        private readonly string _dbName;
        private readonly Dictionary<string, int> _queryCount = new Dictionary<string, int>();

        public IEnumerable<(string Host, int Count)> GetCounts() => _queryCount.Select((item) => (item.Key, item.Value));

        public DbContext()
        {
            _dbName = "Main";
            var connectionUrl = new MongoUrl($"mongodb://127.0.0.1:27117,127.0.0.1:27118,127.0.0.1:27119/{_dbName}");
            var connectionSettings = new MongoClientSettings() {
                Servers = connectionUrl.Servers,
                ClusterConfigurator = cb => {
                    cb.Subscribe<CommandStartedEvent>(e => {
                        var cmd = e.CommandName.ToLower();
                        if (cmd == "find" || cmd == "insert" || cmd == "listDatabases")
                        {
                            var server = e.ConnectionId.ServerId.EndPoint.ToString();
                            if (_queryCount.TryGetValue(server, out var count)) {
                                _queryCount[server] = count + 1;
                            }
                        }
                    });
                },
            };
            ClientContext = new MongoClient(connectionSettings);
            foreach(var srv in ClientContext.Settings.Servers)
            {
                Console.WriteLine($"initializing counts for: {srv.Host}:{srv.Port}");
                _queryCount.TryAdd($"{srv.Host}:{srv.Port}", 0);
            }
        }

        private IMongoCollection<Person> GetTestCollection()
        {
            var db = ClientContext.GetDatabase(_dbName);
            return db.GetCollection<Person>("test_collection");
        }

        public async Task ListDatabases()
        {
            string results = "";
            using (var cursor = await ClientContext.ListDatabasesAsync())
            {
                await cursor.ForEachAsync(d => results += d.ToString());
            }
        }

        public async Task FindPerson(string name)
        {
            var collection = GetTestCollection();

            var fb = new FilterDefinitionBuilder<Person>();
            var result = await collection.FindAsync(fb.Eq(p => p.Name, name));
            var document = result.ToList().FirstOrDefault();
        }

        public async Task CreatePerson(string name, int age)
        {
            var collection = GetTestCollection();

            await collection.InsertOneAsync(new Person()
            {
                Name = name,
                Age = age,
            });
        }
    }
}