using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HZY.Toolkit.ToolKits
{
    /*
    * 参考网址：http://www.cnblogs.com/feiyangqingyun/archive/2010/12/20/1911630.html
    * http://www.cnblogs.com/lekko/archive/2012/09/11/2680288.html
    */
    /// <summary>
    /// 加密安全类，包含AES、MD5、DES、RC2、3DES、AES等加密方式
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// 登录交互加密key
        /// </summary>
        public readonly static string _key = "Hz123456";

        #region MD5加密（消息摘要算法第五版）
        #region[私有方法]

        #region 获取随机生成数
        /// <summary>
        /// 获取随机生成数
        /// </summary>
        /// <param name="size">随机数长度</param>
        /// <returns></returns>
        private static string CreateSalt(int size)
        {
            var provider = new RNGCryptoServiceProvider();
            byte[] data = new byte[size];
            provider.GetBytes(data);
            return Convert.ToBase64String(data);
        }
        #endregion

        #region 加密实现
        /// <summary>
        /// 加密实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] MD5ToHexByte(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(data);
        }

        /// <summary>
        /// 加密实现
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MD5ToHexString(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string t = "";
            string tTemp = "";
            for (int i = 0; i < result.Length; i++)
            {
                tTemp = Convert.ToString(result[i], 16);
                if (tTemp.Length != 2)
                {
                    switch (tTemp.Length)
                    {
                        case 0: tTemp = "00"; break;
                        case 1: tTemp = "0" + tTemp; break;
                        default: tTemp = tTemp.Substring(0, 2); break;
                    }
                }
                t += tTemp;
            }
            return t;
        }

        /// <summary>
        /// 加密实现
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string MD5ToHexString(string strText, Encoding EncodingUsing = null)
        {
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            byte[] data = EncodingUsing.GetBytes(strText);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            string t = "";
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > 0xf)
                {
                    t += Convert.ToString(result[i], 16);
                }
                else
                {
                    t += "0";
                    t += Convert.ToString(result[i], 16);
                }

            }
            return t;
        }
        #endregion
        #endregion

        #region [公共方法]
        #region 加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        public static string CreatePasswordHash(string strPassword, Encoding EncodingUsing = null)
        {
            return CreatePasswordHash(strPassword, null, EncodingUsing);
        }

        /// <summary>
        ///  MD5加密
        /// </summary>
        /// <param name="strPassword">用户输入的密码,可能空</param>
        /// <param name="salt">salt值</param>
        /// <returns>返回MD5加密后的密码</returns>
        /// <remarks>
        /// 这里主要定义了从salt值以什么方式什么次序计算密码
        /// </remarks>
        public static string CreatePasswordHash(string strPassword, string salt, Encoding EncodingUsing = null)
        {
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            if (strPassword == null)
                strPassword = string.Empty;
            if (salt == null)
                salt = string.Empty;
            return MD5ToHexString(strPassword + salt, EncodingUsing);
        }
        #endregion
        #endregion
        #endregion

        #region [对称加解密]



        ///<summary><![CDATA[字符串DES加密函数]]>对应java</summary>  
        ///<param name="str"><![CDATA[被加密字符串 ]]></param>  
        ///<param name="key"><![CDATA[密钥 ]]></param>   
        ///<returns><![CDATA[加密后字符串]]></returns>     
        public static string Encode(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                StringBuilder builder = new StringBuilder();
                foreach (byte num in stream.ToArray())
                {
                    builder.AppendFormat("{0:X2}", num);
                }
                stream.Close();
                return builder.ToString();
            }
            catch (Exception ex) { throw ex; }
        }
        ///<summary><![CDATA[字符串DES解密函数]]>对应java</summary>  
        ///<param name="str"><![CDATA[被解密字符串 ]]></param>  
        ///<param name="key"><![CDATA[密钥 ]]></param>   
        ///<returns><![CDATA[解密后字符串]]></returns>     
        public static string Decode(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < (str.Length / 2); i++)
                {
                    int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                    buffer[i] = (byte)num2;
                }
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                stream.Close();
                return Encoding.GetEncoding("UTF-8").GetString(stream.ToArray());
                //return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
            }
            catch (Exception ex) { throw ex; }
        }

        #region DES 加解密
        /// <summary>
        /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="EncryptString">待加密的密文</param>
        /// <param name="EncryptKey">加密的密钥</param>
        /// <returns>returns</returns>
        public static string DESEncrypt(string EncryptString, string EncryptKey, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

            if (EncryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            string m_strEncrypt = "";

            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btEncryptString = EncodingUsing.GetBytes(EncryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateEncryptor(EncodingUsing.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

                m_stream.Close();
                m_stream.Dispose();

                m_cstream.Close();
                m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strEncrypt;
        }

        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="DecryptString">待解密的密文</param>
        /// <param name="DecryptKey">解密的密钥</param>
        /// <returns>returns</returns>
        public static string DESDecrypt(string DecryptString, string DecryptKey, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

            if (DecryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

            string m_strDecrypt = "";

            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                MemoryStream m_stream = new MemoryStream();

                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(EncodingUsing.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);

                m_cstream.FlushFinalBlock();

                m_strDecrypt = EncodingUsing.GetString(m_stream.ToArray());

                m_stream.Close();
                m_stream.Dispose();

                m_cstream.Close();
                m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }

            return m_strDecrypt;
        }
        #endregion

        #region RC2 加解密
        /// <summary>
        /// RC2 加密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Encrypt(string EncryptString, string EncryptKey, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(EncryptString))
                throw (new Exception("密文不得为空"));
            if (string.IsNullOrEmpty(EncryptKey))
                throw (new Exception("密钥不得为空"));
            if (EncryptKey.Length < 5 || EncryptKey.Length > 16)
                throw (new Exception("密钥必须为5-16位"));
            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }
            string m_strEncrypt = "";
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();
            try
            {
                byte[] m_btEncryptString = EncodingUsing.GetBytes(EncryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateEncryptor(EncodingUsing.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);
                m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);
                m_cstream.FlushFinalBlock();
                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                m_stream.Close();
                m_stream.Dispose();
                m_cstream.Close();
                m_cstream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_RC2Provider.Clear();
            }
            return m_strEncrypt;
        }

        /// <summary>
        /// RC2 解密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Decrypt(string DecryptString, string DecryptKey, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(DecryptString))
                throw (new Exception("密文不得为空"));
            if (string.IsNullOrEmpty(DecryptKey))
                throw (new Exception("密钥不得为空"));
            if (DecryptKey.Length < 5 || DecryptKey.Length > 16)
                throw (new Exception("密钥必须为5-16位"));
            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strDecrypt = "";
            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();
            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateDecryptor(EncodingUsing.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);
                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                m_cstream.FlushFinalBlock();
                m_strDecrypt = EncodingUsing.GetString(m_stream.ToArray());
                m_stream.Close();
                m_stream.Dispose();
                m_cstream.Close();
                m_cstream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_RC2Provider.Clear();
            }
            return m_strDecrypt;
        }
        #endregion

        #region 3DES 加解密
        /// <summary>
        /// 3DES 加密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey1">密钥一</param>
        /// <param name="EncryptKey2">密钥二</param>
        /// <param name="EncryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Encrypt(string EncryptString, string EncryptKey1, string EncryptKey2, string EncryptKey3)
        {
            string m_strEncrypt = "";
            try
            {
                m_strEncrypt = DESEncrypt(EncryptString, EncryptKey3);

                m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey2);

                m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey1);
            }
            catch (Exception ex) { throw ex; }
            return m_strEncrypt;
        }

        /// <summary>
        /// 3DES 解密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey1">密钥一</param>
        /// <param name="DecryptKey2">密钥二</param>
        /// <param name="DecryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Decrypt(string DecryptString, string DecryptKey1, string DecryptKey2, string DecryptKey3)
        {
            string m_strDecrypt = "";
            try
            {
                m_strDecrypt = DESDecrypt(DecryptString, DecryptKey1);

                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey2);

                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey3);
            }
            catch (Exception ex) { throw ex; }
            return m_strDecrypt;
        }

        /// <summary>
        /// TripleDES解密
        /// </summary>
        public static string DESDecrypting(string Source, Encoding EncodingUsing = null)
        {
            try
            {
                if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }

                byte[] bytIn = System.Convert.FromBase64String(Source);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };   //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, EncodingUsing);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }

        /// <summary>
        /// TripleDES加密
        /// </summary>
        public static string DESEncrypting(string strSource)
        {
            try
            {
                byte[] bytIn = Encoding.Default.GetBytes(strSource);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };  //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateEncryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                byte[] bytOut = ms.ToArray();
                return System.Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
        #endregion

        #region AES 加解密(对称加密)
        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static byte[] _aesKeys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <param name="key">初始化向量</param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey, string key = "", Encoding EncodingUsing = null)
        {
            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }
            //将密钥填充或截取，以达到32位
            encryptKey = encryptKey.PadRight(32, ' ');
            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            //获取或设置对称算法密钥
            rijndaelProvider.Key = EncodingUsing.GetBytes(encryptKey.Substring(0, 32));
            if (!string.IsNullOrWhiteSpace(key))
                _aesKeys = Convert.FromBase64String(key);
            rijndaelProvider.IV = _aesKeys;
            //创建对称密器对象
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();
            //获取待加密的文本值字节序列
            byte[] inputData = EncodingUsing.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <param name="key">初始化向量</param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptString, string decryptKey, string key = "", Encoding EncodingUsing = null)
        {
            if (EncodingUsing == null) { EncodingUsing = new UTF8Encoding(); }

            //将密钥填充或截取，以达到32位
            decryptKey = decryptKey.PadRight(32, ' ');
            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            //获取或设置对称算法密钥
            rijndaelProvider.Key = EncodingUsing.GetBytes(decryptKey.Substring(0, 32));
            if (!string.IsNullOrWhiteSpace(key))
                _aesKeys = Convert.FromBase64String(key);
            rijndaelProvider.IV = _aesKeys;
            //创建对称密器对象
            ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

            byte[] inputData = Convert.FromBase64String(decryptString);
            byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return EncodingUsing.GetString(decryptedData);
        }
        #endregion

        #endregion

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(String input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串 
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串 
            return sBuilder.ToString();
        }

        /// <summary>
        /// 将集合以ascii码从小到大排序
        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
        public static List<string> AsciiDictionary(List<string> sArray)
        {
            string[] arrKeys = sArray.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            return arrKeys.ToList();
        }


        public static string Encrypt(string str, string key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(str);
                des.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                des.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 根据Ascii排序    

        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsciiDictionary(Dictionary<string, object> sArray)
        {
            string[] arrKeys = sArray.Keys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            return arrKeys.ToDictionary(key => key, key => sArray[key]);
        }
        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string Text)
        {
            return MD5Encrypt(Text, "wicrosoft");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string MD5Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(sKey));
            string strResult = BitConverter.ToString(data);//转换为十六进制
            //第三步，去掉"-"符号
            string realResult = strResult.Replace("-", null);
            des.Key = ASCIIEncoding.ASCII.GetBytes(realResult.Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(realResult.Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string MD5Decrypt(string Text)
        {
            return MD5Decrypt(Text, "wicrosoft");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string MD5Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(sKey));
            string strResult = BitConverter.ToString(data);//转换为十六进制
            //第三步，去掉"-"符号
            string realResult = strResult.Replace("-", null);

            des.Key = ASCIIEncoding.ASCII.GetBytes(realResult.Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(realResult.Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            try
            {

                byte[] keyBytes = Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            try
            {

                byte[] keyBytes = Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception e)
            {
                throw;
            }
        }



        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="mystr">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1Crypt(string mystr)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(mystr);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }


    }
}
