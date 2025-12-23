@echo off
echo Iniciando el frontend...
cd .\frontend\taskmanager-client
start ng serve

timeout /t 5 /nobreak

echo Iniciando el backend...
cd ..\..\backend\src\TaskManager.Api
start dotnet run
