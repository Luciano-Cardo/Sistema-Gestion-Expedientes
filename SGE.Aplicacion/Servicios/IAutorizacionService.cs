using System;
using SGE.Dominio.Autorizacion;


namespace SGE.Aplicacion.Autorizacion;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
    //En el servicio de autorizaciones (IAutorizacionService),
    // se debe mantener la regla: El permisoExpedienteBaja implica
    // implícitamente contar con el permiso TramiteBaja.

}