using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Una.Cripto.Helpers
{
    public class CriptoFactory
    {
        public static SymmetricAlgorithm GetAlgoritmo(string Algoritmo = "")
        {
            if (Algoritmo.Equals("AES"))
                return new AesCryptoServiceProvider();
            else
                return new DESCryptoServiceProvider();
        }
    }
}
