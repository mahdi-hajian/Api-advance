FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["MyApi/MyApi.csproj", "MyApi/"]
COPY ["WebFramework/WebFramework.csproj", "WebFramework/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "MyApi/MyApi.csproj"
COPY . .
WORKDIR "/src/MyApi"
RUN dotnet build "MyApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyApi.dll"]