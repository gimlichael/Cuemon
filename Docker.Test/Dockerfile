FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["src/Docker.Test/Cuemon.Docker.Test.csproj", "src/Docker.Test/"]
RUN dotnet restore "src/Docker.Test/Cuemon.Docker.Test.csproj"
COPY . .
WORKDIR "/src/src/Docker.Test"
RUN dotnet build "Cuemon.Docker.Test.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Cuemon.Docker.Test.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Cuemon.Docker.Test.dll"]