# Capychef

## Sobre Capychef 

Capychef es una aplicación de gestión de alimentos para hogares. Su objetivo es facilitar la gestión de los alimentos en el hogar, reducir el desperdicio de alimentos y fomentar hábitos de consumo más sostenibles.

Las funcionalidades de Capychef se centran en la gestión común a tiempo real entre los integrantes del hogar, la información de alimentos, la gestión de inventario, la lista de la compra, las recetas y planificación de comidas. Además, Capychef ofrece estadísticas sobre el impacto ambiental y económico de los alimentos consumidos y desperdiciados, con parámetros de CO2 y de dinero ahorrado.

## Usuarios y hogares

Todos los usuario de capychef deben tener un nombre de usuario único. Este nombre será utilizado por el resto de usuarios para identificarlos dentro de la aplicación.

Los usuarios pueden crear una cuenta de invitado, que no requiere email ni contraseña, o una cuenta registrada, que sí les permitirá iniciar sesion en múltiples dispositivos y recuperar su cuenta en caso de pérdida de acceso. Las cuentas de invitado están ligadas al dispositivo en el que se han creado, aunque se pueden transferir a otros dispositivos mediante un código de transferencia. Las cuentas registradas pueden iniciar sesión en cualquier dispositivo indicando su nombre de usario y contraseña, su email y contraseña o mediante un código de inicio de sesión único enviado a su email.

Capychef está diseñado para ser utilizado por hogares, que pueden estar formados por una o varias personas. Cada hogar es independiente y un usuario puede formar parte de varios hogares. 

## Alimentos 

Capychef dispone de una base de datos de alimentos. Los usuarios pueden modificar esta información para adaptarla a sus necesidades o crear nuevos alimentos.

Los alimentos están organizados en categorías y subcategorías, hasta 3 niveles. Los alimentos solo pueden pertenecer a categorías del último nivel, nunca a una categoría que tiene subcategorías.

Cada alimento tiene unas unidades de medida y condiciones de conservación. Estas condiciones determinarán los tiempos estimados de consumo preferente en alimentos que no tengan fecha de caducidad.

## Inventario 

El inventario de alimentos del hogar se compone de lotes. Un lote es una cantidad de un alimento con propiedades comunes, como ubicación y fecha de caducidad. 

Un hogar tiene varios espacios de almacenamiento. Cada espacio de almacenamiento tiene unas condiciones de temperatura, uqe pueden ser a temperatura ambiente, refrigerados o congelados. Según las condiciones de temperatura del espacio de almacenamiento, a partir de la información del alimento se determinará la fecha de consumo preferente estimada y se indicará al usuario cuando debe modificar el espacio de almacenamiento para que el alimento se conserve mejor.

En pisos de estudiantes, es común que cada estudiante tenga su propio espacio de almacenamiento, aunque también pueden compartirlo. En este caso, cada estudiante puede ser responsable de su propio inventario, y todos los estudiantes pueden tener acceso al inventario de los demás para facilitar la gestión conjunta de los alimentos (y no consumir alimentos que no son suyos).

## Recetas y planificación de comidas

Los usuarios pueden planificar la preparación y consumo de los alimentos. La aplicación puede sugerir recetas en base a los alimentos disponibles en el inventario o el tiempo que ha pasado desde la última vez que se consumió un alimento.

Al planificar una receta o el consumo de un alimento, se analiza la cantidad de cada alimento que se necesitará y se compara con la cantidad disponible en el inventario. Si la aplicación interpreta que no habrá cantidad suficiente en el inventario en el día de preparación o consumo, se añadirá a la lista de la compra de forma automática.

La planificacion ayuda a los usuarios no solo a organizar sus comidas, sino también a reducir el desperdicio de alimentos, ya que les permite consumir los alimentos antes de que caduquen o se estropeen. Además, la planificación de comidas puede fomentar hábitos de consumo más sostenibles, ya que los usuarios pueden elegir recetas que utilicen ingredientes de temporada o que tengan un menor impacto ambiental.

La planificación también ayuda con la preparación de las comidas. La aplicacion avisa a los usuarios sobre acciones previas que deben realizar, como sacar un alimento del congelador o realizar alguna preparación. Es un herramienta para ayudar a los usuarios a organizar su tiempo y a preparar sus comidas de manera más eficiente.

## Lista de la compra

Capychef se centra en la comida, pero no todo es comida. La lista de la compra dispone de una opción para añadir elementos que no formen parte de la base de datos de alimentos. Así, Capychef no solo se limita a gestionar los alimentos, sino que también puede ser una herramienta de gestión general de la compra del hogar.



## Funcionalidades principales

- Gestión del inventario
- Lista de la compra
- Recetas y planificación de comidas




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