ProjectMovieBookDB

Overview

ProjectMovieBookDB is a .NET 9.0 web application that allows users to explore a collection of movies and books. The application provides a user-friendly interface for browsing, searching, and managing movies and books through an interactive UI and API endpoints.

Features

View and manage movies and books.

Search for movies by title or director.

Browse featured movie and book collections.

Responsive UI built with Bootstrap 5.

ASP.NET Core MVC framework with Entity Framework Core for database management.

RESTful API for movies and books.

Technologies Used

.NET 9.0 – Backend framework

ASP.NET Core MVC – Web application structure

Entity Framework Core – ORM for database interaction

Bootstrap 5 – Frontend design

Microsoft SQL Server – Database

NUnit – Unit testing framework

Getting Started

Prerequisites

Ensure you have the following installed:

.NET 9.0 SDK

SQL Server (or any compatible database)

Installation

Clone the repository:

git clone https://github.com/LachezarDonchev/Movie-Book-Db-Project.git
cd ProjectMovieBookDB

Restore dependencies:

dotnet restore

Update the database:

dotnet ef database update

Run the application:

dotnet run

Open a browser and go to:

https://localhost:5001

API Endpoints

The application provides a RESTful API for managing movies:

Movies API

GET /api/movies – Retrieves all movies.

GET /api/movies/{id} – Retrieves a specific movie by ID.

POST /api/movies – Adds a new movie.

PUT /api/movies/{id} – Updates an existing movie.

DELETE /api/movies/{id} – Deletes a movie by ID.

Running Tests

Unit tests are written using NUnit.
To run tests:

 dotnet test

Folder Structure

ProjectMovieBookDB/
│-- Controllers/
|   |-- AuthorsController.cs
|   |-- BooksController.cs
|   |-- DirectorsController.cs
|   |-- GenresController.cs
│   │-- HomeController.cs
│   │-- MoviesController.cs
│-- Models/
│   │-- Author.cs
│   │-- Book.cs
│   │-- BookMovieCatalogContext.cs
│   │-- Director.cs
│   │-- ErrorViewModel.cs
│   │-- Genre.cs
│   │-- Movie.cs
│-- Views/
│   │-- Home/
│   │-- Shared/
│-- Tests/
│   │-- MoviesControllerTests.cs
│-- Program.cs
│-- README.md

Contributing

1. Fork the repository.

2. Create a new branch.

3. Commit changes.

4. Push to your branch and create a pull request.

License

This project is licensed under the MIT License.
