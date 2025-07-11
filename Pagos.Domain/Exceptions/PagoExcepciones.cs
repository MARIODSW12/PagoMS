
namespace Pagos.Domain.Exceptions
{
       public class IdPagoInvalido : Exception
        {
            public IdPagoInvalido() : base("El ID del pago no es válido.") { }
        }
        public class IdUsuarioInvalido : Exception
        {
            public IdUsuarioInvalido() : base("El ID del usuario no es válido.") { }
        }
        public class IdAuctionInvalido : Exception
        {
            public IdAuctionInvalido() : base("El ID de la subasta no es válido.") { }
        }
        public class IdMPagoInvalido : Exception
        {
            public IdMPagoInvalido() : base("El ID del método de pago no es válido.") { }
        }
        public class MontoInvalido : Exception
        {
            public MontoInvalido() : base("El monto del pago debe ser mayor a cero.") { }
        }
        public class ConexionBdPagoInvalida : Exception
        {
            public ConexionBdPagoInvalida() : base("La cadena de conexión de MongoDB no está definida.") { }
        }
        public class NombreBdInvalido : Exception
        {
            public NombreBdInvalido() : base("El nombre de la base de datos de MongoDB no está definido.") { }
        }
        public class ErrorConexionBd : Exception
        {
            public ErrorConexionBd() : base("No se pudo conectar a la base de datos.") { }
        }
}
