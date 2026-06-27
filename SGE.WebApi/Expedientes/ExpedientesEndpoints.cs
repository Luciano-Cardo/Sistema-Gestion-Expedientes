using System.Security.Claims;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

namespace SGE.WebApi.Expedientes;

public static class ExpedientesEndpoints
{
    public static IEndpointRouteBuilder MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/expedientes").WithTags("Expedientes");

        grupo.MapGet("/", (
            ListarExpedientesUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ListarExpedientesRequest());
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapGet("/{id:guid}", (
            Guid id,
            ObtenerExpedienteConDetalleUseCase useCase) =>
        {
            var response = useCase.Ejecutar(new ObtenerExpedienteConDetalleRequest(id));
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapPost("/", (
            AgregarExpedienteBody body,
            ClaimsPrincipal user,
            AgregarExpedienteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var request = new AgregarExpedienteRequest(body.Caratula, userId);
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/expedientes/{response.id}", response);
        }).RequireAuthorization();

        grupo.MapPut("/{id:guid}/caratula", (
            Guid id,
            ModificarCaratulaBody body,
            ClaimsPrincipal user,
            ModificarCaratulaExpedienteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var request = new ModificarCaratulaExpedienteRequest(id, body.NuevaCaratula, userId);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapPut("/{id:guid}/estado", (
            Guid id,
            CambiarEstadoBody body,
            ClaimsPrincipal user,
            CambiarEstadoExpedienteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var request = new CambiarEstadoExpedienteRequest(id, body.NuevoEstado, userId);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        }).RequireAuthorization();

        grupo.MapDelete("/{id:guid}", (
            Guid id,
            ClaimsPrincipal user,
            EliminarExpedienteUseCase useCase) =>
        {
            var userId = user.ObtenerUserId();
            var response = useCase.Ejecutar(new EliminarExpedienteRequest(id, userId));
            return Results.Ok(response);
        }).RequireAuthorization();

        return app;
    }
}

public record AgregarExpedienteBody(string Caratula);
public record ModificarCaratulaBody(string NuevaCaratula);
public record CambiarEstadoBody(EstadoExpediente NuevoEstado);
