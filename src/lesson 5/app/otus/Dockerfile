#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV OTUS_PORT=8000
EXPOSE ${OTUS_PORT}

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["lesson 5/app/otus/otus.csproj", "lesson 5/app/otus/"]
RUN dotnet restore "lesson 5/app/otus/otus.csproj"
COPY . .
WORKDIR "/src/lesson 5/app/otus"
RUN dotnet build "otus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "otus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "otus.dll"]