FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Epsilon.Host.WebApi/Epsilon.Host.WebApi.csproj", "Epsilon.Host.WebApi/"]
RUN dotnet restore "Epsilon.Host.WebApi/Epsilon.Host.WebApi.csproj"
COPY . .
WORKDIR "/src/Epsilon.Host.WebApi"

RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./dev-cert.pfx -p ob6KdSAOuNB3oe7
RUN dotnet dev-certs https --trust

RUN dotnet build "Epsilon.Host.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Epsilon.Host.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/Epsilon.Host.WebApi/dev-cert.pfx .
ENTRYPOINT ["dotnet", "Epsilon.Host.WebApi.dll"]
