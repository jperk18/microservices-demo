FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Health.Workflow/Health.Workflow.csproj", "Health.Workflow/"]
RUN dotnet restore "Health.Workflow/Health.Workflow.csproj"
COPY . .
WORKDIR "/src/Health.Workflow"
RUN dotnet build "Health.Workflow.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Health.Workflow.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Health.Workflow.dll"]
