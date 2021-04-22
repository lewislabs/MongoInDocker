directory_name="${PWD##*/}"
directory_name=$(echo "$directory_name" | tr '[:upper:]' '[:lower:]')
docker-compose up -d
docker exec -d ${directory_name}_config1_1 mongo localhost:27019 tmp/scripts/initiate_servers.js
docker exec -d ${directory_name}_shard_rs1_1_1 mongo localhost:27018 tmp/scripts/initiate_shards.js
docker exec -d ${directory_name}_router1_1 mongo localhost:27117 tmp/scripts/add_shard.js

dotnet run --project ./MongoLightWeight/Client/Client.csproj
