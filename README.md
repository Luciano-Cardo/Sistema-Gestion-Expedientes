# Sistema-Gestion-Expedientes
Sistema de Gestión de Expedientes desarrollado en C# .NET utilizando Clean Architecture y Domain Driven Design (DDD).

# SGE — Sistema de Gestión de Expedientes
## Documento Explicativo — Fase 1
 
---
 
## Cómo ejecutar el proyecto
 
Desde la carpeta raíz de la solución, ejecutar:
 
```
dotnet run --project SGE.Consola
```
 
---
 
## Estructura del Program.cs
 
El `Program.cs` actúa como **Composition Root**: instancia todos los repositorios y servicios de infraestructura, los inyecta en los casos de uso, y luego simula un flujo completo de operaciones.
 
```csharp
// Infraestructura
IExpedienteRepository expedienteRepo = new ExpedienteTxtRepository();
ITramiteRepository    tramiteRepo    = new TramiteTxtRepository();
IAutorizacionService  autorizacion   = new AutorizacionProvisionalService();
 
// Servicio de aplicación
var servicioEstado = new ActualizacionEstadoExpedienteService(expedienteRepo, tramiteRepo);
 
// Casos de uso
var agregarExpediente  = new AgregarExpedienteUseCase(expedienteRepo, autorizacion);
var eliminarExpediente = new EliminarExpedienteUseCase(expedienteRepo, tramiteRepo, autorizacion);
var modificarCaratula  = new ModificarCaratulaExpedienteUseCase(expedienteRepo, autorizacion);
var cambiarEstado      = new CambiarEstadoExpedienteUseCase(expedienteRepo, autorizacion);
var listarExpedientes  = new ListarExpedientesUseCase(expedienteRepo);
var agregarTramite     = new AgregarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var eliminarTramite    = new EliminarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var modificarTramite   = new ModificarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var listarTramites     = new ListarTramitesPorExpedienteUseCase(tramiteRepo);
```
 
---
 
## Camino Feliz
 
### 1. Alta de Expedientes
 
Se crean dos expedientes. Al crearse, su estado inicial es siempre `RecienIniciado`.
 
```csharp
var r1 = agregarExpediente.Ejecutar(
    new AgregarExpedienteRequest("Expediente sobre construcción ilegal", usuarioId));
expId1 = r1.id;
Console.WriteLine($"Expediente 1 creado. ID: {expId1}");
 
var r2 = agregarExpediente.Ejecutar(
    new AgregarExpedienteRequest("Expediente de habilitación comercial", usuarioId));
expId2 = r2.id;
Console.WriteLine($"Expediente 2 creado. ID: {expId2}");
```
 
**Salida por consola:**
```
--Alta de expedientes--
Expediente 1 creado. ID: a3f1c2d4-8e5b-4a7f-9c1d-2b3e4f5a6b7c
Expediente 2 creado. ID: b4e2d3c5-9f6a-5b8e-0d2e-3c4f5a6b7c8d
```
 
---
 
### 2. Listado de Expedientes
 
```csharp
foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
    Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
```
 
**Salida por consola:**
```
--Listado de expedientes--
[RecienIniciado] Expediente sobre construcción ilegal
[RecienIniciado] Expediente de habilitación comercial
```
 
---
 
### 3. Alta de Trámites con Cambio de Estado Automático
 
Al agregar un trámite con etiqueta `PaseAEstudio`, el sistema detecta que es el último trámite y cambia automáticamente el estado del expediente a `ParaResolver`.
 
```csharp
var rt1 = agregarTramite.Ejecutar(new AgregarTramiteRequest(
    usuarioId, expId1, EtiquetaTramite.EscritoPresentado,
    new ContenidoTramite("Se presenta el escrito inicial")));
tramiteId1 = rt1.Id;
Console.WriteLine($"Tramite 'EscritoPresentado' agregado. ID: {tramiteId1}");
 
var rt2 = agregarTramite.Ejecutar(new AgregarTramiteRequest(
    usuarioId, expId1, EtiquetaTramite.PaseAEstudio,
    new ContenidoTramite("Pase a estudio del departamento técnico")));
tramiteId2 = rt2.Id;
Console.WriteLine($"Tramite 'PaseAEstudio' agregado. El expediente debería pasar a 'ParaResolver'");
```
 
