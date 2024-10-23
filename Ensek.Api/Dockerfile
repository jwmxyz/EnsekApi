FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
# Expose the standard HTTP and HTTPS ports
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ensek.Api/Ensek.Api.csproj", "Ensek.Api/"]
RUN dotnet restore "Ensek.Api/Ensek.Api.csproj"
COPY . .
WORKDIR "/src/Ensek.Api"
RUN dotnet build "Ensek.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ensek.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the ASP.NET Core URL to use HTTPS
ENV ASPNETCORE_URLS=https://+:443;http://+:8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Ensek.Api.dll"]