# 🎭 JokeApp - Aplicación de Chistes

Aplicación de escritorio en WPF que permite generar chistes, mostrar memes y gestionar favoritos e historial.

---

## ¿Qué hace esta aplicación?

* Genera chistes aleatorios
* Muestra memes dinámicos
* Permite guardar favoritos
* Registra historial de uso
* Navega entre vistas (Principal, Favoritos, Historial)

---

## Tecnologías utilizadas

* C# (.NET)
* WPF (Windows Presentation Foundation)
* Entity Framework Core
* SQLite (base de datos local)
* Arquitectura MVVM
* Consumo de APIs REST

---

## APIs utilizadas

* **JokeAPI**
  https://v2.jokeapi.dev/
  → Proporciona chistes aleatorios

* **Imgflip API**
  https://api.imgflip.com/
  → Proporciona memes

---

## Arquitectura del proyecto

Patrón **MVVM**:

```plaintext
JokeApp/
│
├── Views/          → Interfaces (XAML)
├── ViewModels/     → Lógica de presentación
├── Models/         → Entidades (Joke, Favorite, History)
├── Services/       → Lógica de negocio (APIs, BD)
├── Data/           → DbContext (SQLite)
```

---

## Principios aplicados

* SOLID
* Separación de responsabilidades
* Uso de interfaces (desacoplamiento)
* Código modular y mantenible

---

## Base de datos

* SQLite (archivo local)
* Se crea automáticamente al ejecutar
* No requiere configuración externa

---

## ⚙️ ¿Cómo ejecutar el proyecto?

1. Clonar el repositorio
2. Abrir en Visual Studio
3. Ejecutar (`F5`)

✔ No requiere instalación adicional
✔ La base de datos se genera automáticamente

---

## ¿Cómo probar la aplicación?

* Presionar botón para generar chiste
* Guardar en favoritos
* Ver lista de favoritos
* Revisar historial de uso

---

## Organización del equipo

* Módulo principal
* Favoritos
* Historial
* Infraestructura (servicios, APIs, BD)

---


## Estado del proyecto

En desarrollo 🚧
