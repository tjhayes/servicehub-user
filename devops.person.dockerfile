# build Housing.Foundation.Person.Service
FROM microsoft/aspnetcore-build:2.0 as build
WORKDIR /build

# restore packages before copying entire source - provides optimizations when rebuilding
COPY *.sln ./
COPY Housing.Foundation.Apartment.Context/*.csproj Housing.Foundation.Apartment.Context/
COPY Housing.Foundation.Apartment.Service/*.csproj Housing.Foundation.Apartment.Service/
COPY Housing.Foundation.Batch.Context/*.csproj Housing.Foundation.Batch.Context/
COPY Housing.Foundation.Batch.Service/*.csproj Housing.Foundation.Batch.Service/
COPY Housing.Foundation.Person.Context/*.csproj Housing.Foundation.Person.Context/
COPY Housing.Foundation.Person.Service/*.csproj Housing.Foundation.Person.Service/
COPY Housing.Foundation.Library/*.csproj Housing.Foundation.Library/
COPY Housing.Foundation.TestSuite/*.csproj Housing.Foundation.TestSuite/
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet build *.Person.Service --no-restore
RUN dotnet test *.TestSuite --no-restore
RUN dotnet publish *.Person.Service -c Release -o ../Package --no-restore

# build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build /build/Package ./
ENTRYPOINT [ "dotnet", "Housing.Foundation.Person.Service.dll" ]
