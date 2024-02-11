using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioPedidos
    {
        Task CrearPedido(Pedidos pedidos);
        Task<IEnumerable<Pedidos>> ObtenerActivos();
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
            return await connection.QueryAsync<Pedidos>(@"SELECT Mesas.num_mesa as mesa_pedido, Empleados.doc_empleado as mesero_pedido, estado_pedido
                                                        FROM Pedidos
                                                        JOIN Mesas
                                                        ON Pedidos.mesa_pedido = Mesas.id_mesa
                                                        JOIN Empleados
                                                        ON Pedidos.mesero_pedido = Empleados.id_empleado
                                                        WHERE estado_pedido = 'Activo'");
        }
    }
}
