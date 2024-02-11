using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioEmpleados
    {
        Task Actualizar(Empleados empleados);
        Task Crear(Empleados empleados);
        Task Desactivar(Empleados empleados);
        Task<bool> Existe(string doc_empleado);
        Task<IEnumerable<Empleados>> Obtener();
        Task<Empleados> ObtenerporDoc(string doc_empleado);
        Task<Empleados> ObtenerporId(int id_empleado);
        Task<IEnumerable<Empleados>> ObtenerTodos();
    }
    public class RepositorioEmpleados:IRepositorioEmpleados
    {
        private readonly string connectionString;

        public RepositorioEmpleados(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Empleados empleados)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Empleados(tipodoc_empleado,doc_empleado,cargo_empleado,primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,password,fecha_ingreso,estado_empleado)
                                                            VALUES(@tipodoc_empleado,@doc_empleado,@cargo_empleado,@primer_nombre,@segundo_nombre,@primer_apellido,@segundo_apellido,@password,@fecha_ingreso,@estado_empleado)
                                                            SELECT SCOPE_IDENTITY();;", empleados);
            empleados.id_empleado = id;
        }
        public async Task<bool> Existe(string doc_empleado)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                        FROM Empleados
                                                                        WHERE doc_empleado = @doc_empleado;", new {doc_empleado});
            return existe == 1;
        }
        public async Task<IEnumerable<Empleados>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Empleados>(@"SELECT id_empleado, tipodoc_empleado,doc_empleado,cargo_empleado,primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,password,fecha_ingreso,estado_empleado
                                                            FROM Empleados
                                                            WHERE estado_empleado = 'Activo'");
        }
        public async Task Actualizar(Empleados empleados)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Empleados
                                            SET cargo_empleado = @cargo_empleado,
	                                            primer_nombre = @primer_nombre,
	                                            segundo_nombre = @segundo_nombre,
	                                            primer_apellido = @primer_apellido,
	                                            segundo_apellido = @segundo_apellido
                                            WHERE doc_empleado = @doc_empleado", empleados);
        }
        public async Task<Empleados>ObtenerporDoc(string doc_empleado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Empleados>(@"SELECT id_empleado, tipodoc_empleado,doc_empleado,cargo_empleado,primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,password,fecha_ingreso,estado_empleado,fecha_egreso
                                                                        FROM Empleados
                                                                        WHERE doc_empleado=@doc_empleado", new { doc_empleado });
        }
        public async Task Desactivar(Empleados empleados)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Empleados
                                            SET estado_empleado = @estado_empleado,
	                                            fecha_egreso = @fecha_egreso
                                            WHERE doc_empleado = @doc_empleado", empleados);
        }
        public async Task<IEnumerable<Empleados>> ObtenerTodos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Empleados>(@"SELECT id_empleado, tipodoc_empleado,doc_empleado,cargo_empleado,primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,password,fecha_ingreso,estado_empleado,fecha_egreso
                                                            FROM Empleados");
        }
        public async Task<Empleados> ObtenerporId(int id_empleado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Empleados>(@"SELECT id_empleado, tipodoc_empleado,doc_empleado,cargo_empleado,primer_nombre,segundo_nombre,primer_apellido,segundo_apellido,password,fecha_ingreso,estado_empleado,fecha_egreso
                                                                        FROM Empleados
                                                                        WHERE id_empleado=@id_empleado", new {id_empleado });
        }
    }
}
