FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

EXPOSE 8080
EXPOSE 5046

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PrasenssaAPI/PrasenssaAPI.csproj", "PrasenssaAPI/"]
RUN dotnet restore "PrasenssaAPI/PrasenssaAPI.csproj"
COPY . .
WORKDIR "/src/PrasenssaAPI"
RUN dotnet build "PrasenssaAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PrasenssaAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV MOCK_CLIENT ${MOCK_CLIENT}
ENTRYPOINT ["dotnet", "PrasenssaAPI.dll","--MockPraseansaClient ${MOCK_CLIENT}"]
