#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN  sed -i s@/deb.debian.org/@/mirrors.huaweicloud.com/@g /etc/apt/sources.list
RUN  apt-get clean
RUN  apt-get update
RUN apt-get update && apt-get install -y \
    libopenblas-dev \
    liblapack-dev \
    libx11-6 \
	libgdiplus \
    && apt-get clean && rm -rf /var/lib/apt/lists/*


WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HackathonDemo.API/HackathonDemo.API.csproj", "HackathonDemo.API/"]
RUN dotnet restore "HackathonDemo.API/HackathonDemo.API.csproj"
COPY . .
WORKDIR "/src/HackathonDemo.API"
RUN dotnet build "HackathonDemo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackathonDemo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackathonDemo.API.dll"]