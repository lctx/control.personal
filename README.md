# control de personal
Sistema de control para ingreso de personal haciendo uso de tarjetas RFID
### Test
En caso de errores generar los archivos appsettings.Production.json y/o appsettings.Develoment.json con los mismos campos que appsettings.json
En caso de requerir testear con la base de producción utilizar el siguiente codigo, el cual esta configurado en launchSettings.json
>  dotnet run --launch-profile productionTest 
----------
o en launch.json cambiar ASPNETCORE_ENVIRONMENT de Development a Production
> "ASPNETCORE_ENVIRONMENT": "Production"
### Migraciones
Usar el siguiente comando para generar las migraciones con mysql
> ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add 'migracion a mysql'
----------
Usar el siguiente comando para actualizar la base de datos en producción
> dotnet ef database update -- --environment Production
----------
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
3. en la vista razor para modificar dicha identificación se puede, y debería, escribir un json con los datos personales del usuario que tiene asignada dicha identificación
## API
Consta de un método post
- Uso de jwt para seguridad, solo debe poder usar el método un usuario con rol 'Sensor'
- En el body de la solicitud debe enviar un string
- Validar la longitud de la cadena
### Logica
1. Busca el usuario que tenga asociada una identificacion con el uid enviado
    1. Si no encuentra un usuario con el uid enviado, envía via telegram un enlace en el que conste el uid recibido para que un administrador los asocie
    2. Si encuentra verifica si la identificación está activa
        1. Si está activa se guarda un registro con la fecha/hora del servidor y devuelve un "bienvenido + usuario"
        2. Si no está activa se envía por telegram un aviso del uso de una tarjeta desactivada y devuelve un "Error tarjeta inactiva"

## Vista 
Consta de un formulario para la asociacion de un uid a un usuario existente
- Solo debe ser accesible para administradores
- El campo uid debe ser capturado por un get
- Antes de cargar el formulario verifica si existe dicho uid en la base de datos
- El input de uid debe estar desactivado para solo capturar datos del request y evitar fallos por parte del usuario
### Logica
1. El modelo de vista consta de un método en el que recibe por parámetro el uid
2. Con el uid del parametro se rellena el campo UID del formulario
3. El usuario elige a que Usuario va a asociar dicho uid
4. Se muestra un modal de confirmación y se guarda un objeto de tipo Identificación
### Vistas auxiliares
- Listado de Uids y Usuarios o en su defecto una lista de todos los usuarios y el numero de identificaciones que tiene asociadas 
- Página para inactivar las identificaciones 
    - debería saber cual es y a que usuario afecta o podría generar un estado en el cual elijo una opcion inactivar y la siguiente identificación que ingresa a la api es inactivada o se envía un enlace via telegram a un administrador para que este desactive manualmente la identificación, pero esta logica implicaria que el administrador supiera a que hora va a ser leida dicha identificación y en caso de perdida se haría imposible el desactivarla
    - tambien se puede hacer una pagina en la que cada usuario pueda desactivar sus identificaciones 
    - crear un método que sea accesible por administración (debería ser accedida por una aplicación movil) y que reciba el uid para desactivarlo al leer la identificación
        - puede usar esto https://googlechrome.github.io/samples/web-nfc/
# Mejoras
## Refactorizaciones
### Javascript
- El método Scan usado para leer las etiquetas nfc se repite por lo que debe ser mejorado