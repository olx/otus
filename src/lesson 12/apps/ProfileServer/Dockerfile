#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["lesson 12/apps/ProfileServer/ProfileServer.csproj", "lesson 12/apps/ProfileServer/"]
COPY ["lesson 12/apps/JetExtension/JetExtension.csproj", "lesson 12/apps/JetExtension/"]
COPY ["lesson 12/apps/DatabaseContext/DatabaseContext.csproj", "lesson 12/apps/DatabaseContext/"]
RUN dotnet restore "lesson 12/apps/ProfileServer/ProfileServer.csproj"
COPY . .
WORKDIR "/src/lesson 12/apps/ProfileServer"
RUN dotnet build "ProfileServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProfileServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfileServer.dll"]