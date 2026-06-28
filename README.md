# SGE — Guía de Prueba de Endpoints desde Scalar

Este documento detalla el **orden recomendado** para probar los endpoints de la API del
Sistema de Gestión de Expedientes (SGE) usando la interfaz de **Scalar**, junto con las
**credenciales** de los usuarios precargados por el sistema al iniciar la aplicación.

---

## 1. Cómo levantar la API y acceder a Scalar

Desde la raíz de la solución:

```bash
dotnet run --project SGE.WebApi
```

La aplicación levanta (perfil `https`, ver `Properties/launchSettings.json`) en:

- `https://localhost:7190`
- `http://localhost:5199`

La documentación interactiva de Scalar está disponible en:

```
https://localhost:7190/scalar/v1
```

> Al iniciar, `InicializadorBD` crea automáticamente la base SQLite (`Sge.sqLite`) y la
> precarga con un administrador y dos usuarios de prueba **solo si la tabla de usuarios
> está vacía**. Si ya existe una base con datos, no se vuelve a sembrar.

---

## 2. Credenciales precargadas (seed)

Estas son las credenciales reales definidas en `SGE.Infraestructura/InicializadorBD.cs`:

| Rol | Nombre | Correo electrónico | Contraseña | Permisos |
|---|---|---|---|---|
| **Administrador** | Administrador Principal | `admin@sge.com` | `admin123` | Todos (es administrador, `EsAdministrador = true`) |
| **Usuario de prueba 1** | Usuario Prueba 1 | `prueba1@sge.com` | `123456` | `ExpedienteAlta`, `TramiteAlta` |
| **Usuario de prueba 2** | Usuario Prueba 2 | `prueba2@sge.com` | `123456` | Ninguno (sirve para probar el camino de error "sin autorización") |

> Las contraseñas se almacenan hasheadas en la base; las de la tabla de arriba son las
> que hay que escribir en el endpoint de login, en texto plano.
> Nota: En varios endpoints se requiere el campo usuarioUltimoCambio o idUsuario. Este valor corresponde al ID del usuario con el que estás logueado actualmente. Para obtenerlo, hacé GET
> /api/usuarios con el token de administrador y copiá el ID del usuario correspondiente.

---

## 3. Permiso requerido por cada operación

Antes de probar, conviene tener claro qué permiso exige cada endpoint protegido (definido
en cada caso de uso de `SGE.Aplicacion`):

| Endpoint | Permiso requerido |
|---|---|
| `POST /api/expedientes` | `ExpedienteAlta` |
| `PUT /api/expedientes/{id}/caratula` | `ExpedienteModificacion` |
| `PUT /api/expedientes/{id}/estado` | `ExpedienteModificacion` |
| `DELETE /api/expedientes/{id}` | `ExpedienteBaja` |
| `POST /api/expedientes/{expedienteId}/tramites` | `TramiteAlta` |
| `PUT /api/expedientes/{expedienteId}/tramites/{tramiteId}` | `TramiteModificacion` |
| `DELETE /api/expedientes/{expedienteId}/tramites/{tramiteId}` | `TramiteBaja` |
| `DELETE /api/usuarios/{id}` | Ser administrador |
| `PUT /api/usuarios/{id}/permisos` | Ser administrador |
| `GET /api/usuarios` | Ser administrador |

Regla especial (`AutorizacionService`): un usuario con permiso `ExpedienteBaja` también
queda habilitado automáticamente para `TramiteBaja`. El **administrador** tiene acceso
total a todo, sin necesidad de permisos individuales.

### Códigos HTTP devueltos según el tipo de excepción

Confirmado en `SGE.WebApi/Middlewares/ManejadorExcepciones.cs`:

| Excepción | Código HTTP |
|---|---|
| `DominioException` (validación de negocio) | **400 Bad Request** |
| `AutorizacionException` (sin permiso / credenciales no válidas en login) | **403 Forbidden** |
| `EntidadNoEncontradaException` (recurso no existe) | **404 Not Found** |
| Falta de token JWT o token inválido | **401 Unauthorized** (lo genera el middleware de autenticación, antes de llegar a `ManejadorExcepciones`) |
| Cualquier otra excepción no controlada | 500 Internal Server Error |

---

## 4. Orden recomendado de prueba en Scalar

### Paso 0 — Probar sin token (caso negativo)
Antes de loguearte, probá llamar a `GET /api/expedientes` sin Authorization.
**Esperado:** `401 Unauthorized`. Este código lo devuelve el middleware de autenticación
JWT de ASP.NET Core (no pasa por el manejador de excepciones de la aplicación), por eso
es el único caso de toda la guía con `401`.

