#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["BackendService.csproj", ""]
RUN dotnet restore "./BackendService.csproj"
COPY . .
WORKDIR "/src/."
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh
RUN dotnet build "BackendService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD dotnet BackendService.dll