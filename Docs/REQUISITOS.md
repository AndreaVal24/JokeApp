# Requisitos de JokeApp

**Versión:** 1.0  
**Fecha:** Marzo 2026  
**Proyecto:** JokeApp  
**Equipo:** 4 desarrolladores  

---

## 1. Propósito

Especificar qué debe hacer la aplicación JokeApp desde la perspectiva del usuario y del sistema.

---

## 2. Descripción General

JokeApp es una aplicación de escritorio que permite a los usuarios obtener chistes y memes de forma aleatoria, guardarlos como favoritos y mantener un historial de lo que han consumido.

La aplicación funciona de manera local, sin autenticación ni sincronización en la nube.

---

## 3. Requisitos Funcionales

### 3.1 Generar Chistes

| ID | Descripción |
|----|-------------|
| RF-01 | El usuario puede solicitar un chiste aleatorio |
| RF-02 | El sistema obtiene el chiste desde una API externa |
| RF-03 | El chiste se muestra en la interfaz |
| RF-04 | El usuario puede filtrar chistes por categoría |

### 3.2 Mostrar Memes

| ID | Descripción |
|----|-------------|
| RF-05 | El usuario puede solicitar un meme aleatorio |
| RF-06 | El sistema obtiene el meme desde una API externa |
| RF-07 | La imagen y nombre del meme se muestran en la interfaz |

### 3.3 Guardar Favoritos

| ID | Descripción |
|----|-------------|
| RF-08 | El usuario puede guardar un chiste o meme como favorito |
| RF-09 | El sistema almacena el favorito en la base de datos local |
| RF-10 | El sistema evita duplicados |

### 3.4 Ver Favoritos

| ID | Descripción |
|----|-------------|
| RF-11 | El usuario puede ver una lista de todos sus favoritos guardados |
| RF-12 | Los favoritos se muestran ordenados |
| RF-13 | El usuario puede eliminar un favorito de la lista |

### 3.5 Historial de Consumo

| ID | Descripción |
|----|-------------|
| RF-14 | El sistema registra automáticamente cada chiste y meme que el usuario genera |
| RF-15 | El usuario puede ver el historial de todo lo que ha consumido |
| RF-16 | El usuario puede limpiar el historial completo |

---

## 4. Requisitos No Funcionales

Características de calidad del sistema:

### 4.1 Interfaz Amigable

| ID | Descripción |
|----|-------------|
| RNF-01 | Interfaz intuitiva y fácil de usar |
| RNF-02 | Botones claramente etiquetados |
| RNF-03 | Tema visual consistente |
| RNF-04 | Idioma: Español |

### 4.2 Respuesta Rápida

| ID | Descripción |
|----|-------------|
| RNF-05 | API de chistes: respuesta máximo en 5 segundos |
| RNF-06 | Carga de favoritos: máximo 2 segundos |
| RNF-07 | Carga de historial: máximo 2 segundos |
| RNF-08 | Sin bloqueos durante operaciones de red |

### 4.3 Persistencia de Datos

| ID | Descripción |
|----|-------------|
| RNF-09 | Los favoritos se almacenan localmente en BD |
| RNF-10 | El historial se almacena localmente en BD |
| RNF-11 | Los datos persisten entre sesiones |

### 4.4 Confiabilidad

| ID | Descripción |
|----|-------------|
| RNF-12 | Manejo de errores de conexión a APIs |
| RNF-13 | Validación de datos antes de guardar |
| RNF-14 | No se pierden datos por fallas |

### 4.5 Compatibilidad

| ID | Descripción |
|----|-------------|
| RNF-15 | Funciona en Windows 7 o superior |
| RNF-16 | Requiere .NET 8 |
| RNF-17 | RAM mínima: 512 MB |
| RNF-18 | Resolución mínima: 1024x768 |

---

## 5. Restricciones

### 5.1 Técnicas

- Desarrollado en WPF con .NET 8
- Base de datos SQLite local
- Uso de Entity Framework Core para acceso a BD
- APIs externas: JokeAPI e Imgflip (públicas y gratuitas)

### 5.2 De Negocio

- Aplicación de usuario único
- Sin autenticación requerida
- Sin sincronización en la nube
- Datos 100% locales

### 5.3 De Desarrollo

- Equipo de 4 desarrolladores
- Entrega: marzo 2026
- Control de versiones: Git

---

## 6. Casos de Uso Principales

### Caso 1: Generar y Guardar Chiste

1. Usuario abre la aplicación
2. Usuario presiona "Obtener Chiste"
3. Sistema muestra un chiste
4. Usuario presiona "Guardar como Favorito"
5. Chiste se guarda en la BD

### Caso 2: Ver Favoritos

1. Usuario abre la ventana de Favoritos
2. Sistema muestra lista de todos los favoritos
3. Usuario puede eliminar un favorito
4. Sistema actualiza la lista

### Caso 3: Ver Historial

1. Usuario abre la ventana de Historial
2. Sistema muestra todos los chistes y memes que ha generado
3. Usuario puede limpiar todo el historial
4. Sistema vacía la lista de historial

---

## 7. Requisitos por Módulo

### Módulo Principal

| ID | Descripción |
|----|-------------|
| RF-1 | Interfaz principal funcionando |
| RF-2 | Generar chistes |
| RF-3 | Mostrar memes |
| RF-4 | Seleccionar categoría de chistes |

### Módulo de Favoritos

| ID | Descripción |
|----|-------------|
| RF-5 | Guardar favoritos |
| RF-6 | Ver favoritos |
| RF-7 | Eliminar favoritos |

### Módulo de Historial

| ID | Descripción |
|----|-------------|
| RF-8 | Registrar automáticamente consumo |
| RF-9 | Ver historial |
| RF-10 | Limpiar historial |

### Infraestructura

| ID | Descripción |
|----|-------------|
| RF-11 | Acceso a APIs de chistes |
| RF-12 | Acceso a APIs de memes |
| RF-13 | Configuración de base de datos |