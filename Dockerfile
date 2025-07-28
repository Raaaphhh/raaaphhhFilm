# Étape 1 : build avec le SDK .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie tout dans le conteneur et publie
COPY . ./
RUN dotnet publish -c Release -o out

# Étape 2 : image runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# Démarrage de l'app
ENTRYPOINT ["dotnet", "raaaphhhFilm.dll"]
