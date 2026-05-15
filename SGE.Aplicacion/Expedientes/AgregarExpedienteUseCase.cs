using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AgregarExpedienteUseCase
{
    private readonly IExpedienteRepository _repo;
    private readonly IAutorizacionService _autorizacion;
    public AgregarExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion)
    {
        _repo = repo;
        _autorizacion = autorizacion;
    }

    public  AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteAlta))
        {
           throw new AutorizacionException("El usuario no posee la autorizacion");
        }
        
        var nuevoExpediente = new Expediente(request.Caratula,request.UsuarioUltimoCambio); 

        _repo.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }



}