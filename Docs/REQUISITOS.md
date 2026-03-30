# Requisitos de JokeApp

**Versión:** 1.0  
**Fecha:** Marzo 2026  
**Proyecto:** JokeApp

---

## 1. Propósito

Especificar qué debe hacer la aplicación JokeApp desde la perspectiva del usuario y del sistema.

---

## 2. Descripción General

JokeApp es una aplicación de escritorio que permite a los usuarios obtener chistes y memes de forma aleatoria, guardarlos como favoritos y mantener un historial de lo que han consumido.

---

## 3. Requisitos Funcionales

Lo que la aplicación DEBE hacer:

### Generar Chistes

- El usuario puede solicitar un chiste aleatorio
- El sistema obtiene el chiste desde una API externa
- El chiste se muestra en la interfaz
- El usuario puede filtrar chistes por categoría

### Mostrar Memes

- El usuario puede solicitar un meme aleatorio
- El sistema obtiene el meme desde una API externa
- La imagen y nombre del meme se muestran en la interfaz

### Guardar Favoritos

- El usuario puede guardar un chiste o meme como favorito
- El sistema almacena el favorito en la base de datos local
- El sistema evita duplicados

### Ver Favoritos

- El usuario puede ver una lista de todos sus favoritos guardados
- Los favoritos se muestran ordenados
- El usuario puede eliminar un favorito de la lista

### Historial de Consumo

- El sistema registra automáticamente cada chiste y meme que el usuario genera
- El usuario puede ver el historial de todo lo que ha consumido
- El usuario puede limpiar el historial completo

---

## 4. Requisitos No Funcionales

Características de calidad del sistema:

### Interfaz Amigable

- Interfaz intuitiva y fácil de usar
- Botones claramente etiquetados
- Tema visual consistente
- Idioma: Español

### Respuesta Rápida

- API de chistes: respuesta máximo en 5 segundos
- Carga de favoritos: máximo 2 segundos
- Carga de historial: máximo 2 segundos
- Sin bloqueos durante operaciones de red

### Persistencia de Datos

- Los favoritos se almacenan localmente en BD
- El historial se almacena localmente en BD
- Los datos persisten entre sesiones

### Confiabilidad

- Manejo de errores de conexión a APIs
- Validación de datos antes de guardar
- No se pierden datos por fallas

### Compatibilidad

- Funciona en Windows 7 o superior
- Requiere .NET 8
- RAM mínima: 512 MB
- Resolución mínima: 1024x768

---

## 5. Restricciones

### Técnicas

- Desarrollado en WPF con .NET 8
- Base de datos SQLite local
- Uso de Entity Framework Core para acceso a BD
- APIs externas: JokeAPI e Imgflip (públicas y gratuitas)

### De Negocio

- Aplicación de usuario único
- Sin autenticación requerida
- Sin sincronización en la nube
- Datos 100% locales

### De Desarrollo

- Equipo de 4 desarrolladores
- Entrega: marzo 2026
- Control de versiones: Git

---

## 6. Casos de Uso Principales

**Caso 1: Generar y Guardar Chiste**

1. Usuario abre la aplicación
2. Usuario presiona "Obtener Chiste"
3. Sistema muestra un chiste
4. Usuario presiona "Guardar como Favorito"
5. Chiste se guarda en la BD

**Caso 2: Ver Favoritos**

1. Usuario abre la ventana de Favoritos
2. Sistema muestra lista de todos los favoritos
3. Usuario puede eliminar un favorito
4. Sistema actualiza la lista

**Caso 3: Ver Historial**

1. Usuario abre la ventana de Historial
2. Sistema muestra todos los chistes y memes que ha generado
3. Usuario puede limpiar todo el historial
4. Sistema vacía la lista de historial

---

## 7. Requisitos por Módulo

### Módulo Principal 

- RF-1: Interfaz principal funcionando
- RF-2: Generar chistes
- RF-3: Mostrar memes
- RF-4: Seleccionar categoría de chistes

### Módulo de Favoritos 

- RF-5: Guardar favoritos
- RF-6: Ver favoritos
- RF-7: Eliminar favoritos

### Módulo de Historial 

- RF-8: Registrar automáticamente consumo
- RF-9: Ver historial
- RF-10: Limpiar historial

### Infraestructura

- RF-11: Acceso a APIs de chistes
- RF-12: Acceso a APIs de memes
- RF-13: Configuración de base de datos

