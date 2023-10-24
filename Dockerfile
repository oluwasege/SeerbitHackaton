#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SeerbitHackaton.API/SeerbitHackaton.API.csproj", "SeerbitHackaton.API/"]
RUN dotnet restore "SeerbitHackaton.API/SeerbitHackaton.API.csproj"
COPY . .
WORKDIR "/src/SeerbitHackaton.API"
RUN dotnet build "SeerbitHackaton.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeerbitHackaton.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeerbitHackaton.API.dll"]