# ThingsToDo
# My ASP.NET Core API

This project is an ASP.NET Core Web API that provides endpoints for managing ToDo tasks.

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Endpoints](#endpoints)
- [Contributing](#contributing)
- [License](#license)

## Introduction
This API allows users to create, read, update, and delete (CRUD) ToDo tasks. 
It also supports filtering tasks by timestamps and managing task status (start and stop).

## Features
- Retrieve ToDo tasks
	* all of them
	* by Id
	* filtered by its creational date
	* per page (pagination)
- Create new ToDo tasks
	* create by adding a new item with editing descripition and estimated duration
	* create by press START - default message is random Chuck Norris joke. Zen quotes were not free
- Update existing ToDo tasks
	* update by editing editing descripition and estimated duration
	* update by press START/STOP - start stop fuctions are validated
- Delete ToDo tasks

## Prerequisites
- [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or any other compatible IDE
- [Git](https://git-scm.com/)

## Installation
### Clone the Repository
git clone https://github.com/vargasrac/ThingsToDo.git
cd your-repository
### Restore Dependencies
sh
dotnet restore
### Running the Application
Using the .NET CLI
sh
dotnet build
dotnet run
### Using Visual Studio
Open the solution file (.sln) in Visual Studio.
Set the Startup Project to the API project.
Press F5 to build and run the application.

## Endpoints
GET /api/ToDoTasks - Retrieve all tasks

GET /api/ToDoTasks/{id} - Retrieve a specific task by ID

GET /api/ToDoTasks/filter?from={from}&to={to} - Filter tasks by timestamps

GET: api/ToDoTasks/page?pageNumber=3&pageSize=4

POST /api/ToDoTasks/add - Add a new task

PUT /api/ToDoTasks/update/{id} - Update an existing task by ID

PUT /api/ToDoTasks/update/start/{id} - Start a task

PUT /api/ToDoTasks/update/stop/{id} - Stop a task

DELETE /api/ToDoTasks/{id} - Delete a task by ID

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License - see the LICENSE file for details.
