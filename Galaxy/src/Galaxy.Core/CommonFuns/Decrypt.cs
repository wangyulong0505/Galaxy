using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Core.CommonFuns
{
    public class Decrypt
    {
        // 创建Key  
        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        // 加密字符串  
        public static string EncryptString(string sInputString, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(sInputString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }

        public static string DecryptString(string sInputString, string sKey)
        {
            string[] sInput = sInputString.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        ///MD5加密  
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();


        }

        ///MD5解密  
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecryptStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string password = "JASONHEUNG";
            int passIndex = 0;
            int passLength = password.Length;
            int num = 0, byt = 0;
            int len = str.Length;
            string resultStr = "";
            int preNum = -1;
            int preIndex = 0;
            for (int i = 0; i < len; i++)
            {
                int code = (int)str[i];
                code = (code == 95) ? 63 : ((code == 44) ? 62 : ((code >= 97) ? (code - 61) : ((code >= 65) ? (code - 55) : (code - 48))));

                code = (code - (int)password[passIndex++] + 128) % 64;
                passIndex = passIndex % passLength;

                byt = (byt << 6) + code;
                num += 6;
                while (num >= 8)
                {
                    int b = byt >> (num - 8);
                    if (preIndex > 0)
                    {
                        preNum = (preNum << 6) + (b & (0x3f));
                        preIndex--;
                        if (preIndex == 0)
                        {
                            resultStr += (char)preNum;
                        }
                    }
                    else
                    {
                        if (b >= 224)
                        {
                            preNum = b & (0xf);
                            preIndex = 2;
                        }
                        else if (b >= 128)
                        {
                            preNum = b & (0x1f);
                            preIndex = 1;
                        }
                        else
                        {
                            resultStr += (char)b;
                        }
                    }
                    byt = byt - (b << (num - 8));
                    num -= 8;
                }
            }

            return resultStr;
        }
    }
}
