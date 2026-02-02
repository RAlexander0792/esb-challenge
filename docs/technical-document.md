## High-level system design and data flow
Sequence diagram for Match flow available at [docs\match_flow.svg](match_flow.svg)

## Data model for players, matches, and rankings
Class diagram available at [docs\class-diagram.svg](class-diagram.svg)

## Ranking algorithm explanation and rationale
There are 2 Player Rating algorithms implemented.

#### Classic Elo
An implementation of the Elo rating system originally developed by Árpád Elo, adhering closely to the classic algorithm.
The K-factor controls the magnitude of rating adjustments: higher values produce larger rating swings, while lower values result in more gradual changes, requiring more matches for a player’s rating to accurately reflect their skill.
Supports two-player matches only, with no score-based outcomes.

#### Elo Plus
A variation of the classic Elo rating system that incorporates score differentials into the outcome calculation, alongside support for more than 2 players.

The Actual outcome is derived from the score difference and compared against the Expected outcome computed from rating differences; the influence of score magnitude is controlled by the Tau parameter.

The system supports free-for-all matches with multiple participants, where each player is evaluated against every other player.
The K-factor is dynamically scaled based on the number of participants to maintain a consistent relationship between rating changes and player count.

## How you'd handle concurrent match submissions
Match submissions involving the same players are not expected to occur within a short time window. As a result, concurrent access contention on the LeaderboardPlayer object—which holds the player-specific data required for rating calculations—is not a concern.

When applying rating updates, a Version field is stored alongside the MatchId of the most recently processed match. These values are checked at database write time to ensure that, even in the presence of race conditions, rating updates are applied in the correct order.

## Scalability considerations (millions of players, thousands of matches/minute)
At higher volumes (thousands of matches), match processing should be handled through a dedicated queue, complemented by a dead-letter queue and critical alerts for failed processing. Because processing order is significant—and real-world matchmaking systems depend on up-to-date, reliable ratings for fairness—failures must be detected and addressed immediately.

The current in-application processing approach is insufficient to handle this load gracefully.

For ranking queries, a caching layer is already in place; however, at scale, this cache should be distributed.

## Trade-offs between accuracy, performance, and complexity
Cache TTLs should be configured per entity to strike an optimal balance between data freshness and performance.
For example, profile queries could use a 1-minute TTL, allowing responses to be up to one minute stale while significantly reducing database load.

Leaderboard caches are partitioned by leaderboardId. If a leaderboard represents a regional season, its cache nodes can be deployed closer to that region to minimize read latency.

Dedicated subsystems can be introduced to scale independently. For instance, match processing could run as a separate application, consuming events from a queue.