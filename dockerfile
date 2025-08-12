
FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY . .
RUN dotnet restore && dotnet publish -c Release -o out
WORKDIR /app/out
EXPOSE 5000
ENTRYPOINT ["dotnet", "ASPNETCore-WebAPI-Sample.dll"]
