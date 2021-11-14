namespace x509_storage.Certificate
{
    /// <summary>
    /// Define los datos de un certificado que se cargará al almacenamiento de la aplicación
    /// </summary>
    public class CertificateInfo
    {
        /// <summary>
        /// Nombre de indentificación global para el certificado
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Indica si el certificado está instalado en el equipo
        /// </summary>
        public bool IsInstalled { get; set; }
        /// <summary>
        /// Ruta de acceso del certificado (unicamente si el certificado NO está instalado)
        /// </summary>
        public string? CF { get; set; }
        /// <summary>
        /// Contraseña del certificado (unicamente si el certificado NO está instalado y es un certificado PRIVADO)
        /// </summary>
        public string? CP { get; set; }
        /// <summary>
        /// No de Serie del certificado (unicamente si el certificado está instalado)
        /// </summary>
        public string? CS { get; set; }
    }
}
