FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MockGcc.Service/MockGcc.Service.csproj", "MockGcc.Service/."]
COPY ["MockGcc.UI/MockGcc.UI.csproj", "MockGcc.UI/."]
RUN dotnet restore "MockGcc.Service/MockGcc.Service.csproj"
COPY . .
RUN dotnet build "MockGcc.Service/MockGcc.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockGcc.Service/MockGcc.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockGcc.Service.dll"]