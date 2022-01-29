FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CloudBruh.Trustartup.MediaSystem/CloudBruh.Trustartup.MediaSystem.csproj", "CloudBruh.Trustartup.MediaSystem/"]
RUN dotnet restore "CloudBruh.Trustartup.MediaSystem/CloudBruh.Trustartup.MediaSystem.csproj"
COPY . .
WORKDIR "/src/CloudBruh.Trustartup.MediaSystem"
RUN dotnet build "CloudBruh.Trustartup.MediaSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudBruh.Trustartup.MediaSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudBruh.Trustartup.MediaSystem.dll"]
