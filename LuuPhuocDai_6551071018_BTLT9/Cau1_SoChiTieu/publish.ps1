# Framework-Dependent, win-x64  ->  thư mục publish\
dotnet publish -c Release -r win-x64 --self-contained false -o publish
Write-Host "Xong. Chép thư mục 'publish' sang máy khác (đã cài .NET 8 Desktop Runtime), sửa appsettings.json rồi chạy SoChiTieu.exe"
