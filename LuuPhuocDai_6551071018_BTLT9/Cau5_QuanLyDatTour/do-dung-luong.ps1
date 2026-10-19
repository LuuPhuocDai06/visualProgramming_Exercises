# Đo dung lượng và số file của 2 bản publish để điền vào bảng so sánh trong README
foreach ($d in 'publish_fdd','publish_sc') {
    if (Test-Path $d) {
        $f = Get-ChildItem $d -Recurse -File
        "{0,-12} {1,5} file  {2,8:N1} MB" -f $d, $f.Count, (($f | Measure-Object Length -Sum).Sum / 1MB)
    }
}
