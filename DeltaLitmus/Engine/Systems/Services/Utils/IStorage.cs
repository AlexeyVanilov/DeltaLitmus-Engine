namespace DeltaLitmus.Systems.Services.Utils {
    public interface IStorage {
        void Remove(int entityId);
        void Clear();
    }
}