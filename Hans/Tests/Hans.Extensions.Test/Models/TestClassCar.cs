namespace Hans.Extensions.Test.Models
{
    /// <summary>
    ///  Very simple class representing a car, to test class extensions.
    /// </summary>
    public class TestClassCar
    {
        #region Constructors

        /// <summary>
        ///  Instantiates a new instance of the <see cref="TestClassCar" /> class.
        /// </summary>
        /// <param name="animalName">Name.</param>
        /// <param name="isMammal">If it's a Mammal.</param>
        public TestClassCar(string carName, bool isUsed)
        {
            this.CarName = carName;
            this.IsUsed = isUsed;
        }

        #endregion

        #region Properties

        public string CarName { get; set; }

        public bool IsUsed { get; set; }

        #endregion
    }
}
