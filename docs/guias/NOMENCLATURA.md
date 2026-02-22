# Nomenclatura Capychef

## Base de datos

Las migraciones siguen el formato V{número de versión}__{modulo}.sql en las versiones previas a la 1.0, y el formato V{número de versión}__{descripción}.sql a partir de que el sistema esté en productivo y tenga datos reales.

Todos los elementos están en inglés. Se utiliza snake_case para los nombres de las tablas y de las columnas. Los nombres de las tablas están en plural.

Los índices se nombran con idx_{nombre_tabla}_{nombre_columna}. Para los índices compuestos, se nombran con idx_{nombre_tabla}_{nombre_columna1}_{nombre_columna2}.

Las reglas de deletion se nombran con {nombre_tabla}_delete_restrict. Se utiliza por defecto el DO NOTHING.

Las vistas se nombran con v_{descripción}. La descripción puede ser el nombre de la tabla o lo que representa la vista.

#### Nomenclatura de campos

Los campos que son claves foráneas se nombran con {nombre_tabla_en_singular_id}.

Los campos que se refieren a la fecha en la que se realiza una acción (created, deleted, updated) se nombran con {participio_action}_at y el usuario que la ejecuta con {participio_action}_by. Por ejemplo, created_at y created_by.

Los booleanos se nombran con is_{descripción}.

## Controladores

Los controladores se nombran con {nombre_entidad_o_funcionalidad}Controller.

La ruta del controlador se nombra con el nombre de la entidad en plural o el nombre de la funcionalidad. En caso de ser varias palabras se utiliza el formato kebab-case.

Los métodos de los controladores se nombran con el verbo que representa la acción que realizan. Su ruta será ese mismo nombre con kebab-case. 

Las acciones CRUD se nombran con Create, Get, Update y Delete. La ruta no contiene la acción que realizan, se sobreentiende por el método HTTP que se utiliza.

Las rutas que requieran el id de la entidad principal del controlador tendrán el parámetro {id}. Los ids de las entidades secundarias deberán ser parámetros de query y tendrán la nomenclatura {nombre_entidad_en_singular}_id.

## Middlewares

Los middlewares se nombran con {descripción}Middleware.

## Atributos y filtros

Los atributos se nombrean con {descripción}Attribute.

Los filtros se nombran con {descripción}Filter.

## Servicios

#### Servicios primarios

Los servicios principales se llaman directamente desde los controladores. Se nombran con {nombre_entidad_o_funcionalidad}Service.

#### Servicios secundarios 

Los servicios secundarios o utilitarios se utilizan en los servicios principales para realizar tareas específicas. Se nombran con {descripción}Util o {descripcion}{verbo en infinitivo}, por ejemplo Converter o Helper.

## Repositorios

Los repositorios de entidades de base de dato se nombran con {nombre_entidad}Repository.

Los repositorios de vistas se nombran con {nombre_vista}ViewRepository.

Los repositorios de consultas complejas o específicas se nombran con {descripción}Repository.

## Errores

Los errores se nombran con {descripción}Error. 

Los errores que se refieren a una entidad específica se nombran con {nombre_entidad}{descripción}Error.

## Entidades de base de datos

Las entidades de base de datos se nombran como el nombre de la tabla en singular y con formato PascalCase.

## Comandos

El nombre de los comandos es {Accion}{Entidad}Cmd. Por ejemplo, CreateUser o UpdateRecipe.

## DTOs

Los DTOs se nombran con {nombre_entidad}Dto. Si es un agregado, se nombra con {nombre_entidad}{descripción}Dto. 
