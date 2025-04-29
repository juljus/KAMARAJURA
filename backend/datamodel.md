# Database Model Documentation

This document provides an overview of the database structure for the project. Below are the details of the tables and their respective fields.

## Tables

### 1. Users Table
- **Table Name**: `users`
- **Description**: Stores information about the users of the application.

| Column Name   | Data Type      | Description                       |
|---------------|----------------|-----------------------------------|
| `user_id`     | `INT`          | Primary key, auto-incremented.    |
| `user_name`   | `NVARCHAR(MAX)`| The username of the user.         |
| `user_password`| `NVARCHAR(MAX)`| The password of the user.         |

### 2. Games Table
- **Table Name**: `games`
- **Description**: Stores information about the games being reviewed.

| Column Name       | Data Type        | Description                                |
|-------------------|------------------|--------------------------------------------|
| `game_id`         | `INT`            | Primary key, auto-incremented.             |
| `game_name`       | `NVARCHAR(MAX)`  | The name of the game.                      |
| `game_description`| `NVARCHAR(MAX)`  | A description of the game.                 |
| `game_image`      | `VARBINARY(MAX)` | Binary data representing the game's image. |

### 3. Ratings Table
- **Table Name**: `ratings`
- **Description**: Stores user ratings for games.

| Column Name         | Data Type      | Description                                |
|---------------------|----------------|--------------------------------------------|
| `rating_id`         | `INT`          | Primary key, auto-incremented.             |
| `game_id`           | `INT`          | Foreign key referencing `games.game_id`.   |
| `user_id`           | `INT`          | Foreign key referencing `users.user_id`.   |
| `rating_timestamp`  | `DATETIME`     | The timestamp of when the rating was made. |
| `rating_score`      | `INT`          | The score given by the user.               |
| `rating_description`| `NVARCHAR(MAX)`| A description or comment for the rating.   |

## Notes
- Ensure that all table and column names match the database schema.
- Update this document whenever changes are made to the database structure.