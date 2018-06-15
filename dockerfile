# STAGE - BUILD
FROM microsoft/aspnetcore-build:2.0 as build
WORKDIR /docker
COPY src/ServiceHub.User.Context/*.csproj ServiceHub.User.Context/
COPY src/ServiceHub.User.Library/*.csproj ServiceHub.User.Library/
COPY src/ServiceHub.User.Service/*.csproj ServiceHub.User.Service/
RUN dotnet restore *.Service
COPY src ./
RUN dotnet publish *.Service --no-restore -o ../www

# STAGE - DEPLOY
FROM microsoft/aspnetcore:2.0 as deploy
WORKDIR /webapi
COPY --from=build /docker/www ./
ENV ASPNETCORE_URLS=http://+:80/
EXPOSE 80
CMD [ "dotnet", "ServiceHub.User.Service.dll" ]
