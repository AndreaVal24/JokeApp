# APIs - JokeApp

**Versión:** 1.0  
**Fecha:** Marzo 2026  
**Proyecto:** JokeApp

---

## 1. Resumen

JokeApp consume dos APIs externas públicas para obtener contenido:
- JokeAPI v2 para chistes
- Imgflip API para memes

Ambas son gratuitas y no requieren autenticación.

---

## 2. JokeAPI v2

### 2.1 Descripción

API que proporciona chistes en múltiples categorías. Devuelve un chiste aleatorio, filtrado por categoría e idioma.

### 2.2 Endpoint

```
GET https://v2.jokeapi.dev/joke/{category}
```

### 2.3 Parámetros

**category** (requerido): Categoría del chiste

Categorías disponibles en JokeApp:
- Any (cualquier categoría)
- Programming
- Misc
- Dark
- Pun
- Spooky
- Christmas

**lang** (parámetro query): Idioma del chiste. Valor: "en" (inglés)

**type** (parámetro query): Tipo de chiste. Valor: "single,twopart" (ambos tipos)

### 2.4 Ejemplos de Request

Obtener chiste aleatorio de cualquier categoría:

```
GET https://v2.jokeapi.dev/joke/Any?lang=en&type=single,twopart
```

Obtener chiste de programación:

```
GET https://v2.jokeapi.dev/joke/Programming?lang=en&type=single,twopart
```

### 2.5 Ejemplos de Response

Chiste tipo "single":

```json
{
  "id": 45,
  "category": "Programming",
  "type": "single",
  "joke": "Why do programmers prefer dark mode? Because light attracts bugs!",
  "lang": "en",
  "safe": true,
  "error": false
}
```

Chiste tipo "twopart":

```json
{
  "id": 1,
  "category": "Programming",
  "type": "twopart",
  "setup": "Why do Java developers wear glasses?",
  "delivery": "Because they don't C#",
  "lang": "en",
  "safe": true,
  "error": false
}
```

### 2.6 Modelo de Respuesta Cruda

Archivo: `Models/ApiResponses/Joke.cs`

```csharp
public class Joke
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("joke")]
    public string? JokeText { get; set; }
    
    [JsonPropertyName("setup")]
    public string? Setup { get; set; }
    
    [JsonPropertyName("delivery")]
    public string? Delivery { get; set; }
    
    [JsonPropertyName("lang")]
    public string Lang { get; set; } = string.Empty;
    
    [JsonPropertyName("safe")]
    public bool Safe { get; set; }
    
    [JsonPropertyName("error")]
    public bool Error { get; set; }
}
```

### 2.7 DTO (Data Transfer Object)

Archivo: `Models/DTOs/JokeDto.cs`

```csharp
public class JokeDto
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;
    public DateTime FetchedAt { get; set; } = DateTime.Now;
}
```

### 2.8 Consumo en el Proyecto

**Service:** `Services/JokeService.cs`

El servicio obtiene chistes de la API y los convierte a DTO:

```csharp
public async Task<JokeDto?> GetJokeAsync(string category, string language = "en")
{
    try
    {
        var url = $"{BaseUrl}/{Uri.EscapeDataString(category)}?lang={language}&type=single,twopart";
        
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var stream = await response.Content.ReadAsStreamAsync();
        var raw = await JsonSerializer.DeserializeAsync<Joke>(stream, _jsonOptions);
        
        if (raw == null || raw.Error)
            return null;
        
        return MapToDto(raw);
    }
    catch (HttpRequestException)
    {
        return null;
    }
    catch (JsonException)
    {
        return null;
    }
}
```

El mapeo resuelve la diferencia entre chistes "single" y "twopart":

```csharp
private static JokeDto MapToDto(Joke raw)
{
    var text = raw.Type == "twopart"
        ? $"{raw.Setup}\n\n{raw.Delivery}"
        : raw.JokeText ?? string.Empty;

    return new JokeDto
    {
        Id = raw.Id,
        Category = raw.Category,
        Text = text,
        Lang = raw.Lang,
        FetchedAt = DateTime.Now
    };
}
```

Categorías disponibles:

```csharp
private static readonly IEnumerable<string> Categories = new List<string>
{
    "Any",
    "Programming",
    "Misc",
    "Dark",
    "Pun",
    "Spooky",
    "Christmas"
};
```

**Dónde se usa:** `MainViewModel.GetJokeAsync()` cuando el usuario presiona "Obtener Chiste"

---

## 3. Imgflip API

### 3.1 Descripción

API que proporciona memes populares. Devuelve una lista de todos los memes disponibles con URLs de imágenes.

### 3.2 Endpoint

```
GET https://api.imgflip.com/get_memes
```

### 3.3 Parámetros

No requiere parámetros.

### 3.4 Ejemplo de Request

```
GET https://api.imgflip.com/get_memes
```

### 3.5 Ejemplo de Response

