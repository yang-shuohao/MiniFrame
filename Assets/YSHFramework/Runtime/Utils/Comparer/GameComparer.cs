
namespace YSH.Framework.Utils
{
    /// <summary>
    /// ��Ϸ�Ƚ�����ע�����һ�������Ĺ��캯�������㸳ֵ��Ĭ��ֵ�Ժ󣬲����ᵱ���޲ι��캯������
    /// </summary>
    public static class GameComparer
    {
        public static readonly Vector3EqualityComparer DefaultVector3EqualityComparer = new Vector3EqualityComparer(ComparerConstants.DefaultTolerance);
        public static readonly FloatEqualityComparer DefaultFloatEqualityComparer = new FloatEqualityComparer(ComparerConstants.DefaultTolerance);
    }
}

