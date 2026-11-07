using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OpenTrophy.Core
{
    /// <summary>
    /// The local trophy file: encrypted (AES-256-CBC) and signed (HMAC-SHA256), with a key made from a
    /// secret shipped in the game and from the device. Editing the file by hand, or copying it from
    /// another machine, makes it unreadable: it is then set aside and the game starts from the account's
    /// trophies. This stops casual cheating; it cannot stop someone who takes the game apart.
    /// </summary>
    public sealed class SecureStore
    {
        static readonly byte[] Magic = Encoding.ASCII.GetBytes("OTR1");
        const int Iterations = 20000;
        readonly string path;
        readonly string passphrase;

        public SecureStore(string filePath, string gameSecret, string deviceId)
        {
            path = filePath;
            passphrase = (gameSecret ?? "") + "|" + (deviceId ?? "");
        }

        public string FilePath { get { return path; } }

        /// <summary>The stored text, or null when there is no file or when it cannot be trusted.</summary>
        public string Load()
        {
            string text = Read(path);
            if (text != null) return text;
            // The last write may have been cut short (power loss): the previous version is kept beside it.
            text = Read(path + ".bak");
            if (text == null && File.Exists(path)) SetAside();
            return text;
        }

        string Read(string file)
        {
            try
            {
                if (!File.Exists(file)) return null;
                byte[] all = File.ReadAllBytes(file);
                if (all.Length < Magic.Length + 16 + 16 + 32) return null;
                for (int i = 0; i < Magic.Length; i++) if (all[i] != Magic[i]) return null;
                byte[] salt = new byte[16], iv = new byte[16], mac = new byte[32];
                Buffer.BlockCopy(all, Magic.Length, salt, 0, 16);
                Buffer.BlockCopy(all, Magic.Length + 16, iv, 0, 16);
                int bodyStart = Magic.Length + 32, bodyLength = all.Length - bodyStart - 32;
                Buffer.BlockCopy(all, all.Length - 32, mac, 0, 32);
                byte[] encKey, macKey;
                Keys(salt, out encKey, out macKey);
                using (var h = new HMACSHA256(macKey))
                {
                    byte[] expected = h.ComputeHash(all, 0, all.Length - 32);
                    int diff = 0;
                    for (int i = 0; i < 32; i++) diff |= expected[i] ^ mac[i]; // constant time
                    if (diff != 0) return null;
                }
                using (var aes = Aes.Create())
                {
                    aes.KeySize = 256; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7; aes.Key = encKey; aes.IV = iv;
                    using (var dec = aes.CreateDecryptor())
                        return Encoding.UTF8.GetString(dec.TransformFinalBlock(all, bodyStart, bodyLength));
                }
            }
            catch (Exception) { return null; }
        }

        /// <summary>Writes the text. The previous file stays as a backup until the new one is complete.</summary>
        public bool Save(string text)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                // The salt stays the same for the life of the file (the keys are costly to derive); the IV is new every time.
                byte[] salt = cachedSalt, iv = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    if (salt == null) { salt = new byte[16]; rng.GetBytes(salt); }
                    rng.GetBytes(iv);
                }
                byte[] encKey, macKey;
                Keys(salt, out encKey, out macKey);
                byte[] body;
                using (var aes = Aes.Create())
                {
                    aes.KeySize = 256; aes.Mode = CipherMode.CBC; aes.Padding = PaddingMode.PKCS7; aes.Key = encKey; aes.IV = iv;
                    byte[] plain = Encoding.UTF8.GetBytes(text);
                    using (var enc = aes.CreateEncryptor()) body = enc.TransformFinalBlock(plain, 0, plain.Length);
                }
                byte[] all = new byte[Magic.Length + 32 + body.Length + 32];
                Buffer.BlockCopy(Magic, 0, all, 0, Magic.Length);
                Buffer.BlockCopy(salt, 0, all, Magic.Length, 16);
                Buffer.BlockCopy(iv, 0, all, Magic.Length + 16, 16);
                Buffer.BlockCopy(body, 0, all, Magic.Length + 32, body.Length);
                using (var h = new HMACSHA256(macKey))
                {
                    byte[] mac = h.ComputeHash(all, 0, all.Length - 32);
                    Buffer.BlockCopy(mac, 0, all, all.Length - 32, 32);
                }
                string tmp = path + ".tmp";
                File.WriteAllBytes(tmp, all);
                if (File.Exists(path)) { File.Copy(path, path + ".bak", true); File.Delete(path); }
                File.Move(tmp, path);
                return true;
            }
            catch (Exception) { return false; }
        }

        public void Delete()
        {
            cachedSalt = null;
            foreach (string f in new[] { path, path + ".bak", path + ".tmp" })
                try { if (File.Exists(f)) File.Delete(f); } catch (Exception) { }
        }

        /// <summary>A file that cannot be trusted is kept under another name (for support), not destroyed.</summary>
        void SetAside()
        {
            try
            {
                string aside = path + ".rejected";
                if (File.Exists(aside)) File.Delete(aside);
                File.Move(path, aside);
                if (File.Exists(path + ".bak")) File.Delete(path + ".bak");
            }
            catch (Exception) { }
        }

        byte[] cachedSalt, cachedEnc, cachedMac;

        void Keys(byte[] salt, out byte[] encKey, out byte[] macKey)
        {
            bool same = cachedSalt != null;
            for (int i = 0; same && i < 16; i++) same = cachedSalt[i] == salt[i];
            if (!same)
            {
                using (var kdf = new Rfc2898DeriveBytes(passphrase, salt, Iterations))
                {
                    cachedEnc = kdf.GetBytes(32);
                    cachedMac = kdf.GetBytes(32);
                }
                cachedSalt = (byte[])salt.Clone();
            }
            encKey = cachedEnc;
            macKey = cachedMac;
        }
    }
}
