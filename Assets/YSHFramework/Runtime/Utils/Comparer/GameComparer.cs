
namespace YSH.Framework.Utils
{
    /// <summary>
    /// 游戏比较器，注意带有一个参数的构造函数，当你赋值了默认值以后，并不会当做无参构造函数调用
    /// </summary>
    public static class GameComparer
    {
        public static readonly Vector3EqualityComparer DefaultVector3EqualityComparer = new Vector3EqualityComparer(ComparerConstants.DefaultTolerance);
        public static readonly FloatEqualityComparer DefaultFloatEqualityComparer = new FloatEqualityComparer(ComparerConstants.DefaultTolerance);
    }
}

