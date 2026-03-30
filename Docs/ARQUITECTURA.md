# Arquitectura de JokeApp

**Versión:** 1.0  
**Fecha:** Marzo 2026  
**Proyecto:** JokeApp

---

## 1. Patrón Arquitectónico

JokeApp utiliza **MVVM (Model-View-ViewModel)** como patrón principal. Separa la interfaz de usuario de la lógica de negocio, permitiendo que cada capa evolucione independientemente.

---

## 2. Capas de la Aplicación

### View (Presentación)

Archivos XAML y code-behind de las ventanas:
- MainWindow.xaml
- FavoritesView.xaml
- HistoryView.xaml

Responsabilidad: Mostrar datos y capturar interacción del usuario. Sin lógica de negocio.

### ViewModel (Lógica de Presentación)

Clases que controlan la lógica de cada vista:
- MainViewModel.cs
- FavoritesViewModel.cs
- HistoryViewModel.cs

Responsabilidad: Procesar acciones del usuario, invocar servicios, actualizar datos que la View muestra.

### Model (Modelos de Datos)

Clases que representan datos:
- DTOs: JokeDto, MemeDto
- Persistencia: Favorite, HistoryItem
- APIs: Joke, Meme, ImgflipResponse

Responsabilidad: Estructurar datos. DTOs simplifican información. Modelos de persistencia mapean a BD.

### Service (Lógica de Negocio)

Servicios que encapsulan la lógica:
- JokeService: obtiene chistes desde JokeAPI
- MemeService: obtiene memes desde Imgflip API
- DatabaseService: gestiona favoritos en BD
- HistoryService: gestiona historial en BD

Responsabilidad: Acceder a APIs y BD, devolver datos al ViewModel.

### Data (Acceso a Datos)

- AppDbContext: contexto de Entity Framework Core

Responsabilidad: Mapear tablas SQLite a clases C#, gestionar conexión a BD.

---

## 3. Flujo de Comunicación

### Generación de Chiste

```
MainWindow (click)
    ↓
MainViewModel.GetJokeAsync()
    ↓
JokeService.GetJokeAsync()
    ↓
JokeAPI (HTTP GET)
    ↓
JokeService convierte a JokeDto
    ↓
MainViewModel actualiza JokeText
    ↓
HistoryService.AddAsync() → AppDbContext
    ↓
MainWindow se actualiza automáticamente (data binding)
```

### Guardar Favorito

```
MainWindow (click guardar)
    ↓
MainViewModel
    ↓
DatabaseService.SaveJokeAsync()
    ↓
AppDbContext → tabla Favorites
```

### Ver Favoritos

```
MainWindow (click Favoritos)
    ↓
FavoritesView se abre
    ↓
FavoritesViewModel.LoadFavoritesAsync()
    ↓
DatabaseService.GetAllFavoritesAsync()
    ↓
AppDbContext consulta tabla Favorites
    ↓
FavoritesView se actualiza (data binding)
```

---

## 4. Comunicación entre Capas

**View ↔ ViewModel**

Data binding bidireccional. View enlaza propiedades de ViewModel. Botones invocan comandos del ViewModel.

**ViewModel ↔ Service**

ViewModel inyecta servicios en el constructor. Invoca métodos públicos de servicios sin conocer detalles internos.

**Service ↔ Data**

DatabaseService e HistoryService usan AppDbContext para acceder a BD. Esto centraliza el acceso a datos.

**Service ↔ APIs Externas**

JokeService y MemeService hacen HTTP GET a APIs públicas, parsean respuestas, convierten a DTOs.

---

## 5. Patrones Aplicados

**MVVM**

Separa presentación de lógica. View no conoce servicios ni BD. ViewModel no dibuja interfaz. Model es solo datos.

**Dependency Injection**

ViewModels reciben servicios en constructor, no los crean. Facilita testing con mocks.

**Repository Pattern**

Servicios de BD (DatabaseService, HistoryService) ocultan detalles de acceso a datos. Si cambia SQLite a otra BD, solo estos servicios se modifican.

**DTO Pattern**

DTOs transfieren datos simplificados entre capas. Desacopla de cambios en APIs externas.

**Service Layer**

Servicios encapsulan lógica. ViewModels no hacen HTTP ni acceden directamente a BD.

---

## 6. Diagrama General

```
┌─────────────────────────────────────────────────────────┐
│                      VIEW (XAML)                        │
│   MainWindow | FavoritesView | HistoryView             │
└────────────────────────┬────────────────────────────────┘
                         │Data Binding
                         ↓
┌─────────────────────────────────────────────────────────┐
│                   VIEWMODEL                             │
│ MainViewModel | FavoritesViewModel | HistoryViewModel  │
└────────────────────────┬────────────────────────────────┘
                         │ Invoca métodos
                         ↓
┌─────────────────────────────────────────────────────────┐
│                     SERVICE                             │
│ JokeService | MemeService | DatabaseService |          │
│ HistoryService                                         │
└──────┬──────────────────────────────────┬───────────────┘
       │ HTTP                             │ EF Core
       ↓                                  ↓
┌──────────────┐                  ┌──────────────────┐
│  JokeAPI     │                  │  AppDbContext    │
│  Imgflip API │                  │  (SQLite)        │
└──────────────┘                  └──────────────────┘
```

---

## 7. Estructura de Carpetas

```
JokeApp/
├── Views/
│   ├── MainWindow.xaml
│   ├── FavoritesView.xaml
│   └── HistoryView.xaml
│
├── ViewModels/
│   ├── MainViewModel.cs
│   ├── FavoritesViewModel.cs
│   └── HistoryViewModel.cs
│
├── Models/
│   ├── DTOs/
│   │   ├── JokeDto.cs
│   │   └── MemeDto.cs
│   ├── ApiResponses/
│   │   ├── Joke.cs
│   │   └── Meme.cs
│   ├── Favorite.cs
│   └── HistoryItem.cs
│
├── Services/
│   ├── JokeService.cs
│   ├── MemeService.cs
│   ├── DatabaseService.cs
│   └── HistoryService.cs
│
└── Data/
    └── AppDbContext.cs

```
