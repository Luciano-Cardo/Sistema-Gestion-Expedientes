using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Entidades;

namespace SGE.Aplicacion.CasosDeUso
{
    public class RegistrarUsuarioUseCase 
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHashService _hashService; 

        public RegistrarUsuarioUseCase(IUsuarioRepository usuarioRepository, IHashService hashService)
        {
            _usuarioRepository = usuarioRepository;
            _hashService = hashService;
        }

        public void Ejecutar(Usuario usuario, string contrasenaPlana)
        {
            usuario.ContrasenaHash = _hashService.HashPassword(contrasenaPlana);
            _usuarioRepository.Agregar(usuario);
        }
    }
}