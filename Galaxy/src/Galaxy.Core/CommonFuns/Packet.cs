using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TouchScreen.Core.CommonFuns;

namespace Galaxy.CommonFuns
{
    /// <summary>
    /// 打包、解包
    /// </summary>
    /// <typeparam name="T">数据类型 object</typeparam>
    public abstract class Packet<T>
    {
        /// <summary>
        /// 打包数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="strKey">加密key</param>
        /// <returns></returns>
        public static byte[] PacketData(T t, string strKey)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;               //忽略为NULL的值

            byte[] content;
            string strJson = JsonConvert.SerializeObject(t, setting);           //字符串转json：主要是规范
            strJson = JhEncrypt.Encrypt(strJson, strKey);                       //加密：主要是安全
            content = SerializeHelper.ConvertToByte(strJson);                   //序列化：主要是二进制传输效率高

            return content;
        }

        public static T DePacketData(byte[] content, string strKey)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;                       //忽略为NULL的值   
            T t;
            string strJson = SerializeHelper.DeserializeWithBinary(content).ToString(); //反序列化
            strJson = JhEncrypt.Decrypt(strJson, strKey);                               //解密
            t = (T)JsonConvert.DeserializeObject(strJson, typeof(T), setting);          //json转对象

            return t;
        }
    }
}
