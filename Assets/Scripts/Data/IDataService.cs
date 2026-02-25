public interface IDataService
{
    GameData Load();
    void Save(GameData data);
}