FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Eightnashki/Eightnashki.csproj", "Eightnashki/"]
RUN dotnet restore "Eightnashki/Eightnashki.csproj"
COPY . .
WORKDIR "/src/Eightnashki"
RUN dotnet build "Eightnashki.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Eightnashki.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eightnashki.dll"]
