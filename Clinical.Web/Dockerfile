FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5124

ENV ASPNETCORE_URLS=http://+:5124

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Clinical.Web/Clinical.Web.csproj", "Clinical.Web/"]
RUN dotnet restore "Clinical.Web/Clinical.Web.csproj"
COPY . .
WORKDIR "/src/Clinical.Web"
RUN dotnet build "Clinical.Web.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Clinical.Web.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clinical.Web.dll"]
