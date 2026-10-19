# Bản 2: Self-Contained + 1 file duy nhất (máy đích KHÔNG cần cài .NET)
dotnet publish -c Release -r win-x64 --self-contained true `
    -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true `
    -o publish_sc
# appsettings.json được copy ra publish_sc\ cạnh QuanLyDatTour.exe (không bị gói vào exe) -> sửa connection string được