**Salida por consola:**
```
--Alta de tramites--
Tramite 'EscritoPresentado' agregado. ID: c5f3e4d6-0a7b-6c9f-1e3f-4d5a6b7c8d9e
Tramite 'PaseAEstudio' agregado. El expediente debería pasar a 'ParaResolver'
```
 
---
 
### 4. Verificación del Cambio de Estado Automático
 
```csharp
foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
    Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
```
 
**Salida por consola:**
```
--Estado de expedientes tras agregar trámites--
[ParaResolver] Expediente sobre construcción ilegal
[RecienIniciado] Expediente de habilitación comercial
```
 
El expediente 1 pasó a `ParaResolver` automáticamente. El expediente 2 sigue en `RecienIniciado` porque no tiene trámites.
 
---
 
### 5. Listado de Trámites por Expediente
 
```csharp
foreach (var t in listarTramites.Ejecutar(new ListarTramitesPorExpedienteRequest(expId1)))
    Console.WriteLine($"[{t.Etiqueta}] {t.Contenido.Valor}");
```
 
**Salida por consola:**
```
--Trámites del expediente 1
[EscritoPresentado] Se presenta el escrito inicial
[PaseAEstudio] Pase a estudio del departamento técnico
```
 
---
 
### 6. Agregar Trámite "Resolución" → Estado pasa a ConResolucion
 
```csharp
agregarTramite.Ejecutar(new AgregarTramiteRequest(
    usuarioId, expId1, EtiquetaTramite.Resolucion,
    new ContenidoTramite("Se emite resolución favorable")));
Console.WriteLine("Tramite 'Resolución' agregado. El expediente debería pasar a 'ConResolucion'");
```
 
**Salida por consola:**
```
--Agregar tramite 'Resolucion'--
Tramite 'Resolución' agregado. El expediente debería pasar a 'ConResolucion'
```
 
---
 
### 7. Cambio de Estado Manual
 
El usuario puede cambiar el estado manualmente sin necesidad de agregar un trámite. En este caso se pasa a `EnNotificacion` aunque el último trámite sea una `Resolución`.
 
```csharp
cambiarEstado.Ejecutar(new CambiarEstadoExpedienteRequest(
    expId1, EstadoExpediente.EnNotificacion, usuarioId));
Console.WriteLine("Estado cambiado manualmente a 'EnNotificacion'");
```
 
**Salida por consola:**
```
--Cambio de estado manual a 'EnNotificacion'--
Estado cambiado manualmente a 'EnNotificacion'
```
 
---
 
### 8. Modificar Carátula
 
```csharp
modificarCaratula.Ejecutar(new ModificarCaratulaExpedienteRequest(
    expId1, "Expediente sobre construcción ilegal - CORREGIDO", usuarioId));
Console.WriteLine("Caratula modificada correctamente");
```
 
**Salida por consola:**
```
--Modificar caratula del expediente 1--
Caratula modificada correctamente
```
 
---
 
### 9. Modificar Contenido de un Trámite
 
```csharp
modificarTramite.Ejecutar(new ModificarTramiteRequest(
    tramiteId1,
    new ContenidoTramite("Escrito inicial corregido con documentación adicional"),
    usuarioId));
Console.WriteLine("Tramite modificado correctamente");
```
 
**Salida por consola:**
```
--Modificar contenido de un tramite--
Tramite modificado correctamente
```
 
---
 
### 10. Eliminar un Trámite
 
```csharp
eliminarTramite.Ejecutar(new EliminarTramiteRequest(usuarioId, tramiteId1));
Console.WriteLine($"Trámite {tramiteId1} eliminado");
```
 
**Salida por consola:**
```
--Eliminar tramite--
Trámite c5f3e4d6-0a7b-6c9f-1e3f-4d5a6b7c8d9e eliminado
```
 
