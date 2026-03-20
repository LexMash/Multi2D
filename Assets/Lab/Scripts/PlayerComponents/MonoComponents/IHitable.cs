using Multi2D.Lab.Scripts.Data;

namespace Multi2D
{
    public interface IHitable
    {
        ulong ID { get; }
        void TakeHitRpc(HitData hitData);
    }
}
