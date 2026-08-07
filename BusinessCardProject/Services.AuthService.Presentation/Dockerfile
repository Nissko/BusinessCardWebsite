FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY . .

ENV ASPNETCORE_URLS=https://+:5012
ENV ASPNETCORE_ENVIRONMENT=Prod
EXPOSE 5012

ENTRYPOINT ["dotnet", "Services.AuthService.Presentation.dll"]
