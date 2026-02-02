### ESB Challenge

The solution name is Mgls -> Multiplayer Game Leaderboard System


#### Local solution setup
In "/local-infra" there is a docker-compose.yml file with a MongoDB and Redis databases.
The Redis database is configured authless.
Other configurable options are exposed in ".env.example", file that can just be used insecurely as it is, by renaming to ".env"

To run the dotnet solution, the local execution allows for no extra configuration if the local infra was used.
If ran with docker, then one must ensure network reach between the Mgls application and both the MongoDB and Redis instance, through it's connection strings
configurable at the "appsettings.json" localed at "src/Mgls/Mgls.Api/appsettings.json"
