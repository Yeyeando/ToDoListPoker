@echo off
echo Iniciando el frontend...
cd C:\proyectos\to-do-list\frontend\taskmanager-client
start ng serve

timeout /t 5 /nobreak

echo Iniciando el backend...
cd C:\proyectos\to-do-list\backend\src\TaskManager.Api
start dotnet run
