FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY . .

ENV ASPNETCORE_URLS=http://+:5036
ENV ASPNETCORE_ENVIRONMENT=Prod
EXPOSE 5036

ENTRYPOINT ["dotnet", "Services.FileService.Presentation.dll"]