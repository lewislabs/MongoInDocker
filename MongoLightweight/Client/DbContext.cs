using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Client
{
    public class DbContext
    {
        internal readonly MongoClient ClientContext;
        private readonly string _dbName;
        public DbContext()
        {
            _dbName = "Main";
            ClientContext = new MongoClient("mongodb://127.0.0.1:27017,127.0.0.2:27017,127.0.0.3:27017");
        }

        private IMongoCollection<Person> GetTestCollection()
        {
            return ClientContext.GetDatabase(_dbName).GetCollection<Person>("test_collection");
        }

        public async Task ListDatabases()
        {
            using (var cursor = await ClientContext.ListDatabasesAsync())
            {
                await cursor.ForEachAsync(d => Console.WriteLine(d.ToString()));
            }
        }

        public async Task FindPerson(string name)
        {
            var collection = GetTestCollection();

            var fb = new FilterDefinitionBuilder<Person>();
            var result = await collection.FindAsync(fb.Eq(p => p.Name, name));
            var document = result.ToList().FirstOrDefault();
            if (document != null)
            {
                Console.WriteLine("Document Found");
            }
        }

        public async Task CreatePerson(string name, int age)
        {
            var collection = GetTestCollection();

            await collection.InsertOneAsync(new Person()
            {
                Name = name,
                Age = age,
            });
            Console.WriteLine($"Document Created:{name} - {age}");
        }
    }
}