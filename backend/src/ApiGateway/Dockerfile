# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy the solution file and restore dependencies
COPY backend/TikTokClone.sln .
COPY backend/src/UserService/UserService.csproj ./backend/src/UserService/
COPY backend/src/ApiGateway/ApiGateway.csproj ./backend/src/ApiGateway/
COPY backend/src/InteractionService/InteractionService.csproj ./backend/src/InteractionService/
COPY backend/src/NotificationService/NotificationService.csproj ./backend/src/NotificationService/
COPY backend/src/VideoService/VideoService.csproj ./backend/src/VideoService/

# Restore dependencies for all projects
RUN dotnet restore TikTokClone.sln

# Copy the rest of the source code
COPY backend/src/ApiGateway ./backend/src/ApiGateway

# Build and publish the ApiGateway
WORKDIR /source/backend/src/ApiGateway
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Create a non-root user
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
