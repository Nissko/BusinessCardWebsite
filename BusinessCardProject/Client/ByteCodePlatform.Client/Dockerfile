FROM base AS build-client
RUN dotnet publish "Client/ByteCodePlatform.Client/ByteCodePlatform.Client.csproj" -c Release -o /out/client

FROM nginx:alpine AS blazor-client
COPY --from=build-client /out/client/wwwroot /usr/share/nginx/html
COPY scratch.conf /etc/nginx/conf.d/default.conf