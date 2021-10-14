# control de personal
Sistema de control para ingreso de personal
### Test
En caso de requerir testear con la base de producción utilizar el siguiente codigo, el cual esta configurado en launchSettings.json
>  dotnet run --launch-profile productionTest
o en launch.json cambiar ASPNETCORE_ENVIRONMENT de Development a Production
> "ASPNETCORE_ENVIRONMENT": "Production"
### Migraciones
Usar el siguiente comando para generar las migraciones con mysql
> ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add 'migracion a mysql'
Usar el siguiente comando para actualizar la base de datos en producción
> dotnet ef database update -- --environment Production
> ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
## Scaffold 
### identity
Si se usa un usuario modificado se debe usar el parametro -dc para que lo detecte
> dotnet aspnet-codegenerator identity -dc control.personal.Data.ApplicationDbContext  --files "Account.Register" --force
### Vistas
> dotnet aspnet-codegenerator razorpage -dc control.personal.Data.ApplicationDbContext -m Identificacion -outDir Pages/Identificacion/ 
### Controlador
> dotnet aspnet-codegenerator controller -dc control.personal.Data.ApplicationDbContext -m Identificacion -api -outDir Controllers/ -name IdentificacionController
## Validaciones
Para cambiar los mensajes en las validaciones se cambiaron en jquery.validate.js linea 362 pero no funcionó ya que al parecer las validaciones se generan en el servidor 

----------
# Identificación
Para el registro de identificaciones se utilizan:
1.  un controlador de tipo api en la que se recibe una cadena con la uid, en caso de que dicha uid este asociada a un usuario se almacena un registro, caso contrario se genera un enlace que será enviado a un administrador para que se cree un objeto Identificacion con dicho uid y un usuario
2. una vista razor en la que el administrador pueda asociar un uid y un usuario
## API
