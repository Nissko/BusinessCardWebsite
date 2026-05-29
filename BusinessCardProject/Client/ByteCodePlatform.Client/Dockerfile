#FROM base AS build-client
#RUN dotnet publish "Client/ByteCodePlatform.Client/ByteCodePlatform.Client.csproj" -c Release -o /out/client
#
#FROM nginx:alpine AS blazor-client
#COPY --from=build-client /out/client/wwwroot /usr/share/nginx/html
#COPY scratch.conf /etc/nginx/conf.d/default.conf

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet workload install wasm-tools

WORKDIR /app
COPY . .

RUN dotnet publish -c Release -o /out \
    /p:TargetFramework=net8.0 \
    /p:RuntimeIdentifier=browser-wasm \
    /p:BlazorWebAssemblyLoadAllGlobalizationData=true \
    /p:RunAOTCompilation=false \
    /p:WasmBuildNative=false

FROM nginx:1.25-alpine

COPY --from=build /out/wwwroot /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]