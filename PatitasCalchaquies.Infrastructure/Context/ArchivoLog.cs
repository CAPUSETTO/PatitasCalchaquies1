using System.Text;

namespace PatitasCalchaquies.Infrastructure.Context
{
    public static class ArchivoLog
    {
        private static readonly string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog");

        public static void RegistrarErrores(Exception error)
        {
            try
            {
                DateTime fechaHora = DateTime.Now;
                string nombreCarpeta = fechaHora.ToString("yyyy-MM-dd");
                string directorioDia = Path.Combine(rutaBase, nombreCarpeta);

                Directory.CreateDirectory(directorioDia);

                string nombreArchivo = $"{fechaHora:yyyy-MM-dd_HH-mm-ss-fff}.txt";
                string rutaArchivo = Path.Combine(directorioDia, nombreArchivo);

                using (StreamWriter writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
                {
                    writer.WriteLine($"Fecha y hora: {fechaHora:dd/MM/yyyy HH:mm:ss.fff}");
                    writer.WriteLine($"Tipo de error: {error.GetType().Name}");
                    writer.WriteLine("Mensaje: ");
                    writer.WriteLine(error.Message);
                    writer.WriteLine("Pila de error:");
                    writer.WriteLine(error.StackTrace);

                    if (error.InnerException != null)
                    {
                        writer.WriteLine();
                        writer.WriteLine("InnerException: ");
                        writer.WriteLine(error.InnerException.ToString());
                    }
                    writer.WriteLine("=====================================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar el error en el log: {ex.Message}");
            }
        }
    }
}