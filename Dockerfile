FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 7070

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GithubAPICSharp.csproj", "./"]
RUN dotnet restore "GithubAPICSharp.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./GithubAPICSharp.csproj" -o /app/build

FROM build AS publish
RUN dotnet publish "./GithubAPICSharp.csproj" -o /app/publish 

FROM base AS final
WORKDIR /app
ARG ASPNETCORE_ENVIRONMENT="Development"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "./GithubAPICSharp.dll"]
