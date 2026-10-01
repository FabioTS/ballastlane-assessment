# Task Management System

A small ASP.NET Core Web API for managing tasks with a simple SQLite database.

## Project purpose

This application manages tasks with the following fields:

- Title: required string
- Description: optional string
- Status: enum with ToDo, InProgress, Blocked, Done
- DueDate: optional DateTime
- UserId: Guid for the task owner/association

There is no User entity, authentication, authorization, or user-management system.

## Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + SQLite

## Run locally

```bash
dotnet restore
dotnet build
dotnet run
```

The app listens on the default ASP.NET Core local URL, and the SQLite database is created automatically as `tasks.db` in the project root.

## API endpoints

### GET /api/task
Returns all tasks.

### GET /api/task/{id}
Returns a single task by id.

### POST /api/task
Creates a new task.

### PUT /api/task/{id}
Updates an existing task.

### DELETE /api/task/{id}
Deletes a task by id.

## Example request body

```json
{
  "title": "Finish the project report",
  "description": "Review final notes and send the summary",
  "status": "InProgress",
  "dueDate": "2026-10-20T00:00:00",
  "userId": "22222222-2222-2222-2222-222222222222"
}
```

## Example curl requests

```bash
curl http://localhost:5000/api/task

curl -X POST http://localhost:5000/api/task \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Finish the project report",
    "description": "Review final notes and send the summary",
    "status": "InProgress",
    "dueDate": "2026-10-20T00:00:00",
    "userId": "22222222-2222-2222-2222-222222222222"
  }'
```
