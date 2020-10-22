 # Docker image for running pricing logic, probably installs way too much stuff
# Run `docker build -t samplewebapiaspnetcore .`
# Then `docker run -it --rm -p 5000:80 --name aspnetcore_sample samplewebapiaspnetcore`
# http://localhost:5000/swagger/index.html
# The `docker logs -f <container id>`
# Optional: `docker attach <container id>`

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "SampleWebApiAspNetCore.dll”]