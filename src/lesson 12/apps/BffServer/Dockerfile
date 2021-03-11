#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["lesson 12/apps/BffServer/BffServer.csproj", "lesson 12/apps/BffServer/"]
RUN dotnet restore "lesson 12/apps/BffServer/BffServer.csproj"
COPY . .
WORKDIR "/src/lesson 12/apps/BffServer"
RUN dotnet build "BffServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BffServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BffServer.dll"]