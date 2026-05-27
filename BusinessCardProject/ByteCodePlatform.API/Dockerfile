FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY . .

ENV ASPNETCORE_URLS=http://+:5221
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5221

ENTRYPOINT ["dotnet", "SecureChatUserMicroService.API.dll"]