### Paso 1 — Registro de un nuevo usuario (`POST /api/usuarios/registro`)
Endpoint anónimo (`AllowAnonymous`). Usalo para crear un usuario adicional propio, por
ejemplo:

```json
{
  "correoElectronico": "tester@sge.com",
  "nombre": "Usuario Tester",
  "contrasena": "Tester123"
}
```

**Esperado:** `201 Created` con el `Id` del nuevo usuario. Este usuario nace **sin
permisos y sin ser administrador**, igual que "Usuario Prueba 2".

### Paso 2 — Login como administrador (`POST /api/usuarios/login`)
```json
{
  "correoElectronico": "admin@sge.com",
  "contrasena": "admin123"
}
```

**Esperado:** `200 OK` con un `token` JWT. Copiá ese token y cargalo en Scalar con el
botón de **Authorization → Bearer Token**, para que las siguientes llamadas vayan
autenticadas como administrador.

### Paso 3 — Listar usuarios (`GET /api/usuarios`) — solo admin
Con el token de administrador activo, probá listar todos los usuarios.
**Esperado:** `200 OK` con el listado completo (admin + prueba1 + prueba2 + el que
registraste en el paso 1).

> Probá repetir este paso logueado como `prueba1@sge.com` o `prueba2@sge.com`:
> **esperado `403 Forbidden`**, ya que el caso de uso lanza `AutorizacionException`
> al no ser administradores (confirmado en `ManejadorExcepciones.cs`).

### Paso 4 — Asignar permisos al usuario nuevo (`PUT /api/usuarios/{id}/permisos`)
Como administrador, asignale al usuario creado en el paso 1 el permiso `ExpedienteAlta`:

```json
{
  "idOrigen": "<ID del usuario administrador logueado>",
  "idAEditar": "<ID del usuario al que se le asigna el permiso>",
  "nuevoPermiso": "ExpedienteAlta",
  "asignar": true
}
```
**Esperado:** `204 No Content`.

### Paso 5 — Login como Usuario Prueba 1 (`POST /api/usuarios/login`)
```json
{
  "correoElectronico": "prueba1@sge.com",
  "contrasena": "123456"
}
```
Reemplazá el Bearer Token en Scalar por este nuevo token (usuario con permisos
`ExpedienteAlta` y `TramiteAlta`, pero no administrador).

### Paso 6 — Crear un expediente (`POST /api/expedientes`)
```json
{
  "caratula": "Expediente de prueba - construcción ilegal"
}
```
**Esperado:** `201 Created`, con el `id` del expediente. **Guardá ese `id`**, se usa en
los pasos siguientes. Estado inicial: `RecienIniciado`.

### Paso 7 — Listar expedientes (`GET /api/expedientes`)
**Esperado:** `200 OK` con el expediente recién creado.

### Paso 8 — Obtener expediente con detalle (`GET /api/expedientes/{id}`)
Usando el `id` del paso 6.
**Esperado:** `200 OK` con los datos del expediente (sin trámites todavía).

### Paso 9 — Agregar un trámite (`POST /api/expedientes/{expedienteId}/tramites`)
```json
{
  "usuarioUltimoCambio": "<ID del usuario logueado>",
  "expedienteId": "<ID del expediente creado en el paso 6>",
  "etiqueta": "EscritoPresentado",
  "contenido": "Se presenta el escrito inicial"
}
```
**Esperado:** `201 Created` con el `id` del trámite. Guardalo también.

### Paso 10 — Agregar un segundo trámite con cambio de estado automático
```json
{
  "usuarioUltimoCambio": "<ID del usuario logueado>",
  "expedienteId": "<ID del expediente creado en el paso 6>",
  "etiqueta": "PaseAEstudio",
  "contenido": "Pase a estudio del departamento técnico"
}
```
**Esperado:** `201 Created`. Al ser el trámite `PaseAEstudio`, el sistema cambia
automáticamente el estado del expediente. Confirmalo repitiendo el **Paso 8**: el estado
debería ser ahora `ParaResolver`.

### Paso 11 — Listar trámites del expediente (`GET /api/expedientes/{expedienteId}/tramites`)
**Esperado:** `200 OK` con los dos trámites creados.

### Paso 12 — Intentar modificar un trámite sin el permiso adecuado (caso negativo)
Seguís logueado como `prueba1@sge.com` (que **no** tiene `TramiteModificacion`).
Probá `PUT /api/expedientes/{expedienteId}/tramites/{tramiteId}`:
```json
{
  "idTramite": "<ID del trámite>",
  "nuevoContenido": "Intento de modificación",
  "idUsuario": "<ID del usuario logueado>"
}
```
**Esperado:** `403 Forbidden` (`AutorizacionException`), porque `prueba1` solo
tiene `ExpedienteAlta` y `TramiteAlta`.

