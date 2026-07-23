FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY . .

ENV ASPNETCORE_URLS=http://+:5223
ENV ASPNETCORE_ENVIRONMENT=Prod
EXPOSE 5223

ENTRYPOINT ["dotnet", "Services.MailService.Presentation.dll"]
