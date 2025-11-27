# 1. Dùng ảnh SDK để Build code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file csproj và restore thư viện (cho cả 3 project)
COPY ["BakeryShopAPI/BakeryShopAPI.csproj", "BakeryShopAPI/"]
COPY ["BakeryShopAPI.Data/BakeryShopAPI.Data.csproj", "BakeryShopAPI.Data/"]
COPY ["BakeryShopAPI.Services/BakeryShopAPI.Services.csproj", "BakeryShopAPI.Services/"]
RUN dotnet restore "BakeryShopAPI/BakeryShopAPI.csproj"

# Copy toàn bộ code còn lại
COPY . .
WORKDIR "/src/BakeryShopAPI"
RUN dotnet build "BakeryShopAPI.csproj" -c Release -o /app/build

# Publish ra file DLL
FROM build AS publish
RUN dotnet publish "BakeryShopAPI.csproj" -c Release -o /app/publish

# 2. Dùng ảnh Runtime nhẹ để chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BakeryShopAPI.dll"]