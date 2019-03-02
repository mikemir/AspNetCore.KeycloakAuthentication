FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AspNetCore.Authentication.WebApiThree/AspNetCore.Authentication.WebApiThree.csproj AspNetCore.Authentication.WebApiThree/
COPY AspNetCore.KeycloakAuthentication/AspNetCore.KeycloakAuthentication.csproj AspNetCore.KeycloakAuthentication/
RUN dotnet restore AspNetCore.Authentication.WebApiThree/AspNetCore.Authentication.WebApiThree.csproj
COPY . .
WORKDIR /src/AspNetCore.Authentication.WebApiThree
RUN dotnet build AspNetCore.Authentication.WebApiThree.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AspNetCore.Authentication.WebApiThree.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCore.Authentication.WebApiThree.dll"]
