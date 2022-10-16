FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# unfortunately, it not possible to copy the .csproj files from subdirectories and keep the folder structure in the image
# see: https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
# https://github.com/moby/buildkit/issues/1900
# https://stackoverflow.com/questions/45786035/docker-copy-with-folder-wildcards/58697043#58697043

COPY ViennaRocks.sln .
COPY **/*.csproj ./
SHELL [ "pwsh", "-command", "$ErrorActionPreference = 'Stop';"]
RUN Get-ChildItem -Filter *.csproj | ForEach-Object { $_.Name.Replace(".csproj", "") } | ForEach-Object { New-Item -ItemType Directory $_ } | Out-Null
RUN Get-ChildItem -Filter *.csproj | ForEach-Object { Move-Item -Path $_ -Destination $_.Name.Replace(".csproj", "") }

RUN dotnet restore .

COPY . .
RUN dotnet publish -c Release --no-restore ViennaRocks.Api/ViennaRocks.Api.csproj -o /build

FROM runtime as final
WORKDIR /app
COPY --from=build /build .
ENTRYPOINT ["dotnet", "ViennaRocks.Api.dll"]