---
 
### 11. Eliminar Expediente en Cascada
 
Al eliminar un expediente, el sistema elimina primero todos sus trámites asociados y luego el expediente.
 
```csharp
eliminarExpediente.Ejecutar(new EliminarExpedienteRequest(expId2, usuarioId));
Console.WriteLine("Expediente 2 eliminado junto con todos sus tramites");
```
 
**Salida por consola:**
```
--Eliminar expediente 2 (con sus tramites)--
Expediente 2 eliminado junto con todos sus tramites
```
 
---
 
### 12. Listado Final
 
```csharp
foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
    Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
```
 
**Salida por consola:**
```
--Listado final de expedientes--
[EnNotificacion] Expediente sobre construcción ilegal - CORREGIDO
```
 
Solo queda el expediente 1 con la carátula corregida y el estado actualizado. El expediente 2 fue eliminado.
 
---
 
## Caminos de Error
 
### Error 1: Carátula vacía → DominioException
 
El Value Object `Caratula` valida que el texto no sea vacío. Si lo es, lanza una `DominioException` antes de que la entidad llegue a crearse.
 
```csharp
agregarExpediente.Ejecutar(new AgregarExpedienteRequest("", usuarioId));
```
 
**Salida por consola:**
```
--Error 1: caratula vacia--
[DominioException] La caratula no puede estar vacia
```
 
---
 
### Error 2: Contenido de trámite vacío → DominioException
 
El Value Object `ContenidoTramite` valida que el contenido no sea vacío.
 
```csharp
agregarTramite.Ejecutar(new AgregarTramiteRequest(
    usuarioId, expId1, EtiquetaTramite.Despacho, new ContenidoTramite("")));
```
 
**Salida por consola:**
```
--Error 2: contenido de tramite vacio--
[DominioException] El contenido del tramite no puede estar vacio
```
 
---
 
### Error 3: Sin autorización → AutorizacionException
 
Se usa `AutorizacionSiempreDenegadaService`, que siempre devuelve `false`, para simular un usuario sin permisos.
 
```csharp
IAutorizacionService sinPermisos = new AutorizacionSiempreDenegadaService();
var agregarSinPermiso = new AgregarExpedienteUseCase(expedienteRepo, sinPermisos);
agregarSinPermiso.Ejecutar(new AgregarExpedienteRequest("No deberia crearse", usuarioId));
```
 
**Salida por consola:**
```
--Error 3: sin autorizacion--
[AutorizacionException] El usuario no posee la autorización para crear expedientes
```
 
---
 
### Error 4: Expediente inexistente → EntidadNoEncontradaException
 
Se intenta cambiar el estado de un expediente con un ID que no existe en el sistema.
 
```csharp
cambiarEstado.Ejecutar(new CambiarEstadoExpedienteRequest(
    Guid.NewGuid(), EstadoExpediente.Finalizado, usuarioId));
```
 
**Salida por consola:**
```
--Error 4: expediente que no existe--
[EntidadNoEncontradaException] No existe expediente con ese ID
```
 
---
 
## Persistencia en archivos de texto
 
Los datos se guardan en dos archivos de texto plano en el directorio de ejecución:
 
- `Datos de los expedientes` — un expediente por bloque de 6 líneas: Id, Carátula, FechaCreacion, FechaUltimaModificacion, UsuarioUltimoCambio, Estado.
- `Datos de los tramites` — un trámite por bloque de 7 líneas: Id, ExpedienteId, Etiqueta, Contenido, FechaCreacion, FechaUltimaModificacion, UsuarioUltimoCambio.
Ejemplo del archivo `Datos de los expedientes` tras ejecutar el programa:
 
```
a3f1c2d4-8e5b-4a7f-9c1d-2b3e4f5a6b7c
Expediente sobre construcción ilegal - CORREGIDO
17/05/2026 10:32:14
17/05/2026 10:32:21
f7a8b9c0-1d2e-3f4a-5b6c-7d8e9f0a1b2c
EnNotificacion
```