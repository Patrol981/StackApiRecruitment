FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY StackAPI/. ./StackAPI/.
COPY StackAPI.Models/. ./StackAPI.Models/.
COPY StackAPI.Utils/. ./StackAPI.Utils/.
COPY StackAPI.Tests.Unit/. ./StackAPI.Tests.Unit/.

WORKDIR /app/StackAPI
RUN dotnet restore
RUN dotnet build

WORKDIR /app/StackAPI.Tests.Unit
RUN dotnet test

WORKDIR /app/StackAPI
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/StackAPI/out .
EXPOSE 7031
EXPOSE 5141
ENTRYPOINT [ "dotnet", "StackAPI.dll"]