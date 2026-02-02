## ESB Challenge

The solution name is Mgls -> Multiplayer Game Leaderboard System


### Local solution setup
In "/local-infra" there is a docker-compose.yml file with a MongoDB and Redis databases.
The Redis database is configured authless.
Other configurable options are exposed in ".env.example", file that can just be used insecurely as it is, by renaming to ".env"

To run the dotnet solution, the local execution allows for no extra configuration if the local infra was used.
If ran with docker, then one must ensure network reach between the Mgls application and both the MongoDB and Redis instance, through it's connection strings
configurable at the "appsettings.json" localed at "src/Mgls/Mgls.Api/appsettings.json"

### Getting started
The Application is composed of the following main entities:
- Player
    - Represents a User of the application, can be a participant of Matches
- Leaderboard
    - Represents a ranked list of players ratings
    - Has a specific Rating Ruleset, which dictates how a player rating change is calculated after submiting a Match.
- Match
    - Represents one instance of a game, with a list of participant players and their respective scores.
    - References a required Leaderboard
- RatingRuleset
    - Represents a combination of a specific Rating Resolver and parameters configuration
- RatingResolver
    - Represents the calculator of player rating changes, is configurable through RatingRuleset parameters
- LeaderboardPlayer
    - Aggregates the outcomes of all matches for a Player in a specific Leaderboard
    - Is feed by atomics LeaderBoardPlayerRatingAudit entities

##### To have a testable environment
At least one RatingRuleset should be created, utilizing one RatingResolver.
Then a Leaderboard can be created, as it needs a RatingRuleset.
From this point, Matches can be created, which will require first the creation of at least 2 Players.

##### UtilitiesCli
A standalone console application.
It must have access to the same "appsettings.json" configuration as "Mgls.Api/appsettings.json"
It expects the Mgls.Api server to be running at localhost:7010

Within it, 3 main actions can be done:
- DataGeneration
    - This actions allow the creation of objects through HTTP
    - Can be used to generate test players with random username in bulk
        - CodeGenerator -> Player -> Input how many
    - Can be used to generate Matches
        - It will prompt for the parameters to use, most have defaults but selecting an existing Leaderboard id is required.
        - "OneMatch" generates a single match
        - "Continuous" constantly generates a new match, with a configurable delay between calls.
- RunBenchmark
    - Only available when building the CLI in Release
    - Meant to be used for testing performance of Rating Resolvers implementations
- PlotCharts
    - A simple plotting mechanism, used as an early way to validate the Rating Resolvers

The code of this tool is done "quick and dirty", as it is meant just as a setup simplifier, and not a real part of the Mgls application.


### Known improvements for a real application
- Error handling is basic and non exhaustive
    - A global error handler as a Middleware should be added to handle and log all unhandled errors
    - Specific error catching should be added to the different layers, like MongoDB exceptions on the Mgls.Data project, Redis errors on the Mgls.Application SortedLeaderboardService
    - MatchRatingProcessor is brittle and could lose Match rating changes processing, as it lacks proper error handling and some way of alerting when a critical error happens while processing a Match (as it could skew the rest of the leaderboard matchs processings)
- Logging is not exhaustive
- CRUD operations are not exposed for all entities
- Unit tests should cover all operations
- Integrations tests should cover critical paths for the application
- "leaderboardPlayerRatingAudits" MongoDB collection should be backuped when a Leaderboard ends, as it's a fast growing collection, and once a leaderboard is closed, it is only needed for historical/audit reasons.
- a Match is won if there was an positive rating change for the player, but it should be determined by the rules of the game.
- Different types of Dtos were introduced, but they don't cover all layers correctly.
    - We are reutilizing Entities for API requests, instead of proper Requests Dtos
    - An object mapper would be used to handle the different Dto transformations across layers
- SortedLeaderboardService should hide Redis details


### Time
A total of around 34 hours were invested
- 9 hs on design of data models and research on game ratings, MongoDB practices and Redis sorted collections types.
- 25 hs on implementation from which
    - around 6 hs were for the UtilitiesCli, which helped in being able to generate enough data to validate the application

### AI agents
With the intention of measuring only my baseline, no integrated coding agents were used for this development.
AI Chatbots were used for research, consulting, troubleshooting and text rephrasing.