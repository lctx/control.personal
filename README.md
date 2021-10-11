# control de personal
Sistema de control para ingreso de personal
### Test
En caso de requerir testear con la base de producción utilizar el siguiente codigo, el cual esta configurado en launchSettings.json
>  dotnet run --launch-profile productionTest
o en launch.json cambiar ASPNETCORE_ENVIRONMENT de Development a Production
> "ASPNETCORE_ENVIRONMENT": "Production"
### Migraciones
Usar el siguiente comando para actualizar la base de datos en producción
> dotnet ef database update --environment Production
