FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
# Publish app
ARG release_type=Release
COPY . /opt/app/build
WORKDIR /opt/app/build
RUN dotnet publish -c ${release_type} -o published/

# Build run image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
COPY --from=build /opt/app/build/published /opt/app
WORKDIR /opt/app
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "FlatRent.dll"]