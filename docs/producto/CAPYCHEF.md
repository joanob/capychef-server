# Capychef

## Objetivo

Blah blah blah

## Funcionalidades

- Gestión común a tiempo real entre los integrantes del hogar
- Información de alimentos
- Gestión de inventario 
- Lista de la compra
- Recetas y planificación de comidas
- Impacto ambiental y económico en parámetros de CO2 y de dinero ahorrado

### Hogares

Un hogar es un grupo de personas que comparten un espacio común y gestionan su alimentación de manera conjunta.

En cada hogar existen varios espacios de almacenamiento. Estos espacios de almacenamiento pueden ser a temperatura ambiente, refrigerados o congelados. 

### Alimentos

Capychef dispone de una base de datos de alimentos. Los usuarios pueden modificar esta información para adaptarla a sus necesidades o crear nuevos alimentos.

Son alimentos los ingredientes, la comida preparada mediante recetas y los platos preparados. 

Los alimentos están organizados en categorías y subcategorías, hasta 3 niveles. Los alimentos solo pueden pertenecer a categorías del último nivel, nunca a una categoría que tiene subcategorías.

Cada alimento tiene unas unidades de medida, que pueden ser de peso, volumen o conteo. Una de ellas será la unidad base y las demás podrán tener un factor de conversión respecto a la unidad base. 

Los alimentos tienen un tiempo máximo de caducidad o de consumo preferente. Esta información pasará a los lotes de alimento del inventario, según su método de conservación. Se pueden añadir notificaciones para que avisen al usuario cuando un lote de alimento esté próximo a caducar o a su fecha de consumo preferente.

Para cada alimento se puede establecer una cantidad mímina. Si la cantidad total de ese alimento en el inventario es menor que la cantidad mínima, se añadirá automáticamente a la lista de la compra. También se podrán añadir cantidades mínimas según espacio de almacenamiento o condiciones de almacenamiento.

Los alimentos se pueden asociar a un supermercado preferido, informar su precio y el impacto ambiental.

### Inventario

El inventario de alimentos se compone de lotes. Un lote es una cantidad de un alimento con propiedades comunes. El flujo de un lote es el siguiente:
1. Se compra o prepara el alimento
2. Se guarda el alimento en un espacio de almacenamiento. Dependiendo de las características del espacio de almacenamiento, a partir de la información del alimento se asignará una fecha de caducidad o de consumo preferente.
3. Si el alimento se traslada a otro espacio de almacenamiento, se actualizará la fecha de caducidad o de consumo preferente según las características del nuevo espacio de almacenamiento.
4. Si el alimento se consume, se elimina el lote del inventario. Si el alimento se estropea, se elimina el lote del inventario y se registra la cantidad de alimento desperdiciado.
5. Se pueden consumir, eliminar o trasladar cantidades parciales de un lote. En este caso, se actualizará la cantidad del lote original y se creará un nuevo lote con la cantidad consumida, eliminada o trasladada.
6. Se podrán añadir notificaciones para avisar al usuario de que un lote de alimento está próximo a caducar o a su fecha de consumo preferente. Si el alimento ya tiene esta notificación, al crear el lote se asignará automáticamente.

### Lista de la compra

La lista de la compra dispone de una opción para añadir elementos que no formen parte de la base de datos de alimentos. Así, Capychef no solo se limita a gestionar los alimentos, sino que también puede ser una herramienta de gestión general de la compra del hogar.

Los elementos de la lista de la compra se pueden organizar según la categoría, el supermercado preferido o la receta para la que se necesita.

Los elementos de la lista no necesariamente deben tener una cantidad asociada.

El proceso de compra dentro de Capychef es el siguiente:
1. Se añaden elementos a la lista de la compra, ya sea manualmente o automáticamente
2. Mientras el usuario está comprando, se marcan elementos como comprados
3. Una vez finalizada la compra, el usuario debe indicar qué cantidad de cada alimento ha guardado en cada espacio de almacenamiento.

### Recetas y planificación de comidas

Los usuarios pueden planificar la preparación y consumo de los alimentos. La aplicación puede sugerir recetas en base a los alimentos disponibles en el inventario o el tiempo que ha pasado desde la última vez que se consumió un alimento.

El usuario puede crear recetas o acceder a la lista de recetas de la comunidad. Cada receta tiene como datos obligatorios la lista de ingredientes y sus cantidades. La información adicional que puede tener la receta es el tiempo de preparación, la dificultad y los pasos a seguir.

Al añadir una receta o alimento a la planificación, según el número de comensales se calcula la cantidad necesaria de cada alimento y se añade a la lista de la compra de forma automática si la aplicación interpreta que no habrá cantidad suficiente en el inventario en el día de preparación o consumo.

### Impacto ambiental y económico

La aplicación dispone de estadísticas de alimentos consumidos y desperdiciados. A partir de los datos ambientales y económicos de cada alimento, se calcula el impacto ambiental y económico.