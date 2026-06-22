using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Comun;

namespace SGE.WebApi.Middlewares;

public class ManejadorExcepciones : IExceptionHandler
{
    private readonly ILogger<ManejadorExcepciones> _registro;

    public ManejadorExcepciones(ILogger<ManejadorExcepciones> registro)
    {
        this.registro = registro;
    }

    public async ValueTask<bool> ManejarError(HttpContext httpContext,Exception exception,CancellationToken cancellationToken)
    {
        _registro.LogError(exception, "Ocurrió un error no controlado: {Message}", exception.Message);

        var codigoEstado = StatusCodes.Status500InternalServerError;
        var titulo = "Error Interno del Servidor";
        var detalle = exception.Message;

        if (exception is DominioException)
        {
            codigoEstado = StatusCodes.Status400BadRequest;
            titulo = "Error de Validación de Negocio";
        }
        else if (exception is AutorizacionException)
        {
            codigoEstado = StatusCodes.Status403Forbidden;
            titulo = "Acceso Denegado";
        }
        else if (exception is EntidadNoEncontradaException)
        {
            codigoEstado = StatusCodes.Status404NotFound;
            titulo = "Recurso no encontrado";
        }

        var detalleProblema = new ProblemDetails
        {
            Status = codigoEstado,
            Title = titulo,
            Detail = detalle,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = codigoEstado;
        await httpContext.Response.WriteAsJsonAsync(detalleProblema, cancellationToken);

        return true;
    }   
}