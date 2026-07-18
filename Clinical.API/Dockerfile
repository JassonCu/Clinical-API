FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5123

ENV ASPNETCORE_URLS=http://+:5123

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Clinical.API/Clinical.API.csproj", "Clinical.API/"]
COPY ["Clinical.Application.DTOS/Clinical.Application.DTOS.csproj", "Clinical.Application.DTOS/"]
COPY ["Clinical.Domain/Clinical.Domain.csproj", "Clinical.Domain/"]
COPY ["Clinical.Infraestructure/Clinical.Infraestructure.csproj", "Clinical.Infraestructure/"]
COPY ["Clinical.Interface/Clinical.Interface.csproj", "Clinical.Interface/"]
COPY ["Clinical.Persistence/Clinical.Persistence.csproj", "Clinical.Persistence/"]
COPY ["Clinical.UseCases/Clinical.UseCases.csproj", "Clinical.UseCases/"]
COPY ["Clinical.Utils/Clinical.Utils.csproj", "Clinical.Utils/"]
RUN dotnet restore "Clinical.API/Clinical.API.csproj"
COPY . .
WORKDIR "/src/Clinical.API"
RUN dotnet build "Clinical.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Clinical.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clinical.API.dll"]
