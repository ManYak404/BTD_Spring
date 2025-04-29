using System.Collections.Generic;

[System.Serializable]
public class BalloonData
{
    public string name;
    public float speed;
    public float health; // health of the balloon
    public int moneyValue; // money value of the balloon
    public int damage; // damage of the balloon
    public string[] nextBalloonsOnDestroy; // array of balloon types that can be spawned after this one
}

public class BalloonDataList
{
    public List<BalloonData> balloons; // array of balloon data objects
}