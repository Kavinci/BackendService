#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apk add bash sqlite 

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["BackendService.csproj", ""]
RUN dotnet restore "./BackendService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BackendService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet ef database-update
ENTRYPOINT ["dotnet", "BackendService.dll"]