using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AgregarExpedienteUseCase
{
    private IExpedienteRepository _repo;
    private IAutorizacionService _autorizacion;
    
    public AgregarExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion)
    {
        _repo = repo;
        _autorizacion = autorizacion;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no posee la autorización");
        }

        var caratula = new Caratula(request.Caratula);
        
        var expediente = new Expediente(caratula, request.IdUsuario);

        _repo.Agregar(expediente);

        return new AgregarExpedienteResponse(expediente.Id);
    }
}