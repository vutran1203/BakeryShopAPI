# 1. Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# --- SỬA ĐOẠN NÀY CHO KHỚP VỚI TÊN THƯ MỤC TRÊN Ổ CỨNG ---

# Folder: BakeryShop -> File Project: BakeryShopAPI.csproj
COPY ["BakeryShop/BakeryShopAPI.csproj", "BakeryShop/"]

# Folder: BakeryAPI.Data -> File Project: BakeryShopAPI.Data.csproj
COPY ["BakeryAPI.Data/BakeryShopAPI.Data.csproj", "BakeryAPI.Data/"]

# Folder: BakeryAPI.Services -> File Project: BakeryShopAPI.Services.csproj
COPY ["BakeryAPI.Services/BakeryShopAPI.Services.csproj", "BakeryAPI.Services/"]

# ---------------------------------------------------------

# Restore dựa trên đường dẫn mới
RUN dotnet restore "BakeryShop/BakeryShopAPI.csproj"

COPY . .

# Chuyển vào thư mục BakeryShop (nơi chứa API) để build
WORKDIR "/src/BakeryShop"
RUN dotnet build "BakeryShopAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BakeryShopAPI.csproj" -c Release -o /app/publish

# 2. Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BakeryShopAPI.dll"]