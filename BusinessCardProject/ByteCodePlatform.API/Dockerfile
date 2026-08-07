FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY . .

ENV ASPNETCORE_URLS=https://+:5221
ENV ASPNETCORE_ENVIRONMENT=Prod
EXPOSE 5221

ENTRYPOINT ["dotnet", "ByteCodePlatform.API.dll"]
