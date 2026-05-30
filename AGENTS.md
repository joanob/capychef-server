# Instrucciones para agentes de IA

## Archivos de referencia obligatorios

Antes de generar cualquier código o responder cualquier pregunta técnica, lee siempre estos archivos:

- `.agents/ARCHITECTURE.md` — arquitectura del proyecto, módulos, patrones y convenciones de código
- `.agents/SECURITY_IGNORE.md` — problemas de seguridad que el desarrollador ha decidido ignorar explícitamente

## Reglas generales

- Nunca ejecutes builds para comprobar errores, el desarrollador los ejecuta manualmente
- Nunca generes documentación en el código después de crear código (comentarios explicativos, XML docs, etc.). La documentación se generará posteriormente a petición del programador
- Escribe siempre código limpio separando responsabilidades
- Si tienes dudas sobre algo que te piden, añádelas al final del archivo donde el usuario indique las tareas o en el chat
- El desarrollador trabaja con archivos `.log` como interfaz: listas de tareas, análisis, etc. 
