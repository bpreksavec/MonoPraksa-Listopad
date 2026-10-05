namespace ConsoleApp1
{
    class Vehicle : IVehicle
    {
        private string vechileType;
        
        
        public string VechileType
        {
            get {
                Console.WriteLine("Enter vechile type: ");
                vechileType = Console.ReadLine();
                return vechileType; 
            }
            set { vechileType = value; }
        }

        public void vehicleGoesBrmBrm()
        {
            Console.WriteLine("The " + VechileType + " goes Brm Brm.");
        }

        public void honk()
        {
            Console.WriteLine("Tuut, tuut!");
        }
    }
}
