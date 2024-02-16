using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioPedidos
    {
        Task CrearPedido(Pedidos pedidos);
        Task Finalizar(Pedidos pedidos);
        Task<IEnumerable<Pedidos>> ObtenerActivos();
        Task<Pedidos> ObtenerporId(int id_pedido);
    }
    public class RepositorioPedidos:IRepositorioPedidos
    {
        private readonly string connectionString;
        public RepositorioPedidos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CrearPedido(Pedidos pedidos)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"Insertar_Pedido", new {pedidos.mesa_pedido,pedidos.mesero_pedido,pedidos.estado_pedido,pedidos.fecha_pedido},
                commandType: System.Data.CommandType.StoredProcedure);
            pedidos.id_pedido = id;
        }
        public async Task<IEnumerable<Pedidos>> ObtenerActivos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Pedidos>(@"ObtenerPedidosActivos");
        }
        public async Task Finalizar(Pedidos pedidos)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Finalizar_Pedido", new {pedidos.id_pedido}, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<Pedidos> ObtenerporId(int id_pedido)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Pedidos>(@"SELECT id_pedido,mesa_pedido,mesero_pedido,estado_pedido,fecha_pedido
                                                                        FROM Pedidos
                                                                        WHERE id_pedido = @id_pedido", new { id_pedido });
        }
    }
}
