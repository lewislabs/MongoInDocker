directory_name="${PWD##*/}"
docker-compose up -d
docker exec -d ${directory_name}_config1_1 mongo localhost:27019 initiate_config_server.js
## SAME For shard
docker exec -d ${directory_name}_config1_1 mongo localhost:27019 initiate_config_server.js
