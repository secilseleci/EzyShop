# 1. .NET SDK  
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Proje dosyalarý 
COPY *.sln ./
COPY WebUI/*.csproj ./WebUI/
COPY DataAccess/*.csproj ./DataAccess/
COPY Business/*.csproj ./Business/
COPY Core/*.csproj ./Core/
COPY Models/*.csproj ./Models/
RUN dotnet restore

# 3. Copy and build 
COPY . ./
WORKDIR /app/WebUI
RUN dotnet publish -c Release -o /out

# 4. Runtime imajý 
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out ./

# 5. Web sunucusu portu
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebUI.dll"]