### Paso 13 — Volver a loguearse como administrador y otorgar el permiso faltante
Repetí el **Paso 2** (login admin) y luego `PUT /api/usuarios/{idDePrueba1}/permisos`:
```json
{
  "permiso": "TramiteModificacion",
  "asignar": true
}
```

### Paso 14 — Reintentar la modificación del trámite (ahora con permiso)
Logueate de nuevo como `prueba1@sge.com` (Paso 5) y repetí el **Paso 12**.
**Esperado:** `200 OK`, el contenido del trámite queda actualizado.

### Paso 15 — Modificar la carátula del expediente (`PUT /api/expedientes/{id}/caratula`)
Requiere `ExpedienteModificacion`. Si `prueba1` no lo tiene, otorgáselo como admin (igual
que en el paso 13) y luego probá:
```json
{
  {
  "id": "<ID del expediente>",
  "nuevaCaratula": "Expediente de prueba - CORREGIDO",
  "usuarioUltimoCambio": "<ID del usuario logueado>"
}
```
**Esperado:** `200 OK`.

### Paso 16 — Cambiar el estado manualmente (`PUT /api/expedientes/{id}/estado`)
```json
{
  "id": "<ID del expediente>",
  "nuevoEstado": "EnNotificacion",
  "usuarioUltimoCambio": "<ID del usuario logueado>"
}
```
**Esperado:** `200 OK`. El estado se actualiza sin pasar por un trámite.

### Paso 17 — Eliminar un trámite (`DELETE /api/expedientes/{expedienteId}/tramites/{tramiteId}`)
Requiere `TramiteBaja` (o `ExpedienteBaja`, que la habilita indirectamente).
**Esperado:** `200 OK` si el usuario tiene el permiso; de lo contrario, `403 Forbidden` —
útil para verificar la regla especial mencionada en la sección 3.

### Paso 18 — Probar "mis datos" (`PUT /api/usuarios/mis-datos`)
Logueado como cualquier usuario, probá modificar tu propio nombre y/o contraseña:
```json
{
  "id": "<ID del usuario logueado>",
  "contrasena": "NuevaClave456",
  "nombre": "Usuario Prueba 1 (editado)"
}
```
**Esperado:** `204 No Content`. Verificá luego que el login con la nueva contraseña
funcione.

### Paso 19 — Eliminar un expediente en cascada (`DELETE /api/expedientes/{id}`)
Logueado como administrador o como un usuario con `ExpedienteBaja`.
**Esperado:** `200 OK`. Esto elimina primero los trámites asociados y luego el
expediente. Confirmalo repitiendo el **Paso 7**: el expediente ya no debe aparecer.

### Paso 20 — Eliminar un usuario (`DELETE /api/usuarios/{idAEliminar}`) — solo admin
Logueado como administrador, eliminá el usuario creado en el **Paso 1**.
**Esperado:** `204 No Content`.

### Paso 21 — Caso de error: expediente inexistente
Probá `GET /api/expedientes/{guid-inventado}` con un GUID que no exista.
**Esperado:** `404 Not Found` (`EntidadNoEncontradaException`).

### Paso 22 — Caso de error: login con credenciales inválidas
Hay dos variantes, y devuelven códigos distintos (`LoginUseCase.cs`):

**a) Correo que no existe:**
```json
{
  "correoElectronico": "noexiste@sge.com",
  "contrasena": "cualquiera"
}
```
**Esperado:** `403 Forbidden` (el caso de uso lanza `AutorizacionException`).

**b) Correo correcto, contraseña incorrecta:**
```json
{
  "correoElectronico": "admin@sge.com",
  "contrasena": "claveIncorrecta"
}
```
**Esperado:** `400 Bad Request` (el caso de uso lanza `DominioException`).

### Paso 23 — Caso de error: registrar un correo ya existente
```json
{
  "correoElectronico": "admin@sge.com",
  "nombre": "Otro Admin",
  "contrasena": "loquesea123"
}
```
**Esperado:** `400 Bad Request` (`DominioException`: el correo ya está registrado).
Confirmado en `RegistrarUsuarioUseCase.cs`.

---

## 5. Resumen rápido de credenciales para copiar y pegar

```
Administrador:
  correo: admin@sge.com
  contraseña: admin123

Usuario de prueba 1 (con permisos ExpedienteAlta y TramiteAlta):
  correo: prueba1@sge.com
  contraseña: 123456

Usuario de prueba 2 (sin permisos):
  correo: prueba2@sge.com
  contraseña: 123456
```
