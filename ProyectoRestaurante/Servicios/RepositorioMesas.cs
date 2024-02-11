using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioMesas
    {
        Task Actualizar(Mesas mesas);
        Task Borrar(int id_mesa);
        Task Crear(Mesas mesas);
        Task<bool> Existe(int num_mesa);
        Task<IEnumerable<Mesas>> Obtener();
        Task<IEnumerable<Mesas>> ObtenerDispo();
        Task<Mesas> ObtenerporId(int id_mesa);
        Task<Mesas> ObtenerporNum(int num_mesa);
    }
    public class RepositorioMesas:IRepositorioMesas
    {
        private readonly string connectionString;

        public RepositorioMesas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Mesas mesas)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Mesas(num_mesa,estado_mesa,capacidad_mesa)
                                                    VALUES (@num_mesa,@estado_mesa,@capacidad_mesa)
                                                    SELECT SCOPE_IDENTITY();",mesas);
            mesas.id_mesa = id;
        }
        public async Task<bool> Existe(int num_mesa)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                        FROM Mesas
                                                                        WHERE num_mesa = @num_mesa",new {num_mesa});
            return existe == 1;
        }

        public async Task <IEnumerable<Mesas>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Mesas>(@"SELECT id_mesa,num_mesa,estado_mesa,capacidad_mesa
                                                 FROM Mesas" );
        }
        public async Task<IEnumerable<Mesas>> ObtenerDispo()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Mesas>(@"SELECT id_mesa,num_mesa,estado_mesa,capacidad_mesa
                                                 FROM Mesas
                                                WHERE estado_mesa='Disponible'");
        }
        public async Task Actualizar(Mesas mesas)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Mesas
                                            SET capacidad_mesa = @capacidad_mesa
                                                WHERE id_mesa= @id_mesa", mesas);
        }
        public async Task<Mesas>ObtenerporId(int id_mesa)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Mesas>(@"SELECT id_mesa,num_mesa,
                                                                    estado_mesa,
                                                                    capacidad_mesa
                                                                    FROM Mesas
                                                                    WHERE id_mesa=@id_mesa",
                                                                    new {id_mesa});
        }
        public async Task<Mesas> ObtenerporNum(int num_mesa)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Mesas>(@"SELECT id_mesa,num_mesa,
                                                                    estado_mesa,
                                                                    capacidad_mesa
                                                                    FROM Mesas
                                                                    WHERE num_mesa=@num_mesa",
                                                                    new { num_mesa });
        }
        public async Task Borrar(int id_mesa)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Mesas WHERE id_mesa = @id_mesa",new {id_mesa});
        }
    }
}
