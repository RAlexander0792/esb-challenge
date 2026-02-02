**Requirement:**

Design and implement a  Multiplayer Game Leaderboard System with match history and ranking calculations.
  

## Core Requirements

### Functional Requirements

1. Match Recording: Submit match results with players, scores, and match metadata (game mode, timestamp)
    
2. Leaderboard Query: Retrieve top N players globally or filtered by game mode
    
3. Player Profile: Get individual player stats including rank, total matches, win/loss ratio, and score history
    
4. Ranking Algorithm: Implement an ELO-like system that updates after each match
    
5. Match History: Retrieve recent matches for a player with opponents and outcomes
    
6. Season Support: Handle leaderboard resets for different seasons while preserving historical data
    

  

### Technical Requirements

1. API Design: RESTful API with at least four endpoints:
    - Submit match result
    - Query leaderboard (with filtering/pagination)
    - Get player profile and stats
    - Get match history
    
2. Data Persistence: Implement storage for players, matches, and rankings (in-memory acceptable but if you wanna try a MongoDB one as an exercise for you to see NoSql DBs that would be brilliant)
    
3. Ranking Logic: Implement a fair ranking algorithm that handles:
	- Initial player ratings
    - Rating adjustments based on match outcomes
    - Consideration of opponent strength
    
4. Performance: Leaderboard queries should be efficient even with thousands of players
    
5. Data Integrity: Handle concurrent match submissions correctly
 
  

## Deliverables

### 1. Architecture Document (short/high level one)

Brief written explanation covering:

- High-level system design and data flow
    
- Data model for players, matches, and rankings

- Ranking algorithm explanation and rationale
    
- How you'd handle concurrent match submissions
    
- Scalability considerations (millions of players, thousands of matches/minute)
    
- Trade-offs between accuracy, performance, and complexity
    

### 2. Implementation 

Working code including - in .net core - version is up to you, we currently use 8:

- Core game logic with clear separation of concerns
    
- API endpoints with request/response handling
    
- Ranking calculation implementation
    
- Data access layer
    
- README with setup, usage instructions, and algorithm explanation - this is optional as we will schedule a 20min meeting to walk through the solution and talk about it - basically Code and Architectural Review - be prepared to talk about extension and changes.
    

## Bonus Challenges (Optional)

- Streak tracking (win streaks, loss streaks)
    
