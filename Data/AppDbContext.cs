using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Vehiculo> Vehiculos { get; set; }
		public DbSet<Ubicacion> Ubicaciones { get; set; }
		public DbSet<Reservacion> Reservaciones { get; set; }
		public DbSet<Mensaje> Mensajes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configuración de Reservacion con múltiples FK a Ubicacion
			modelBuilder.Entity<Reservacion>()
				.HasOne(r => r.UbicacionRecogida)
				.WithMany(u => u.ReservacionesRecogida)
				.HasForeignKey(r => r.UbicacionRecogidaId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Reservacion>()
				.HasOne(r => r.UbicacionDevolucion)
				.WithMany(u => u.ReservacionesDevolucion)
				.HasForeignKey(r => r.UbicacionDevolucionId)
				.OnDelete(DeleteBehavior.Restrict);

			// Índice único para email de usuario
			modelBuilder.Entity<Usuario>()
				.HasIndex(u => u.Email)
				.IsUnique();

			// Datos semilla
			SeedData(modelBuilder);
		}

		private void SeedData(ModelBuilder modelBuilder)
		{
			// Usuarios de prueba
			modelBuilder.Entity<Usuario>().HasData(
				new Usuario
				{
					UsuarioId = 1,
					Nombre = "Admin",
					Email = "admin@kiasrent.com",
					Password = "admin123",
					Telefono = "809-555-0001",
					Rol = "Admin",
					FechaRegistro = DateTime.UtcNow
				},
				new Usuario
				{
					UsuarioId = 2,
					Nombre = "Juan Pérez",
					Email = "juan@email.com",
					Password = "1234",
					Telefono = "809-555-0002",
					Rol = "Cliente",
					FechaRegistro = DateTime.UtcNow
				},
				new Usuario
				{
					UsuarioId = 3,
					Nombre = "María García",
					Email = "maria@email.com",
					Password = "1234",
					Telefono = "809-555-0003",
					Rol = "Cliente",
					FechaRegistro = DateTime.UtcNow
				}
			);

			// Ubicaciones de prueba
			modelBuilder.Entity<Ubicacion>().HasData(
				new Ubicacion
				{
					UbicacionId = 1,
					Nombre = "Aeropuerto Internacional JFK",
					Direccion = "Terminal 1, Aeropuerto JFK",
					Activa = true
				},
				new Ubicacion
				{
					UbicacionId = 2,
					Nombre = "Centro de Manhattan, NY",
					Direccion = "5th Avenue, Manhattan",
					Activa = true
				},
				new Ubicacion
				{
					UbicacionId = 3,
					Nombre = "Oficina Central, Av. Principal 123",
					Direccion = "Av. Principal #123, Centro",
					Activa = true
				},
				new Ubicacion
				{
					UbicacionId = 4,
					Nombre = "Sucursal Plaza Norte",
					Direccion = "Plaza Norte, Local 45",
					Activa = true
				}
			);

			// Vehículos de prueba
			modelBuilder.Entity<Vehiculo>().HasData(
				new Vehiculo
				{
					VehiculoId = 1,
					Modelo = "Kia Sportage",
					Descripcion = "SUV compacto ideal para familias. Cuenta con 5 puertas, transmisión automática y aire acondicionado.",
					Categoria = "SUV",
					Asientos = 5,
					Transmision = "Automatic",
					PrecioPorDia = 75.00,
					ImagenUrl = "https://images.unsplash.com/photo-1619682817481-e994891cd1f5?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				},
				new Vehiculo
				{
					VehiculoId = 2,
					Modelo = "Kia Sorento",
					Descripcion = "SUV grande con capacidad para 7 pasajeros. Perfecto para viajes largos.",
					Categoria = "SUV",
					Asientos = 7,
					Transmision = "Automatic",
					PrecioPorDia = 95.00,
					ImagenUrl = "https://images.unsplash.com/photo-1606611013016-969c19ba27bb?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				},
				new Vehiculo
				{
					VehiculoId = 3,
					Modelo = "Kia Rio",
					Descripcion = "Sedán compacto económico. Ideal para ciudad con excelente rendimiento de combustible.",
					Categoria = "Sedan",
					Asientos = 5,
					Transmision = "Manual",
					PrecioPorDia = 45.00,
					ImagenUrl = "https://images.unsplash.com/photo-1583121274602-3e2820c69888?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				},
				new Vehiculo
				{
					VehiculoId = 4,
					Modelo = "Kia EV6",
					Descripcion = "Vehículo 100% eléctrico con autonomía de 500km. Tecnología de punta.",
					Categoria = "Electric",
					Asientos = 5,
					Transmision = "Automatic",
					PrecioPorDia = 120.00,
					ImagenUrl = "https://images.unsplash.com/photo-1617788138017-80ad40651399?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				},
				new Vehiculo
				{
					VehiculoId = 5,
					Modelo = "Kia K5",
					Descripcion = "Sedán premium con acabados de lujo y sistema de navegación.",
					Categoria = "Sedan",
					Asientos = 5,
					Transmision = "Automatic",
					PrecioPorDia = 85.00,
					ImagenUrl = "https://images.unsplash.com/photo-1605559424843-9e4c228bf1c2?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				},
				new Vehiculo
				{
					VehiculoId = 6,
					Modelo = "Kia Telluride",
					Descripcion = "SUV de lujo con 8 asientos. El más espacioso de la familia KIA.",
					Categoria = "SUV",
					Asientos = 8,
					Transmision = "Automatic",
					PrecioPorDia = 150.00,
					ImagenUrl = "https://images.unsplash.com/photo-1549317661-bd32c8ce0db2?w=800",
					Disponible = true,
					FechaIngreso = DateTime.UtcNow
				}
			);

			// Reservaciones de prueba
			modelBuilder.Entity<Reservacion>().HasData(
				new Reservacion
				{
					ReservacionId = 1,
					UsuarioId = 2,
					VehiculoId = 1,
					FechaRecogida = DateTime.UtcNow.AddDays(5),
					HoraRecogida = "10:00",
					FechaDevolucion = DateTime.UtcNow.AddDays(8),
					HoraDevolucion = "10:00",
					UbicacionRecogidaId = 1,
					UbicacionDevolucionId = 2,
					Estado = "Confirmada",
					Subtotal = 225.00,
					Impuestos = 40.50,
					Total = 265.50,
					CodigoReserva = "KR-ABC123",
					FechaCreacion = DateTime.UtcNow
				},
				new Reservacion
				{
					ReservacionId = 2,
					UsuarioId = 3,
					VehiculoId = 4,
					FechaRecogida = DateTime.UtcNow.AddDays(10),
					HoraRecogida = "14:00",
					FechaDevolucion = DateTime.UtcNow.AddDays(12),
					HoraDevolucion = "14:00",
					UbicacionRecogidaId = 3,
					UbicacionDevolucionId = 3,
					Estado = "Pendiente",
					Subtotal = 240.00,
					Impuestos = 43.20,
					Total = 283.20,
					CodigoReserva = "KR-DEF456",
					FechaCreacion = DateTime.UtcNow
				}
			);

			// Mensajes de prueba
			modelBuilder.Entity<Mensaje>().HasData(
				new Mensaje
				{
					MensajeId = 1,
					UsuarioId = 2,
					Asunto = "Problema con la reserva #5821",
					Contenido = "Mensaje de ejemplo sobre el problema encontrado.",
					FechaCreacion = DateTime.UtcNow.AddDays(-2),
					Leido = false
				},
				new Mensaje
				{
					MensajeId = 2,
					UsuarioId = 3,
					Asunto = "Duda sobre el seguro del vehículo",
					Contenido = "Quisiera saber qué cubre exactamente el seguro.",
					FechaCreacion = DateTime.UtcNow.AddDays(-1),
					Leido = false
				}
			);
		}
	}
}