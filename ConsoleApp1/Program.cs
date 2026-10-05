using ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        Car myCar = new Car();

        myCar.honk();

        Console.WriteLine(myCar.Brand + " " + myCar.modelName);

        myCar.vehicleGoesBrmBrm();

    }
}