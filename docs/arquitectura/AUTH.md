# Autenticación y Autorización

La información de un usuario autenticado se representa mediante tres valores: id de usuario, id de sesión y hogar activo. La clase AuthUserDetails encapsula esta información.

Cuando el usuario se registra o inicia sesión, se genera una sesión. Sobre esa sesión posteriormente podrá asignar y cambiar el hogar activo. 

Las sesiones son revocables tanto por el usuario como por el equipo de gestión de Capychef.

## Flujo de autenticación y autorización

Cuando llega una petición HTTP, el middleware AuthMiddleware utiliza el servicio JWTService para extraer estos datos de los JWT de sesión o refresco. Este objeto se añade al contexto de la petición utilizando el AuthUserDetailsService. Utilizando esta misma clase, el objeto de autenticación se obtiene de nuevo en los controladores y se pasa a los servicios principales de Capychef. Los endpoints de los controladores que requieren que el usuario sea miembro del hogar que tienen seleccionado o que sea propietario del mismo son CheckMembershipAttribute y CheckOwnershipAttribute.

La JWT de sesión tiene una expiración corta (15 minutos) y se utiliza para autenticar al usuario rápidamente en cada petición. La JWT de refresco tiene una expiración mucho más larga (100 años) y se utiliza para controlar que la sesión no haya sido revocada y generar un nuevo JWT de sesión cuando el anterior haya expirado.

## AuthMiddleware

El middleware AuthMiddleware contiene una lista de rutas públicas para la cuales no es necesario que el usuario esté autenticado.

Para el resto de rutas, primero se obtienen los AuthUserDetails del JWT de sesión. Si el usuario está autenticado, se registra la conexión, se añaden los detalles al contexto de la petición y se continúa con la siguiente etapa del pipeline. 

Si la autenticación con el JWT de sesión falla, se intenta autenticar con el JWT de refresco. Si esta autenticación es exitosa, se genera un nuevo JWT de sesión, se añaden los detalles al contexto de la petición y se continúa con la siguiente etapa del pipeline.

En caso de que la autenticación falle, se registra el intento de conexión y se devuelve un error 401 Unauthorized.

## JWTService

El servicio JWTService se encarga de generar y validar los JWT de sesión y refresco.

El método CreateAndSendJWT recibe los AuthUserDetails y genera los JWT de sesión y refresco. La expiración del JWT de sesión es de 15 minutos, mientras que la del JWT de refresco es de 100 años. Ambos JWT se envían al cliente como cookies HTTPOnly.

Los métodos getUserDetailsFromSessionJWT y getUserDetailsFromRefreshJWT llaman internamente a getJWT pasando el objeto de request y el nombre de la cookie correspondiente. Este método extrae el JWT de la cookie, lo valida y devuelve los AuthUserDetails si la validación es exitosa. En caso contrario, devuelve un valor nulo.


## Filtros CheckMembershipAttribute y CheckOwnershipAttribute

Los filtros CheckMembershipAttribute y CheckOwnershipAttribute se utilizan para proteger los endpoints que requieren que el usuario sea miembro o propietario del hogar activo, respectivamente. Utilizando los datos de AuthUserDetails, se verifica con una consulta a base de datos si el usuario cumple con el requisito necesario para acceder al recurso solicitado. Si la verificación falla, se devuelve un error 404 Not Found.