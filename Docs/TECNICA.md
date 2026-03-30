# Decisiones Técnicas - JokeApp

**Versión:** 1.0  
**Fecha:** Marzo 2026  
**Proyecto:** JokeApp

---

## 1. Por qué WPF

Se eligió WPF (Windows Presentation Foundation) porque:

**Desarrollo rápido de UI compleja:** WPF permite crear interfaces modernas sin código boilerplate excesivo. XAML es declarativo y visual, facilitando cambios rápidos.

**Data Binding nativo:** WPF tiene soporte integrado para data binding, esencial para MVVM. Los cambios en propiedades del ViewModel se reflejan automáticamente en la UI sin código adicional.

**Soporte MVVM nativo:** El patrón MVVM es la forma natural de desarrollar en WPF. No es una imposición, sino el diseño esperado.

**Comunidad activa:** Hay amplio ecosistema de librerías y documentación disponible.

**Alternativa rechazada:**
- Winforms: Más antigua, soporte limitado para MVVM, menos mantenible a largo plazo

---

## 2. Por qué MVVM

Se utilizó MVVM (Model-View-ViewModel) porque:

**Separación clara de responsabilidades:** View no tiene lógica. ViewModel no dibuja. Model no conoce la UI. Cada capa es independiente.

**Testing simplificado:** Se puede testear ViewModels sin necesidad de instanciar Views. Los servicios se inyectan y pueden ser mockeados.

**Reutilización de código:** La misma lógica (ViewModel) puede usarse en múltiples vistas si es necesario.

**Mantenibilidad:** Cambios en la lógica de negocio no afectan la interfaz. Cambios visuales no afectan la lógica.

**Data Binding:** WPF y MVVM trabajan juntos. Las propiedades observables notifican cambios automáticamente.

---

## 3. Por qué SQLite

Se utilizó SQLite porque:

**Sin servidor externo:** No requiere configuración de SQL Server o MySQL. El archivo `app.db` es portátil y se copia fácilmente.

**Bajo overhead:** Para una aplicación de usuario único con datos locales, SQLite es más que suficiente. No hay latencia de red.

**Integración perfecta con EF Core:** Entity Framework Core tiene soporte nativo y excelente para SQLite.

**Facilidad de distribución:** El ejecutable + archivo .db es todo lo necesario. No hay dependencias externas complicadas.

**Persistencia simple:** Los favoritos e historial se guardan localmente. El usuario no necesita cuenta ni sincronización en la nube.

**Alternativas rechazadas:**
- SQL Server: Requiere licencia, instalación compleja, overkill para esta escala
- MySQL: Requiere servidor, más configuración
- In-memory: Perdería datos al cerrar la aplicación

---

## 4. CommunityToolkit.Mvvm

Se utilizó CommunityToolkit.Mvvm porque:

**Reduce código boilerplate:** Sin toolkit, cada propiedad observable requiere INotifyPropertyChanged manual. Con el toolkit, un atributo `[ObservableProperty]` hace todo automáticamente.

**Generación de código en compilación:** Los atributos `[RelayCommand]` y `[ObservableProperty]` generan código durante la compilación. Sin magia en runtime.

**Mejor rendimiento:** El código generado es optimizado. No usa reflection en cada cambio de propiedad.

**Mantenimiento por Microsoft:** Es la librería oficial de Microsoft para MVVM en WPF, no una tercera parte desconocida.

**Ejemplo de reducción de código:**

Con toolkit:
```csharp
[ObservableProperty]
private string jokeText = "Inicio";

[RelayCommand]
private async Task GetJokeAsync() { ... }
```

Sin toolkit (mucho más código):
```csharp
private string _jokeText = "Inicio";
public string JokeText
{
    get => _jokeText;
    set
    {
        if (_jokeText != value)
        {
            _jokeText = value;
            OnPropertyChanged(nameof(JokeText));
        }
    }
}

private async void GetJoke() { ... }
// Necesitaría ICommand manual
```

---

## 5. RelayCommand

**¿Qué es?**

RelayCommand es una implementación de ICommand que ejecuta un método cuando se invoca desde la UI (ej: click de botón).

**Cómo se usa en el proyecto:**

```csharp
[RelayCommand]
private async Task GetJokeAsync()
{
    IsLoading = true;
    try
    {
        _currentJoke = await _jokeService.GetJokeAsync(SelectedCategory);
        JokeText = _currentJoke.Text;
    }
    finally
    {
        IsLoading = false;
    }
}
```

