# Arquitectura Capychef

## Base de datos

La base de datos es PostgreSQL. 

Las migraciones se hacen utilizando el CLI de Flyway. Los archivos de migración están en la carpeta /Migrations. Hasta que la aplicación no esté en producción, se pueden modificar las migraciones anteriores y reconstruir la base de datos.

No se pueden eliminar datos de las tablas. Para eliminar datos, se debe utilizar una regla de eliminación que los marque como eliminados sin borrarlos físicamente.

## Servidor

El servidor es una solución .NET 8.0 con dos proyectos: Capychef y Capychef.Api. El proyecto Capychef contiene la lógica de negocio y el proyecto Capychef.Api contiene los controladores y la configuración del servidor.

### Proyecto Capychef

El proyecto Capychef contiene la lógica de negocio y la persistencia de datos.

Existe una carpeta para cada módulo y dentro de esa carpeta hay una estructura de carpetas siguiendo una arquitectura limpia. 

Adicionalmente hay tres carpetas que son comunes a todos los módulos:

- Common: contiene código compartido entre los módulos.
- Infrastructure: contiene las implementaciones de los servicios externos que se utilizan en los módulos, como el servicio de email.
- Persistence: contiene el DbContext y el DbContextFactory.

#### Organización en módulos

Cada módulo representa una funcionalidad o un conjunto de funcionalidades relacionadas. Hay tres carpetas principales: Domain, Repositories y Services.

La carpeta Domain contiene a su vez 5 subcarpetas: Entities, Errors, Interfaces, Commands y Dtos.

- Entities: contiene las entidades de base de datos.
- Errors: contiene los errores relacionados con el módulo.
- Interfaces: contiene las interfaces de los servicios y repositorios del módulo.
- Commands: contiene los comandos que representan las acciones que se pueden realizar en el módulo.
- Dtos: contiene los DTOs que se utilizan para transferir datos hacia el exterior.

La carpeta Repositories contiene las implementaciones de los repositorios de entidades.

La carpeta Services contiene las implementaciones de los servicios del módulo, ya sean servicios principales o servicios secundarios.

### Proyecto Capychef.Api

El proyecto Capychef.Api contiene los controladores, la configuración del servidor, la configuración de logging, el servicio de autenticación JWT y los filtros de autorización de membresía y propiedad del hogar.

## Gestión de errores

En Capychef no se utilizan excepciones para el control de flujo. En su lugar, se utilizan objetos de error que se encapsulan en un objeto Result. Este objeto Result puede contener un valor de éxito o un error, pero no ambos al mismo tiempo. Result contiene un método para comprobar si la operación ha fallado, un método para obtener el error y un método para obtener los datos de éxito.

Todos los métodos de los servicios devuelven un objeto Result. En los controladores se comprueba si el resultado ha fallado y se devuelve el error correspondiente. En caso de éxito, se devuelven los datos correspondientes.

El error base es AppError. La información mímina que contiene un error es el ErrorType. Este valor es el que se utiliza en última instancia para determinar el código de estado HTTP que se devuelve al cliente. Además, hay un string Message que contiene un mensaje de error legible para el usuario. Este mensaje se registra en el logging de la aplicación.

Como la mayoría de errores ocurren sobre una entidad, AppError tiene una propiedad opcional Entity (clase EntityDetails). EntityDetails contiene el tipo de la entidad y el id de la entidad, que puede ser un id numérico o un string. Si la entidad tiene un id compuesto, se concatena en el string separado por espacios.

Hay clases que heredan de AppError para representar errores específicos de cada tipo o para situaciones concretas de cada módulo. 

### Tipos de error

Hay 5 tipos de error:
- NotFound: no se encuentra un objeto o recurso.
- Authentication: el usuario no está autenticado.
- Authorization: el usuario no tiene permisos para realizar la acción.
- CannotCreate: no se puede realizar la acción seleccionada.
- UnknownError: se ha producido un error inesperado.

## Entidades

La mayoría de entidades de la base de dato tienen campos comunes. Para evitar la repetición de código, se han creado clases base que contienen estos campos comunes.

**BaseEntity**
Contiene los campos id, createdAt y rowVersion. La mayoría de entidades heredan de esta clase.

**BaseDeletableEntity**
Contiene los campos deleted y deletedAt. 

