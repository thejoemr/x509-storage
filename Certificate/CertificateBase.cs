using System;
using System.Security.Cryptography.X509Certificates;

namespace x509_storage.Certificate
{
    /// <summary>
    /// Define la información general de un certificado
    /// </summary>
    public class CertificateBase
    {
        /// <summary>
        /// Crea un certificado general, sin funcionalidad
        /// </summary>
        /// <param name="info">Información del certificadó que se cargó</param>
        /// <param name="certificate">Certificado adjunto</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CertificateBase(CertificateInfo info, X509Certificate2 certificate)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            IsPrivate = certificate.HasPrivateKey;
        }

        /// <summary>
        /// Información del certificado
        /// </summary>
        public CertificateInfo Info { get; }
        /// <summary>
        /// Certificado
        /// </summary>
        public X509Certificate2 Certificate { get; }

        /// <summary>
        /// Especifica si el certificado es privado (de lo contrario, es público)
        /// </summary>
        public bool IsPrivate { get; }
    }

}