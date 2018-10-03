namespace Hans.Extensions.Test.Models
{
    /// <summary>
    ///  Very simple class representing an animal, to test class extensions.
    /// </summary>
    public class TestClassAnimal
    {
        #region Constructors

        /// <summary>
        ///  Instantiates a new instance of the <see cref="TestClassAnimal" /> class.
        /// </summary>
        /// <param name="animalName">Name.</param>
        /// <param name="isMammal">If it's a Mammal.</param>
        public TestClassAnimal(string animalName, bool isMammal)
        {
            this.AnimalName = animalName;
            this.IsMammal = IsMammal;
        }

        #endregion

        #region Properties

        public string AnimalName { get; set; }

        public bool IsMammal { get; set; }

        #endregion
    }
}
