directory_name="${PWD##*/}"
docker-compose up -d
docker exec -d ${directory_name}_config1_1 mongo localhost:27019 tmp/scripts/initiate_config_server.js
docker exec -d ${directory_name}_shard_rs1_1_1 mongo localhost:27018 tmp/scripts/initiate_shards.js
docker exec -d ${directory_name}_router1_1 mongo localhost:27017 tmp/scripts/add_shard.js
