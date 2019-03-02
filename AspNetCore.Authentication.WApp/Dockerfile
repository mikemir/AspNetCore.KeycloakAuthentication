FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AspNetCore.Authentication.WApp/AspNetCore.Authentication.WApp.csproj AspNetCore.Authentication.WApp/
COPY AspNetCore.KeycloakAuthentication/AspNetCore.KeycloakAuthentication.csproj AspNetCore.KeycloakAuthentication/
RUN dotnet restore AspNetCore.Authentication.WApp/AspNetCore.Authentication.WApp.csproj
COPY . .
WORKDIR /src/AspNetCore.Authentication.WApp
RUN dotnet build AspNetCore.Authentication.WApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AspNetCore.Authentication.WApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCore.Authentication.WApp.dll"]
