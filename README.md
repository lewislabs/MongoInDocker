## Mongo In Docker example

This example projects creates a mongo infrastructure within docker:

- 3 Config Servers
- 3 Mongo Routers (mongos)
- 3 Sharded Replica Sets

It then launches a .Net Core console application which will begin making queries against the routers. It will display the number of queries run against each router and the status of that router.

```
Server Status:
    127.0.0.1:27117 (ShardRouter) (2005) Connected
    127.0.0.1:27118 (ShardRouter) (2367) Connected
    127.0.0.1:27119 (ShardRouter) (2369) Connected

Press ctrl+c to exit...
```

This is to demonstrate how the Mongo C# Driver handles connections to multiple routers.