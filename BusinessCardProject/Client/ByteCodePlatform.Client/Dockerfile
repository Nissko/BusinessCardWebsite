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