```json
{
  "success": true,
  "data": {
    "memes": [
      {
        "id": "61579",
        "name": "Drake Hotline Bling",
        "url": "https://i.imgflip.com/30b1gx.jpg",
        "width": 1200,
        "height": 1357,
        "box_count": 2
      },
      {
        "id": "438680",
        "name": "Batman Slapping Robin",
        "url": "https://i.imgflip.com/9ehk.jpg",
        "width": 400,
        "height": 387,
        "box_count": 2
      }
    ]
  }
}
```

### 3.6 Modelos de Respuesta Cruda

Archivo: `Models/ApiResponses/ImgflipResponse.cs`

```csharp
public class ImgflipResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("data")]
    public ImgflipData? Data { get; set; }
}
```

Archivo: `Models/ApiResponses/ImgflipData.cs`

```csharp
public class ImgflipData
{
    [JsonPropertyName("memes")]
    public List<Meme> Memes { get; set; } = new();
}
```

Archivo: `Models/ApiResponses/Meme.cs`

```csharp
public class Meme
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("box_count")]
    public int BoxCount { get; set; }
}
```

### 3.7 DTO (Data Transfer Object)

Archivo: `Models/DTOs/MemeDto.cs`

```csharp
public class MemeDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime FetchedAt { get; set; } = DateTime.Now;
}
```

### 3.8 Consumo en el Proyecto

**Service:** `Services/MemeService.cs`

Obtener meme aleatorio:

```csharp
public async Task<MemeDto?> GetRandomMemeAsync()
{
    var memes = (await GetAllMemesAsync()).ToList();

    if (!memes.Any())
        return null;

    var index = _random.Next(memes.Count);
    return memes[index];
}
```

Obtener todos los memes:

```csharp
public async Task<IEnumerable<MemeDto>> GetAllMemesAsync()
{
    try
    {
        var response = await _httpClient.GetAsync(ApiUrl);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var raw = await JsonSerializer.DeserializeAsync<ImgflipResponse>(stream, _jsonOptions);

        if (raw == null || !raw.Success || raw.Data == null)
            return Enumerable.Empty<MemeDto>();

        return raw.Data.Memes.Select(MapToDto);
    }
    catch (HttpRequestException)
    {
        return Enumerable.Empty<MemeDto>();
    }
    catch (JsonException)
    {
        return Enumerable.Empty<MemeDto>();
    }
}
```

Mapeo a DTO:

```csharp
private static MemeDto MapToDto(Meme raw)
{
    return new MemeDto
    {
        Id = raw.Id,
        Name = raw.Name,
        Url = raw.Url,
        FetchedAt = DateTime.Now
    };
}
```

**Dónde se usa:** `MainViewModel.GetMemeAsync()` cuando el usuario presiona "Obtener Meme"

---

## 4. Flujo de Consumo en la Aplicación

### Generar Chiste

```
MainWindow (usuario click en "Obtener Chiste")
    ↓
MainViewModel.GetJokeAsync()
    ↓
JokeService.GetJokeAsync(category)
    ↓
HTTP GET a https://v2.jokeapi.dev/joke/{category}?lang=en&type=single,twopart
    ↓
Response JSON deserializado a objeto Joke
    ↓
Mapeo a JokeDto (resuelve single/twopart)
    ↓
MainViewModel actualiza propiedad JokeText
    ↓
HistoryService.AddAsync() registra en BD
    ↓
MainWindow se actualiza automáticamente (data binding)
```

### Obtener Meme

```
MainWindow (usuario click en "Obtener Meme")
    ↓
MainViewModel.GetMemeAsync()
    ↓
MemeService.GetRandomMemeAsync()
    ↓
HTTP GET a https://api.imgflip.com/get_memes
    ↓
Response JSON deserializado a ImgflipResponse
    ↓
Selecciona meme aleatorio
    ↓
Mapeo a MemeDto
    ↓
MainViewModel actualiza propiedades MemeUrl y MemeName
    ↓
HistoryService.AddAsync() registra en BD
    ↓
MainWindow se actualiza automáticamente (data binding)
```

---

## 5. Manejo de Errores

### HttpRequestException

Errores de conectividad, timeout o respuesta HTTP no exitosa. El servicio retorna null.

### JsonException

JSON malformado o incompatible con el modelo. El servicio retorna null.

### Validaciones

- Verificar que la respuesta no sea null
- Verificar field "error" en Joke (si es true, la API devolvió un error)
- Verificar field "success" en ImgflipResponse
- Verificar que la lista de memes no esté vacía

---

## 6. Opciones JSON

Ambos servicios usan:

```csharp
private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
```

Esto permite que las propiedades camelCase de las APIs se mapeen correctamente a las propiedades PascalCase de C#.

---

## 7. Referencias Oficiales

**JokeAPI**
- Documentación: https://v2.jokeapi.dev/
- Categorías: Any, Programming, Misc, Dark, Pun, Spooky, Christmas
- Idiomas soportados: en, de, es, fr, pt, ja

**Imgflip API**
- Documentación: https://imgflip.com/api
- Endpoint: https://api.imgflip.com/get_memes

