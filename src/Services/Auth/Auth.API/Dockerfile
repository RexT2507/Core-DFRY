#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /core-dfry
COPY "Core-DFRY.sln" "Core-DFRY.sln"
WORKDIR /core-dfry/src
COPY "src/Core/ApiLibrary" "Core/ApiLibrary/"
COPY "src/Services/Auth/Auth.API" "Services/Auth/Auth.API/"
RUN dotnet restore "Services/Auth/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/core-dfry/src/Services/Auth/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]