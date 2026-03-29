# JokeApp - Documentación de Avance v2

**Versión:** 2.0  
**Rama:** `develop`  
**Fecha:** 29/03/2026  
**Tipo:** Segunda Entrega Técnica - Módulos de Favoritos e Historial

---

## Objetivo de la Versión

Integrar dos módulos complementarios de persistencia de datos: Favoritos (Joseidy) e Historial (Dubenny), completando la funcionalidad de gestión de contenido en la aplicación.

---

## Trabajo Realizado (Actualización v2)

### Módulo de Favoritos — Joseidy

**Componentes Implementados:**

| Componente | Ubicación | Responsabilidad |
|-----------|-----------|-----------------|
| `Favorite.cs` | Models/ | Modelo de entidad para favoritos en BD |
| `FavoritesService.cs` | Services/ | Lógica de persistencia (CRUD de favoritos) |
| `FavoritesViewModel.cs` | ViewModels/ | Lógica de presentación y comandos |
| `FavoritesView.xaml` | Views/ | Interfaz visual de favoritos |
| `FavoritesView.xaml.cs` | Views/ | Code-behind de la vista |

**Características Técnicas:**

- ✔️ Guardado de chistes y memes como favoritos
- ✔️ Listado organizado de contenido guardado
- ✔️ Eliminación de favoritos con confirmación
- ✔️ Prevención de duplicados en BD
- ✔️ Data binding reactivo con `ObservableCollection<T>`
- ✔️ Integración con `AppDbContext` (Entity Framework Core)
- ✔️ Patrón MVVM con `CommunityToolkit.Mvvm`

**Decisiones de Diseño:**

- Uso de `DatabaseService` como intermediario entre ViewModel y BD
- Aplicación de principios SOLID (especialmente Responsabilidad Única)
- Métodos async/await para no bloquear la UI durante operaciones de BD
- Propiedades observables con `[ObservableProperty]` para reducir código repetitivo

**Métodos Principales del Servicio:**

```csharp
GetAllFavoritesAsync()           // Obtiene todos los favoritos
SaveJokeAsync(JokeDto joke)      // Guarda chiste como favorito
SaveMemeAsync(MemeDto meme)      // Guarda meme como favorito
DeleteFavoriteAsync(int id)      // Elimina favorito específico
EnsureFavoritesTableAsync()      // Garantiza existencia de tabla
```

**Resultado:**

Usuario puede guardar, listar y eliminar sus contenidos favoritos de forma persistente en SQLite. La interfaz se actualiza automáticamente con cada acción.

---

### Módulo de Historial — Dubenny

**Componentes Implementados:**

| Componente | Ubicación | Responsabilidad |
|-----------|-----------|-----------------|
| `HistoryItem.cs` | Models/ | Modelo de entidad para historial en BD |
| `HistoryService.cs` | Services/ | Lógica de persistencia (operaciones de historial) |
| `HistoryViewModel.cs` | ViewModels/ | Lógica de presentación y comandos |
| `HistoryView.xaml` | Views/ | Interfaz visual de historial |
| `HistoryView.xaml.cs` | Views/ | Code-behind de la vista |

**Características Técnicas:**

- ✔️ Registro automático de chistes generados
- ✔️ Registro automático de memes generados
- ✔️ Listado ordenado del más reciente al más antiguo
- ✔️ Limpieza completa del historial
- ✔️ Formato legible de fechas (dd/MM/yyyy HH:mm)
- ✔️ Índice en `GeneratedAt` para consultas eficientes
- ✔️ Integración transparente con `MainViewModel`

**Métodos Principales del Servicio:**

```csharp
AddAsync(contentType, contentId, title)      // Registra contenido en historial
GetAllAsync()                                 // Obtiene historial ordenado
ClearAllAsync()                               // Elimina todos los registros
```

**Flujo de Captura:**

1. Usuario genera chiste/meme en MainWindow
2. `MainViewModel` invoca `HistoryService.AddAsync()`
3. Servicio crea `HistoryItem` con timestamp actual
4. EF Core persiste el registro en SQLite
5. Historial se actualiza automáticamente

**Modificaciones a Archivos Existentes:**

- `AppDbContext.cs`: Agregado `DbSet<HistoryItem>` e índice en `GeneratedAt`
- `MainWindow.xaml.cs`: Configuración de `DbContext` y `EnsureCreated()`
- `MainViewModel.cs`: Inyección de `HistoryService` y llamadas a `AddAsync()`

**Resultado:**

Registro automático y persistente de toda actividad del usuario. Historial completamente funcional con limpieza manual disponible.

---

## Estado Actual del Sistema (v2)

### Funcionalidades Operativas Completas

| Funcionalidad | Estado | Módulo |
|--------------|--------|--------|
| Consumo de APIs (chistes/memes) | ✔️ | Infraestructura |
| Interfaz principal | ✔️ | Módulo Principal |
| Almacenamiento de favoritos | ✔️ | Favoritos |
| Listado de favoritos | ✔️ | Favoritos |
| Eliminación de favoritos | ✔️ | Favoritos |
| Registro automático de historial | ✔️ | Historial |
| Visualización del historial | ✔️ | Historial |
| Limpieza de historial | ✔️ | Historial |
| Persistencia en SQLite | ✔️ | Ambos módulos |




### Arquitectura Final (v2)

```
Views/
├── MainWindow.xaml
├── FavoritesView.xaml
└── HistoryView.xaml

ViewModels/
├── MainViewModel.cs
├── FavoritesViewModel.cs
└── HistoryViewModel.cs

Services/
├── JokeService.cs
├── MemeService.cs
├── DatabaseService.cs (Favoritos)
└── HistoryService.cs

Models/
├── Joke.cs
├── Meme.cs
├── Favorite.cs
└── HistoryItem.cs

Data/
└── AppDbContext.cs
```