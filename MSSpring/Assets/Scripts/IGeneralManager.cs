using System.Collections.Generic;


public interface IGeneralManager
{
    List<animalProperty> getAllAnimals();
    void addAnAnimal(animalProperty newAnimal);

    int getCurCoinAmount();

    /// <summary>
    /// 尝试是否可以消耗一定数量
    /// </summary>
    /// <returns>是否可以改变</returns>
    bool checkIfCanChangeCoin(int n);

    /// <summary>
    /// 改变金币数量
    /// </summary>
    /// <returns>是否改变成功</returns>
    bool changeCoinAmount(int n);

    void setCoinAmount(int n);
}
