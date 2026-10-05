namespace ConsoleApp1
{
    class Car : Vehicle
    {
        private string manufacturerName;
        private string modelName;

        public string ManufacturerName
        {
            get
            {
                Console.WriteLine("Enter car manufacturer: ");
                return manufacturerName = Console.ReadLine();
            }
            set
            {
                manufacturerName = value;
            }
        }

        public string ModelName
        {
            get
            {
                Console.WriteLine("Enter car model: ");
                return modelName = Console.ReadLine();
            }
            set
            {
                modelName = value;
            }
        }

        public void fastAndFurious()
        {
            Console.WriteLine("The " + ManufacturerName + " " + ModelName + " goes really fast and is furious :-)");
        }
    }
}