El atributo `[RelayCommand]` genera automáticamente:
- Un comando público `GetJokeAsyncCommand` 
- Lógica para ejecutar el método cuando se invoca
- Manejo automático de propiedades `IsRunning` si las necesitas

**En XAML:**

```xml
<Button Command="{Binding GetJokeAsyncCommand}" Content="Obtener Chiste" />
```

**Ventaja:** El comando es async-aware. No bloquea la UI mientras se ejecuta.

---

## 6. Data Binding

**¿Cómo funciona?**

Data binding vincula propiedades de la UI a propiedades del ViewModel. Cuando cambia el ViewModel, la UI se actualiza automáticamente.

**Ejemplo en el proyecto:**

En MainViewModel.cs:
```csharp
[ObservableProperty]
private string jokeText = "Presiona Generar Chiste";
```

En MainWindow.xaml:
```xml
<TextBlock Text="{Binding JokeText}" />
```

Cuando `GetJokeAsync()` actualiza `JokeText`, la UI se refresca sin código adicional.

**Flujo real:**

1. Usuario presiona botón "Obtener Chiste"
2. XAML invoca `GetJokeAsyncCommand`
3. `GetJokeAsync()` se ejecuta
4. `JokeText` se actualiza
5. View recibe notificación automáticamente
6. TextBlock muestra nuevo texto

**Ventaja:** Cero código en code-behind. Todo fluye desde ViewModel.

---

## 7. Estructura del Proyecto

### Views/

Contiene XAML puro sin lógica. Solo presentación.

- MainWindow.xaml: Interfaz principal
- HistoryView.xaml: Vista de historial

**Por qué sin lógica:** Cambios visuales no afectan lógica. Diseñador visual puede trabajar sin romper código.

### ViewModels/

Contiene lógica de presentación. Propiedades observables y comandos.

- MainViewModel: Controla generación de chistes/memes, navegación

**Por qué separado:** Testeable sin UI. Los servicios se inyectan como parámetros.

### Models/

**ApiResponses/:** Modelos que mapean exactamente las respuestas de APIs externas.
- Joke.cs: Estructura de JokeAPI
- Meme.cs: Estructura de Imgflip API
- ImgflipResponse.cs, ImgflipData.cs: Estructura de respuesta Imgflip

**DTOs/:** Versiones simplificadas para transferencia entre capas.
- JokeDto.cs: Contiene solo lo que necesita ViewModel
- MemeDto.cs: Simplificado de Meme.cs

**Por qué separado:** Si JokeAPI cambia estructura, solo se modifica Joke.cs. ViewModel no se toca.

**Persistencia:**
- HistoryItem.cs: Entidad para tabla HistoryItems

### Services/

Encapsulan lógica de negocio.

**Interfaces/:** Contratos públicos.
- IJokeService: GetJokeAsync(), GetAvailableCategories()
- IMemeService: GetRandomMemeAsync()

**Implementaciones:**
- JokeService: Consume JokeAPI
- MemeService: Consume Imgflip API
- HistoryService: Operaciones de BD para historial
- DatabaseService: Operaciones de BD para favoritos (Persona 2)

**Por qué interfaces:** Permite inyectar mocks en tests. Facilita cambiar implementaciones sin afectar ViewModel.

### Data/

- AppDbContext.cs: Mapeo de entidades a tablas SQLite

**Por qué centralizado:** Un único contexto abierto. Evita conflictos de múltiples conexiones.

### Docs/

Documentación técnica (este proyecto).

---


## 8. Inyección de Dependencias

**Cómo funciona en el proyecto:**

MainViewModel recibe servicios en constructor:

```csharp
public MainViewModel(
    IJokeService jokeService, 
    MemeService memeService, 
    HistoryService historyService)
{
    _jokeService = jokeService;
    _memeService = memeService;
    _historyService = historyService;
}
```

**En MainWindow.xaml.cs (configuración real):**

```csharp
public MainWindow()
{
    InitializeComponent();
    
    // 1. Configurar BD
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Data Source=app.db")
        .Options;
    var context = new AppDbContext(options);
    context.Database.EnsureCreated();  // Crea tablas si no existen
    
    // 2. Crear servicios
    var httpClient = new HttpClient();
    IJokeService jokeService = new JokeService(httpClient);
    var memeService = new MemeService(httpClient);
    var historyService = new HistoryService(context);
    
    // 3. Inyectar en ViewModel
    DataContext = new MainViewModel(jokeService, memeService, historyService);
}
```

