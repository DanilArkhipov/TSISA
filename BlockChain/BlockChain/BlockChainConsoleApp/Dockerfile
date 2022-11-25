FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BlockChainConsoleApp/BlockChainConsoleApp.csproj", "BlockChainConsoleApp/"]
RUN dotnet restore "BlockChainConsoleApp/BlockChainConsoleApp.csproj"
COPY . .
WORKDIR "/src/BlockChainConsoleApp"
RUN dotnet build "BlockChainConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlockChainConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlockChainConsoleApp.dll"]
