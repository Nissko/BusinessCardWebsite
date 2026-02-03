FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /src
COPY . .

FROM base AS build-api
RUN dotnet restore "BusinessCardProject.sln"
RUN dotnet publish "BusinessCardProject.Server.Core.Api/BusinessCardProject.Server.Core.Api.csproj" -c Release -o /out/api

FROM base AS build-client
RUN dotnet publish "Client/BusinessCardProject.Client/BusinessCardProject.Client.csproj" -c Release -o /out/client

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
COPY --from=build-api /out/api .
ENTRYPOINT ["dotnet", "BusinessCardProject.Server.Core.Api.dll"]

FROM nginx:alpine AS frontend
COPY --from=build-client /out/client/wwwroot /usr/share/nginx/html
COPY scratch.conf /etc/nginx/conf.d/default.conf