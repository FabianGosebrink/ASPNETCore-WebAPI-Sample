# ASP .Net runtime for base image 
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80 

# Building app 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./SampleWebApiAspNetCore/SampleWebApiAspNetCore.csproj .
RUN dotnet restore SampleWebApiAspNetCore.csproj
COPY ./SampleWebApiAspNetCore/ .
RUN dotnet build "SampleWebApiAspNetCore.csproj" -c Release -o /app/build

# publishing 
FROM build AS publish
RUN dotnet publish "SampleWebApiAspNetCore.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleWebApiAspNetCore.dll"]
