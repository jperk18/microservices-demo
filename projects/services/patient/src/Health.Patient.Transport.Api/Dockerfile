FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Health.Patient.Transport.Api/Health.Patient.Transport.Api.csproj", "Health.Patient.Transport.Api/"]
RUN dotnet restore "Health.Patient.Transport.Api/Health.Patient.Transport.Api.csproj"
COPY . .
WORKDIR "/src/Health.Patient.Transport.Api"
RUN dotnet build "Health.Patient.Transport.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Health.Patient.Transport.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Health.Patient.Transport.Api.dll"]
