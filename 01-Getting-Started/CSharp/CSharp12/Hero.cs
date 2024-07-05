//
// Primary Constructors
//

// Before
public class Hero
{
    private string _name;
    private int _age;
    private string _superPower;

    public Hero(string name, int age, string superPower)
    {
        _name = name;
        _age = age;
        _superPower = superPower;
    }
}

// After
public class Hero2(string name, int age, string superPower)
{
    private string _name = name;
    private int _age = age;
    private string _superPower = superPower;
}