**EnsureCreated():** Método clave que crea la base de datos y todas las tablas si no existen. Se ejecuta una sola vez al iniciar la aplicación. Esto evita scripts de migración complicados.

**Ventaja:** ViewModel no crea servicios, los recibe. Permite testear con mocks. La composición de dependencias ocurre en un único lugar.

---

## 9. Problemas y Soluciones

### Problema 1: Contexto múltiple de BD

**Qué pasó:** Inicialmente, cada ventana creaba su propio `AppDbContext`. Causaba conflictos de lectura/escritura con SQLite.

**Solución:** Un único contexto compartido creado en MainWindow, pasado a todas las vistas y servicios.

SQLite es mono-usuario. Un solo contexto = sin conflictos.

### Problema 2: UI bloqueada durante API calls

**Qué pasó:** Las primeras llamadas a JokeAPI eran sincrónicas. Si la API tardaba, la UI se congelaba.

**Solución:** Todo async/await. GetJokeAsync() es `async Task`, no `void`. Mantiene UI responsiva.

```csharp
[RelayCommand]
private async Task GetJokeAsync() // async Task, no void
{
    IsLoading = true;
    try { ... }
    finally { IsLoading = false; }
}
```

### Problema 3: Deserialización fallaba con APIs

**Qué pasó:** JokeAPI usa camelCase (`jsonPropertyName`), pero C# usa PascalCase. Deserialización fallaba.

**Solución:** `JsonSerializerOptions` con `PropertyNameCaseInsensitive = true`:

```csharp
private readonly JsonSerializerOptions _jsonOptions = 
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
```

Ahora mapea automáticamente `joke` → `JokeText`.

### Problema 4: Modelos de API cambian, rompen la app

**Qué pasó:** Si JokeAPI cambia estructura, el objeto Joke se deserializa mal.

**Solución:** Usar DTOs. Joke.cs mapea exactamente la API (puede cambiar). JokeDto.cs es estable (solo lo que ViewModel necesita).

JokeService convierte entre ellos:
```csharp
private JokeDto MapToDto(Joke raw) { ... }
```

Si API cambia, solo JokeService se modifica. ViewModel sigue igual.

---

## 10. Estructura de Base de Datos

### Tabla HistoryItems

```sql
CREATE TABLE HistoryItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ContentType TEXT NOT NULL (max 10),
    ContentId TEXT NOT NULL (max 50),
    Title TEXT NOT NULL,
    GeneratedAt DATETIME NOT NULL
);

CREATE INDEX idx_HistoryItems_GeneratedAt ON HistoryItems(GeneratedAt);
```

**Índice en GeneratedAt:** Acelera consultas ordenadas por fecha (HistoryView siempre ordena así).

### Tabla Favorites (Persona 2)

```sql
CREATE TABLE Favorites (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ContentType TEXT NOT NULL (max 10),
    ContentId TEXT NOT NULL (max 50),
    Title TEXT NOT NULL,
    ImageUrl TEXT,
    SavedAt DATETIME NOT NULL
);
```

**Diseño:**
- Un único tipo de tabla para chistes y memes
- ContentType distingue entre "joke" y "meme"
- ImageUrl es opcional (chistes no tienen imagen)

---

## 11. Configuración de Compilación

**.csproj:**
- Target Framework: net8.0
- RuntimeIdentifier: win-x64 (Windows 64-bit)
- CommunityToolkit.Mvvm: versión 8.x
- EntityFrameworkCore.Sqlite: versión 8.x

**Por qué .NET 8:** LTS (Long Term Support), soporte hasta noviembre 2026, suficiente para el plazo del proyecto.

---

## 12. Notas Importantes

**Contexto de BD compartido:** AppDbContext se crea una sola vez en MainWindow y se pasa a todos los servicios. No crear nuevos contextos en cada vista.

**Async todo:** Cualquier operación de I/O (API, BD) debe ser async. Nunca `.Result` o `.Wait()`.

**Errores silenciosos:** Los servicios retornan null en errores de red, no lanzan excepciones. El ViewModel maneja null y muestra mensaje al usuario.

**DTOs obligatorios:** Nunca exponer modelos de API directamente al ViewModel. Siempre pasar por DTO.

**EnsureCreated en startup:** Se ejecuta una única vez. Si la BD ya existe, no hace nada. Es seguro llamarlo siempre.

