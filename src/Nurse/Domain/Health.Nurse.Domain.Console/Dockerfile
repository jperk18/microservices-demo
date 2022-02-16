FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Health.Patient.Domain/Health.Patient.Domain.csproj", "Health.Patient.Domain/"]
RUN dotnet restore "Health.Patient.Domain/Health.Patient.Domain.csproj"
COPY . .
WORKDIR "/src/Health.Patient.Domain"
RUN dotnet build "Health.Patient.Domain.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Health.Patient.Domain.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Health.Patient.Domain.dll"]
