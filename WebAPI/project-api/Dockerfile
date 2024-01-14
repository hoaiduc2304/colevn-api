#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y libfreetype6
RUN apt-get install -y libfontconfig1




FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["03-Web/project-api/project-api.csproj", "03-Web/project-api/"]
COPY ["01-Module/04-Project/PM.API/PM.API.csproj", "01-Module/04-Project/PM.API/"]
COPY ["02-Core/DNH.API/DNH.API.csproj", "02-Core/DNH.API/"]
COPY ["02-Core/DNH.Cache/DNH.Cache.csproj", "02-Core/DNH.Cache/"]
COPY ["02-Core/DNH.Core/DNHCore.csproj", "02-Core/DNH.Core/"]
COPY ["02-Core/DNH.Infrastructure.Database/DNH.Infrastructure.Database.csproj", "02-Core/DNH.Infrastructure.Database/"]
COPY ["02-Core/DNH.Infrastructure.Utility/DNH.Infrastructure.Utility.csproj", "02-Core/DNH.Infrastructure.Utility/"]
COPY ["01-Module/04-Project/PM.BusinessServices/PM.BusinessServices.csproj", "01-Module/04-Project/PM.BusinessServices/"]
COPY ["01-Module/04-Project/PM.Contact/PM.Contact.csproj", "01-Module/04-Project/PM.Contact/"]
RUN dotnet restore "03-Web/project-api/project-api.csproj"
COPY . .
WORKDIR "/src/03-Web/project-api"
RUN dotnet build "project-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "project-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "project-api.dll"]