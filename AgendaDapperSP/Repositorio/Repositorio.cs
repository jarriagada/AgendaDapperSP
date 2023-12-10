using AgendaDapperSP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AgendaDapperSP.Repositorio
{
    public class Repositorio : IRepositorio
    {
        /* CLASE _CONNECTION */
        private readonly IDbConnection _db;

        /* SE UTILIZA COMO INYECCION DE DEPENDENCIAS */
        public Repositorio(IConfiguration configuracion)
        {
            _db = new SqlConnection(configuracion.GetConnectionString("ConexionLocalDB"));
        }

        public Repositorio() { }

        public Cliente ActualizarCliente(Cliente cliente)
        {
            //var sql = "UPDATE Cliente SET Nombres=@Nombres, Apellidos=@Apellidos, Telefono=@Telefono, Email=@Email, Pais=@Pais "
            //    + " WHERE IdCliente =@IdCliente";
            //_db.Execute(sql, cliente);
            //return cliente;


            var parametros = new DynamicParameters();
            parametros.Add("@ClienteId", cliente.IdCliente, DbType.Int32);
            parametros.Add("@Nombres", cliente.Nombres);
            parametros.Add("@Apellidos", cliente.Apellidos);
            parametros.Add("@Telefono", cliente.Telefono);
            parametros.Add("@Email", cliente.Email);
            parametros.Add("@Pais", cliente.Pais);

            this._db.Execute("sp_ActualizarCliente", parametros, commandType: CommandType.StoredProcedure);
            return cliente;
        }

        public Cliente AgregarCliente(Cliente cliente) {
            //var sql = "INSERT INTO Cliente(Nombres, Apellidos, Telefono, Email, Pais, FechaCreacion)VALUES(@Nombres, @Apellidos, @Telefono, @Email, @Pais, @FechaCreacion)"
            //  + " SELECT CAST(SCOPE_IDENTITY() as int);";
            //var id = _db.Query<int>(sql, cliente).Single();
            //cliente.IdCliente = id;

            //configuracion DynamicParametrers
            var parametros = new DynamicParameters();
            parametros.Add("@ClienteId", 0, DbType.Int32, direction: ParameterDirection.Output);
            parametros.Add("@Nombres", cliente.Nombres);
            parametros.Add("@Apellidos", cliente.Apellidos);
            parametros.Add("@Telefono", cliente.Telefono);
            parametros.Add("@Email", cliente.Email);
            parametros.Add("@Pais", cliente.Pais);
            //parametros.Add("@FechaCreacion", cliente.FechaCreacion);

            this._db.Execute("sp_CrearCliente", parametros, commandType: CommandType.StoredProcedure);
            cliente.IdCliente = parametros.Get<int>("ClienteId");
            return cliente;
        }
            //var sql = "INSERT INTO Cliente(Nombres, Apellidos, Telefono, Email, Pais, FechaCreacion)VALUES(@Nombres, @Apellidos, @Telefono, @Email, @Pais, @FechaCreacion)"
            //+ " SELECT CAST(SCOPE_IDENTITY()  as int);";
            //var id = _db.Query<int>(sql, new
            //{
            //    cliente.Nombres,
            //    cliente.Apellidos,
            //    cliente.Telefono,
            //    cliente.Email,
            //    cliente.Pais,
            //    cliente.FechaCreacion
            //}).Single();

            //cliente.IdCliente = id;
            //return cliente;
           
        public void BorrarCliente(int id)
        {
            /* query Dapper */
            //var sql = "DELETE FROM CLIENTE WHERE IdCliente=@IdCliente";
            //_db.Execute(sql, new { @IdCliente = id });
            _db.Execute("sp_BorrarCliente", new { ClienteId = id }, commandType: CommandType.StoredProcedure);
        }

        public Cliente GetCliente(int id)
        {
            /* query Dapper */
            //var sql = "SELECT * FROM CLIENTE WHERE IdCliente=@IdCliente";
            //return _db.Query<Cliente>(sql, new { @IdCliente = id }).Single();
            return _db.Query<Cliente>("sp_GetClienteId", new { ClienteId = id }, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }

        public List<Cliente> GetClientes()
        {
            /* query Dapper */
            //var sql = "SELECT * FROM CLIENTE";
            return _db.Query<Cliente>("sp_GetClientes", commandType: CommandType.StoredProcedure).ToList();

        }
    }
}
