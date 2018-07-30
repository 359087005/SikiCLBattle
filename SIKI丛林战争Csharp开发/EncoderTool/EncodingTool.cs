using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// 粘包拆包  进行封装
/// </summary>
public static class EncodingTool
{
    /// <summary>
    /// 构造数据包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] EnconderPacket(byte[] data)
    {
        using (MemoryStream me = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(me))
            {
                //构建数据包
                bw.Write(data.Length);
                //写入数据
                bw.Write(data);

                byte[] byteArray = new byte[(int)me.Length];
                Buffer.BlockCopy(me.GetBuffer(),0,byteArray,0,(int)me.Length);

                return byteArray;
            }
        }
    }

    /// <summary>
    /// 解析数据包
    /// </summary>
    /// <returns></returns>
    public static byte[] DeconderPacket(ref List<byte> dataCache)
    {
        if (dataCache.Count < 4) return null;//数据长度不够解析

        using (MemoryStream me = new MemoryStream(dataCache.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(me))
            {
                int length = br.ReadInt32(); //获取数据的总长度
                int remainLength = (int)(me.Length - me.Position);
                if (length > remainLength)  //数据总长度 大于 需要读取的长度
                {                           //说明数据不足
                    return null;
                }
                byte[] data = br.ReadBytes(length); //读取该长度的数据
                //保留缓存区多余的数据
                dataCache.Clear();
                dataCache.AddRange(br.ReadBytes(remainLength));
                return data;
            }
        }
    }
}

