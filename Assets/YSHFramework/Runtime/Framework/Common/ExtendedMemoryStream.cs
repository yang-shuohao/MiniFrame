
using System;
using System.IO;
using System.Text;

public class ExtendedMemoryStream : MemoryStream
{
    public ExtendedMemoryStream()
    {

    }

    public ExtendedMemoryStream(byte[] buffer) : base(buffer)
    {

    }

    #region Short
    /// <summary>
    /// �����ж�ȡһ��short����
    /// </summary>
    /// <returns></returns>
    public short ReadShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);
        return BitConverter.ToInt16(arr, 0);
    }

    /// <summary>
    /// ��һ��short����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteShort(short value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region UShort
    /// <summary>
    /// �����ж�ȡһ��ushort����
    /// </summary>
    /// <returns></returns>
    public ushort ReadUShort()
    {
        byte[] arr = new byte[2];
        base.Read(arr, 0, 2);
        return BitConverter.ToUInt16(arr, 0);
    }

    /// <summary>
    /// ��һ��ushort����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteUShort(ushort value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Int
    /// <summary>
    /// �����ж�ȡһ��int����
    /// </summary>
    /// <returns></returns>
    public int ReadInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToInt32(arr, 0);
    }

    /// <summary>
    /// ��һ��int����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteInt(int value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region UInt
    /// <summary>
    /// �����ж�ȡһ��uint����
    /// </summary>
    /// <returns></returns>
    public uint ReadUInt()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToUInt32(arr, 0);
    }

    /// <summary>
    /// ��һ��uint����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteUInt(uint value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Long
    /// <summary>
    /// �����ж�ȡһ��long����
    /// </summary>
    /// <returns></returns>
    public long ReadLong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToInt64(arr, 0);
    }

    /// <summary>
    /// ��һ��long����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteLong(long value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region ULong
    /// <summary>
    /// �����ж�ȡһ��ulong����
    /// </summary>
    /// <returns></returns>
    public ulong ReadULong()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToUInt64(arr, 0);
    }

    /// <summary>
    /// ��һ��ulong����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteULong(ulong value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Float
    /// <summary>
    /// �����ж�ȡһ��float����
    /// </summary>
    /// <returns></returns>
    public float ReadFloat()
    {
        byte[] arr = new byte[4];
        base.Read(arr, 0, 4);
        return BitConverter.ToSingle(arr, 0);
    }

    /// <summary>
    /// ��һ��float����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteFloat(float value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Double
    /// <summary>
    /// �����ж�ȡһ��double����
    /// </summary>
    /// <returns></returns>
    public double ReadDouble()
    {
        byte[] arr = new byte[8];
        base.Read(arr, 0, 8);
        return BitConverter.ToDouble(arr, 0);
    }

    /// <summary>
    /// ��һ��double����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteDouble(double value)
    {
        byte[] arr = BitConverter.GetBytes(value);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

    #region Bool
    /// <summary>
    /// �����ж�ȡһ��bool����
    /// </summary>
    /// <returns></returns>
    public bool ReadBool()
    {
        return base.ReadByte() == 1;
    }

    /// <summary>
    /// ��һ��bool����д����
    /// </summary>
    /// <param name="value"></param>
    public void WriteBool(bool value)
    {
        base.WriteByte((byte)(value == true ? 1 : 0));
    }
    #endregion

    #region UTF8String
    /// <summary>
    /// �����ж�ȡһ��sting����
    /// </summary>
    /// <returns></returns>
    public string ReadUTF8String()
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];
        base.Read(arr, 0, len);
        return Encoding.UTF8.GetString(arr);
    }

    /// <summary>
    /// ��һ��string����д����
    /// </summary>
    /// <param name="str"></param>
    public void WriteUTF8String(string str)
    {
        byte[] arr = Encoding.UTF8.GetBytes(str);
        if (arr.Length > 65535)
        {
            throw new InvalidCastException("�ַ���������Χ");
        }
        WriteUShort((ushort)arr.Length);
        base.Write(arr, 0, arr.Length);
    }
    #endregion

}
