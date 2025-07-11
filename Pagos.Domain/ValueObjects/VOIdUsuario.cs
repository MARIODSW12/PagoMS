
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOIdUsuario
    {
        public string IdUsuario { get; private set; }
        public VOIdUsuario(string idUsuario)
        {
            if (string.IsNullOrWhiteSpace(idUsuario))
                throw new IdUsuarioInvalido();

            if (!Guid.TryParse(idUsuario, out _))
                throw new IdUsuarioInvalido();

            IdUsuario = idUsuario;
        }
    }
}
