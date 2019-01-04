var config = {
    _id:"config_servers",
    configsvr:true,
    version: 1,
    members:[
        { _id : 0, host : "mongoindocker_config1_1:27019" },
        { _id : 1, host : "mongoindocker_config2_1:27019" },
        { _id : 2, host : "mongoindocker_config3_1:27019" }
    ]};

rs.initiate(config);
