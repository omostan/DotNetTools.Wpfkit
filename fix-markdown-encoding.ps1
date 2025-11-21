# Fix Markdown Files Encoding to UTF-8 with Emojis
# This script converts all markdown files to UTF-8 with BOM to ensure emojis display correctly

Write-Host "Starting markdown file encoding fix..." -ForegroundColor Cyan
Write-Host ""

$files = @(
    "README.md",
    "DotNetTools.WpfKit\README.md",
    "DotNetTools.WpfKit\ICON_GUIDE.md",
    "DotNetTools.WpfKit.Tests\README.md",
    "release-notes.md"
)

$successCount = 0
$errorCount = 0

foreach ($file in $files) {
    $fullPath = Join-Path $PSScriptRoot $file
    
    if (Test-Path $fullPath) {
        Write-Host "Processing: $file" -ForegroundColor White
        
        try {
            # Read the file content
            $content = Get-Content $fullPath -Raw -Encoding UTF8
            
            # Create a temporary file path
            $tempFile = "$fullPath.temp"
            
            # Write with UTF-8 BOM using Out-File (works in constrained mode)
            $content | Out-File -FilePath $tempFile -Encoding UTF8 -NoNewline
            
            # Replace original file
            Move-Item -Path $tempFile -Destination $fullPath -Force
            
            Write-Host "  ✓ Converted to UTF-8 with BOM" -ForegroundColor Green
            $successCount++
        }
        catch {
            Write-Host "  ✗ Error: $_" -ForegroundColor Red
            $errorCount++
        }
    }
    else {
        Write-Host "  ⚠ File not found: $file" -ForegroundColor Yellow
        $errorCount++
    }
    Write-Host ""
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Summary:" -ForegroundColor White
Write-Host "  ✓ Successfully converted: $successCount files" -ForegroundColor Green
Write-Host "  ✗ Errors/Not found: $errorCount files" -ForegroundColor $(if($errorCount -gt 0){'Red'}else{'Green'})
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Emojis should now display correctly in all processed markdown files!" -ForegroundColor Green
Write-Host "You can verify by opening any markdown file and checking for proper emoji display." -ForegroundColor White
