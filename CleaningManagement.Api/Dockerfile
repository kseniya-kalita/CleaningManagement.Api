FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["CleaningManagementApi.Api/CleaningManagementApi.Api.csproj", "CleaningManagement.Api/"]
RUN dotnet restore "CleaningManagementApi.Api/CleaningManagementApi.Api.csproj"
COPY . .
WORKDIR "/src/CleaningManagementApi.Api"
RUN dotnet build "CleaningManagement.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CleaningManagement.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CleaningManagement.Api.dll"]