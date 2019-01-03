var config = { 
    _id:"rs1", 
    members:[
        { _id : 0, host : "mongoindocker_shard_rs1_1_1:27018" },
        { _id : 1, host : "mongoindocker_shard_rs1_2_1:27018" },
        { _id : 2, host : "mongoindocker_shard_rs1_3_1:27018" }
    ]};
rs.initiate(config);
