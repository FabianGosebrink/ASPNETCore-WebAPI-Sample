# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the source files
COPY . .

# Navigate to the project directory and restore dependencies
WORKDIR /src/exam/app./SampleWebApiAspNetCore
RUN dotnet restore ./SampleWebApiAspNetCore.csproj

# Build the application
RUN dotnet publish ./SampleWebApiAspNetCore.csproj -c Release -o /app/publish

# Final stage: copy the build to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SampleWebApiAspNetCore.dll"]
