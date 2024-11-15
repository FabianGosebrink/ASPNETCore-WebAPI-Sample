# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SampleWebApiAspNetCore/SampleWebApiAspNetCore.csproj", "SampleWebApiAspNetCore/"]
RUN dotnet restore "SampleWebApiAspNetCore/SampleWebApiAspNetCore.csproj"
COPY . .
WORKDIR "/src/SampleWebApiAspNetCore"
RUN dotnet build "SampleWebApiAspNetCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleWebApiAspNetCore.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleWebApiAspNetCore.dll"]