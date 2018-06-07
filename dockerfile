# STAGE - BUILD
FROM microsoft/dotnet:2.0-sdk as build
WORKDIR /docker
COPY ./src .
RUN dotnet build ServiceHub.User.sln
RUN dotnet publish ServiceHub.User.Service/ServiceHub.User.Service.csproj --output ../www

# STAGE - DEPLOY
FROM microsoft/aspnetcore:2.0 as deploy
WORKDIR /webapi
COPY --from=build /docker/www .
ENV ASPNETCORE_URLS=http://+:80/
EXPOSE 80
CMD [ "dotnet", "ServiceHub.User.Service.dll" ]
