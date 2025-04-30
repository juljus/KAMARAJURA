# Frontend Documentation

## Overview
This document outlines the goals and structure of the frontend for the KAMARAJURA game reviewing website. The frontend is built using Nuxt.js and interacts with the backend API to provide a seamless user experience.

## Pages

### 1. Main Page
- **Description**: Placeholder for now. No specific functionality is implemented yet.

### 2. Find Games
- **Description**: Allows users to search for games in the database.
- **Features**:
  - Search bar to query games.
  - Displays a list of games matching the search criteria.
  - Clicking on a game shows its details, including ratings and reviews.

### 3. Login/Sign Up
- **Description**: Handles user authentication.
- **Features**:
  - Login form for existing users.
  - Sign-up form for new users.
  - Displays a table of all users in the database (admin functionality).

### 4. Game Details
- **Description**: Displays detailed information about a selected game.
- **Features**:
  - Shows the game's rating (out of 10).
  - Displays user reviews.
  - Allows logged-in users to leave a review (rating and description).

## Backend Endpoints
The frontend interacts with the following backend API endpoints:

### User Endpoints
- **GET /api/user**: Retrieve all users.
- **GET /api/user/{id}**: Retrieve a user by ID.
- **POST /api/user**: Create a new user.
- **GET /api/user/test-connection**: Test database connection.

### Game Endpoints
- **GET /api/game**: Retrieve all games.
- **GET /api/game/{id}**: Retrieve a game by ID.
- **POST /api/game**: Create a new game.
- **PUT /api/game/{id}**: Update a game.
- **DELETE /api/game/{id}**: Delete a game.

### Rating Endpoints
- **GET /api/rating**: Retrieve all ratings.
- **GET /api/rating/{id}**: Retrieve a rating by ID.
- **POST /api/rating**: Create a new rating.
- **PUT /api/rating/{id}**: Update a rating.
- **DELETE /api/rating/{id}**: Delete a rating.

## Database Model Reference
Referencing the `backend/datamodel.md` file:

### Users Table
- **Columns**:
  - `user_id`: Primary key.
  - `user_name`: Username.
  - `user_password`: Password.

### Games Table
- **Columns**:
  - `game_id`: Primary key.
  - `game_name`: Name of the game.
  - `game_description`: Description of the game.

### Ratings Table
- **Columns**:
  - `rating_id`: Primary key.
  - `game_id`: Foreign key referencing `games`.
  - `user_id`: Foreign key referencing `users`.
  - `rating_timestamp`: Timestamp of the rating.
  - `rating_score`: Score given by the user.
  - `rating_description`: Review description.

## Notes
- Ensure the frontend properly handles API errors and edge cases.
- Update this document as new features or endpoints are added.