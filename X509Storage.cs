using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using x509_storage.Certificate;

namespace x509_storage
{
    /// <summary>
    /// Clase que administra los certificados cargados en la aplicación
    /// </summary>
    public class X509Storage
    {
        /// <summary>
        /// Almacen de certificados
        /// </summary>
        public static X509Storage Storage => instance;
        private static readonly X509Storage instance = new();

        /// <summary>
        /// Boveda de certificados de la aplicación
        /// </summary>
        public IReadOnlyDictionary<string, CertificateBase> Certificates => certificates;
        private readonly Dictionary<string, CertificateBase> certificates = new();

        private X509Storage() { }

        /// <summary>
        /// Carga los certificados especificados usando la información proporcionada
        /// </summary>
        /// <param name="infos">Información de los nuevos certificados</param>
        /// <exception cref="Exception"></exception>
        public void Load(params CertificateInfo[] infos)
        {
            foreach (CertificateInfo? info in infos)
            {
                if (info is not null)
                {
                    if (string.IsNullOrEmpty(info.Name))
                    {
                        throw new Exception($"Nombre de certificado inválido: {info.Name}");
                    }
                    else if (Certificates.ContainsKey(info.Name))
                    {
                        throw new Exception($"Nombre de certificado dupliacado: {info.Name}");
                    }
                    else
                    {
                        X509Certificate2 certificate;

                        if (info.IsInstalled)
                        {
                            if (string.IsNullOrEmpty(info.CS))
                            {
                                throw new Exception($"{nameof(info.CS)}, se necesita un número de serie válido");
                            }

                            certificate = X509Extensions.Get(info.CS);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(info.CF))
                            {
                                throw new Exception($"{nameof(info.CF)}, se necesita un path de certificado válido");
                            }

                            certificate = X509Extensions.Get(info.CF, info.CP);
                        }

                        if (certificate.HasPrivateKey)
                        {
                            certificates.Add(info.Name, new PrivateCertificate(info, certificate));
                        }
                        else
                        {
                            certificates.Add(info.Name, new PublicCertificate(info, certificate));
                        }
                    }
                }
            }
        }
    }
}
