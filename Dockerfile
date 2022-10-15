FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# TODO: how to copy the .csproj files from the folders and keep the folder structure in the destination?
COPY ViennaRocks.sln .
COPY ViennaRocks.Api/*.csproj ViennaRocks.Api/
COPY ViennaRocks.Api.BusinessLogic/*.csproj ViennaRocks.Api.BusinessLogic/
COPY ViennaRocks.Api.BusinessLogic.Contract/*.csproj ViennaRocks.Api.BusinessLogic.Contract/
COPY ViennaRocks.Api.Models/*.csproj ViennaRocks.Api.Models/
RUN dotnet restore .

COPY . .
RUN dotnet publish -c Release --no-restore ViennaRocks.Api/ViennaRocks.Api.csproj -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS run
WORKDIR /app
COPY --from=build /app .

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "ViennaRocks.Api.dll"]
