using System;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Interfaces;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuariio, Permiso permiso);
}
