# stop-all.ps1
Write-Host "Stopping all services..." -ForegroundColor Yellow
docker compose down
Write-Host "All services stopped. Data is preserved!" -ForegroundColor Green