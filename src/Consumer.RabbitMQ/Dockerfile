# Dockerfile para Consumer.RabbitMQ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Consumer.RabbitMQ/Consumer.RabbitMQ.csproj", "Consumer.RabbitMQ/"]
RUN dotnet restore "Consumer.RabbitMQ/Consumer.RabbitMQ.csproj"
COPY . .
WORKDIR "/src/Consumer.RabbitMQ"
RUN dotnet build "Consumer.RabbitMQ.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer.RabbitMQ.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.RabbitMQ.dll"]
