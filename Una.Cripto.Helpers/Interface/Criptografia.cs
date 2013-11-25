using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Una.Cripto.Helpers
{
    public class Criptografia
    {
        #region Variáveis
        private byte[] Salt = Encoding.UTF8.GetBytes("/*-+.,01");
        private CryptoStream Cs;
        private ICryptoTransform Transform;
        private FileStream Fs;
        private byte[] Text;
        #endregion

        #region Propriedades
        public string CipherFile
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "ciphertext.txt");
            }
        }

        public string PlainFile
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "plaintext.txt");
            }
        }

        public string Algoritmo { get; set; }
        public PaddingMode Padding { get; set; }
        public CipherMode Mode { get; set; }

        private string iv = string.Empty;
        public string IV { get { return iv; } set { iv = !string.IsNullOrEmpty(value) ? value : string.Empty; } }
        private byte[] _IV { get; set; }

        public string Key { get; set; }
        private byte[] _Key { get; set; }
        #endregion

        #region Métodos Concretos
        public void Criptografar(string plainText)
        {
            try
            {
                this.Criptografar(Encoding.UTF8.GetBytes(plainText));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Descriptografar(string cipherText)
        {
            try
            {
                this.Descriptografar(Encoding.UTF8.GetBytes(cipherText));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Criptografar(Stream plainText)
        {
            try
            {
                byte[] buffer = new byte[plainText.Length];
                plainText.Read(buffer, 0, (int)plainText.Length);

                this.Criptografar(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Descriptografar(Stream cipherText)
        {
            try
            {
                byte[] buffer = new byte[cipherText.Length];
                cipherText.Read(buffer, 0, (int)cipherText.Length);

                this.Descriptografar(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Criptografar(byte[] plainText)
        {
            try
            {
                SymmetricAlgorithm _Algoritmo = CriptoFactory.GetAlgoritmo(Algoritmo);
                Rfc2898DeriveBytes helper = new Rfc2898DeriveBytes(this.Key, this.Salt);
                _Algoritmo.Mode = this.Mode;
                _Algoritmo.Padding = this.Padding;
                this._Key = helper.GetBytes(_Algoritmo.KeySize / Convert.ToInt32(Math.Sqrt(_Algoritmo.KeySize)));
                helper = new Rfc2898DeriveBytes(this.IV, this.Salt);
                this._IV = helper.GetBytes(_Algoritmo.KeySize / Convert.ToInt32(Math.Sqrt(_Algoritmo.KeySize)));
                this.Text = plainText;
                this.Transform = _Algoritmo.CreateEncryptor(this._Key, this._IV);
                this.Fs = File.Open(this.CipherFile, FileMode.Create, FileAccess.ReadWrite);
                this.Cs = new CryptoStream(this.Fs, this.Transform, CryptoStreamMode.Write);
                this.ExecutarCriptografia(_Algoritmo);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception("O arquivo se encontra inacessível", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Descriptografar(byte[] cipherText)
        {
            try
            {
                SymmetricAlgorithm _Algoritmo = CriptoFactory.GetAlgoritmo(Algoritmo);
                Rfc2898DeriveBytes helper = new Rfc2898DeriveBytes(this.Key, Encoding.UTF8.GetBytes("/*-+.,01"));
                _Algoritmo.Mode = this.Mode;
                _Algoritmo.Padding = this.Padding;
                this._Key = helper.GetBytes(_Algoritmo.KeySize / Convert.ToInt32(Math.Sqrt(_Algoritmo.KeySize)));
                helper = new Rfc2898DeriveBytes(this.IV, this.Salt);
                this._IV = helper.GetBytes(_Algoritmo.KeySize / Convert.ToInt32(Math.Sqrt(_Algoritmo.KeySize)));
                this.Text = cipherText;
                this.Transform = _Algoritmo.CreateDecryptor(this._Key, this._IV);
                this.Fs = File.Open(this.PlainFile, FileMode.Create, FileAccess.ReadWrite);
                this.Cs = new CryptoStream(this.Fs, this.Transform, CryptoStreamMode.Write);
                this.ExecutarCriptografia(_Algoritmo);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception("O arquivo se encontra inacessível", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExecutarCriptografia(SymmetricAlgorithm _Algoritmo)
        {
            try
            {
                using (this.Fs)
                using (_Algoritmo)
                using (this.Cs)
                    Cs.Write(this.Text, 0, this.Text.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
