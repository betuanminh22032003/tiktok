# TikTok Clone Microservices

This directory contains the backend microservices for the TikTok Clone application.

## Services

- **UserService**: Manages user accounts, authentication, and profiles.
- **VideoService**: Handles video uploads, processing, and streaming.
- **InteractionService**: Manages user interactions like likes and comments.
- **NotificationService**: Provides real-time notifications using SignalR.

## Getting Started

Each service is a separate .NET Core Web API project. To run a service, navigate to its directory and use the `dotnet run` command.

Example:
```bash
cd src/UserService
dotnet run
```
