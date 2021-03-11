#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["lesson 12/apps/IdentityServer/IdentityServer.csproj", "lesson 12/apps/IdentityServer/"]
COPY ["lesson 12/apps/JetExtension/JetExtension.csproj", "lesson 12/apps/JetExtension/"]
COPY ["lesson 12/apps/DatabaseContext/DatabaseContext.csproj", "lesson 12/apps/DatabaseContext/"]
RUN dotnet restore "lesson 12/apps/IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/lesson 12/apps/IdentityServer"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]