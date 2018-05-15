FROM microsoft/dotnet:2.0-sdk as build
WORKDIR /docker
COPY ./src .
RUN dotnet build ServiceHub.Person.sln
RUN dotnet publish ServiceHub.Person.Service/ServiceHub.Person.Service.csproj --output ../www

FROM microsoft/aspnetcore:2.0 as deploy
WORKDIR /webapi
COPY --from=build /docker/www .
ENV ASPNETCORE_URLS=http://+:80/
ENV ASPNETCORE_ENVIRONMENT=Staging
EXPOSE 80
CMD [ "dotnet", "ServiceHub.Person.Service.dll" ]
