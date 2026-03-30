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


## 📥 Descargar y Ejecutar

### Opción 1: Ejecutable (Recomendado)

1. Ve a la sección **Releases** en este repositorio
2. Descarga la última versión: `JokeApp-v1.0.zip`
3. Extrae el archivo
4. Ejecuta `JokeApp.exe`

✔ No requiere instalación  
✔ No requiere .NET 8 preinstalado  
✔ Funciona automáticamente

### Opción 2: Desde el código fuente

1. Clona el repositorio:
```bash
   git clone https://github.com/AndreaVal24/JokeApp.git
```
2. Abre `JokeApp.sln` en Visual Studio
3. Presiona `F5` para ejecutar
4. La base de datos se genera automáticamente

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

Completado
