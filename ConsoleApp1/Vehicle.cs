namespace ConsoleApp1
{
    class Vehicle : IVehicle
    {
        private string brand = "Ford";
        
        
        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }

        public void vehicleGoesBrmBrm()
        {
            Console.WriteLine("The vehicle goes Brm Brm.");
        }

        public void honk()
        {
            Console.WriteLine("Tuut, tuut!");
        }
    }
}
