using System.Security.Claims;
using SGE.Aplicacion.Usuarios;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Autorizacion;

namespace SGE.WebApi.Usuarios;

public static class UsuariosEndpoints
{
    public static IEndpointRouteBuilder MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/usuarios").WithTags("Usuarios");

        grupo.MapPost("/registro", (
            RegistrarUsuarioRequest body,
            RegistrarUsuarioUseCase useCase) =>
        {
            var response = useCase.Ejecutar(body);
            return Results.Created($"/api/usuarios/{response.Id}", response);
        }).AllowAnonymous();

        grupo.MapPost("/login", (
            LoginUsuarioRequest body,
            LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(body);
            return Results.Ok(response);
        }).AllowAnonymous();

        grupo.MapPut("/mis-datos", (
            ModificarMisDatosBody body,
            ClaimsPrincipal user,
            ModificarMisDatosUseCase useCase) =>
        {
            var userId = user.ObtenerUserId(); 
            var request = new ModificarMisDatosRequest(userId, body.Contrasena, body.Nombre);
            useCase.Ejecutar(userId, request);
            return Results.NoContent();
        }).RequireAuthorization();

        grupo.MapGet("/", (
            ClaimsPrincipal user,
            ListarUsuariosUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var response = useCase.Ejecutar(new ListarUsuariosRequest(userId));
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapDelete("/{idAEliminar:guid}", (
            Guid idAEliminar,
            ClaimsPrincipal user,
            EliminarUsuarioUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            useCase.Ejecutar(new EliminarUsuarioRequest(userId, idAEliminar));
            return Results.NoContent();
        }).RequireAuthorization();

        grupo.MapPut("/{idAEditar:guid}/permisos", (
            Guid idAEditar,
            ModificarPermisosBody body,
            ClaimsPrincipal user,
            ModificarPermisosUsuarioUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var request = new ModificarPermisosUsuarioRequest(userId, idAEditar, body.Permiso, body.Asignar);
            useCase.Ejecutar(request);
            return Results.NoContent();
        }).RequireAuthorization();

        return app;
    }
}

public record ModificarMisDatosBody(string Nombre, string Contrasena);
public record ModificarPermisosBody(Permiso Permiso, bool Asignar);
