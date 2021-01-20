#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["lesson 6/app/otus6/otus6.csproj", "lesson 6/app/otus6/"]
RUN dotnet restore "lesson 6/app/otus6/otus6.csproj"
COPY . .
WORKDIR "/src/lesson 6/app/otus6"
RUN dotnet build "otus6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "otus6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "otus6.dll"]