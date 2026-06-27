using System.Security.Claims;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.WebApi.Tramites;

public static class TramitesEndpoints
{
    public static IEndpointRouteBuilder MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/expedientes/{expedienteId:guid}/tramites").WithTags("Tramites");

        grupo.MapGet("/", (
            Guid expedienteId,
            ListarTramitesPorExpedienteUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ListarTramitesPorExpedienteRequest(expedienteId));
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapPost("/", (
            Guid expedienteId,
            AgregarTramiteBody body,
            ClaimsPrincipal user,
            AgregarTramiteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var contenido = new ContenidoTramite(body.Contenido);
            var request = new AgregarTramiteRequest(userId, expedienteId, body.Etiqueta, contenido);
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/expedientes/{expedienteId}/tramites/{response.Id}", response);
        }).RequireAuthorization();

        grupo.MapPut("/{tramiteId:guid}", (
            Guid expedienteId,
            Guid tramiteId,
            ModificarTramiteBody body,
            ClaimsPrincipal user,
            ModificarTramiteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var nuevoContenido = new ContenidoTramite(body.NuevoContenido);
            var request = new ModificarTramiteRequest(tramiteId, nuevoContenido, userId);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapDelete("/{tramiteId:guid}", (
            Guid expedienteId,
            Guid tramiteId,
            ClaimsPrincipal user,
            EliminarTramiteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var response = useCase.Ejecutar(new EliminarTramiteRequest(userId, tramiteId));
            return Results.Ok(response);
        }).RequireAuthorization();

        return app;
    }
}

public record AgregarTramiteBody(EtiquetaTramite Etiqueta, string Contenido);
public record ModificarTramiteBody(string NuevoContenido);
