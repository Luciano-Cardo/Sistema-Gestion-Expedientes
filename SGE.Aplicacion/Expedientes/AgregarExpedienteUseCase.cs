using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

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
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteAlta)) 
            throw new AutorizacionException("El usuario no posee la autorización para crear expedientes");
        
        var caratula = new Caratula(request.Caratula);

        var nuevoExpediente = new Expediente(caratula,request.UsuarioUltimoCambio); 

        _repo.Agregar(nuevoExpediente);

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}