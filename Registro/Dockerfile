#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Registro/Registro.csproj", "Registro/"]
RUN dotnet restore "Registro/Registro.csproj"
COPY . .
WORKDIR "/src/Registro"
RUN dotnet build "Registro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Registro.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Registro.dll"]
