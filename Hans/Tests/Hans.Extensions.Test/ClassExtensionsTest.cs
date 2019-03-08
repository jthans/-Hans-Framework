using Hans.DependencyInjection;
using Hans.Extensions.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Hans.Extensions.Test
{
    /// <summary>
    ///  Class used to unit test any <see cref="ClassExtensions" /> that we've written.
    /// </summary>
    [TestClass]
    public class ClassExtensionsTest
    {
        #region Assembly Management

        /// <summary>
        ///  Initializes the assembly by building the DI framework container.
        /// </summary>
        /// <param name="context">Context giving explanation about the tests.</param>
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            MEFBootstrapper.Build();
        }

        #endregion

        #region ClearProperties

        /// <summary>
        ///  Ensures that [rp[ertoes are successfully cleared when this method is called.  Checks both
        ///     value types, and complex types.
        /// </summary>
        [TestMethod]
        public void ClearProperties_ClearSuccesful()
        {
            var animalClass = new TestClassAnimal("Orangutan", true);
            animalClass.ClearProperties();

            Assert.AreEqual(animalClass.AnimalName, default(string));
            Assert.AreEqual(animalClass.IsMammal, default(bool));
        }

        #endregion

        #region CopyPropertiesFromInstance

        /// <summary>
        ///  Method to ensure properties copy from one class to another successfully. 
        ///     AnimalOne = Empty class to copy into.
        ///     AnimalTwo = Class that has values to copy.
        /// </summary>
        [TestMethod]
        public void CopyPropertiesFromInstance_PropertiesCopyAcrossClassesSuccessfully()
        {
            // Create the instances.  We'll ensure the second class is different enough.
            var animalOne = new TestClassAnimal(null, false);
            var animalTwo = new TestClassAnimal("Tiger", true);

            // Copy the values, and assert it was done correctly.
            animalOne.CopyPropertiesFromInstance(animalTwo);

            Assert.AreEqual(animalOne.AnimalName, animalTwo.AnimalName);
            Assert.AreEqual(animalOne.IsMammal, animalTwo.IsMammal);
        }

        /// <summary>
        ///  Method to ensure an exception is thrown when two instances of different types are attempted
        ///     to copy across.
        /// </summary>
        [TestMethod]
        public void CopyPropertiesFromInstance_ThrowsExceptionOnDifferingInstanceTypes()
        {
            var animal = new TestClassAnimal("Tiger", true);
            var car = new TestClassCar("Kia Rio", false);

            // Ensure the copying throws an exception indicating you can't copy across types.
            Assert.ThrowsException<ReflectionTypeLoadException>(() =>
            {
                animal.CopyPropertiesFromInstance(car);
            });
        }

        #endregion

        #region MakeGenericMethodFromClass

        /// <summary>
        ///  Tests that a generic method can be called successfully from this method's creation.
        ///     Tests string/int types to return different values.
        /// </summary>
        [TestMethod]
        [DataTestMethod]
        [DataRow(typeof(string))]
        [DataRow(typeof(int))]
        public void MakeGenericMethodFromClass_CanExecuteGenericCall(Type testType)
        {
            var genericMethod = this.MakeGenericMethodFromClass("ReturnValueForGenericTest", testType);

            // Pull the type name using the generic method, and check that it matches the type given.
            string typeName = (string) genericMethod.Invoke(this, null);
            Assert.AreEqual(typeName, testType.Name);
        }

        [TestMethod]
        public void MakeGenericMethodFromClass_InvalidMethodReturnsException()
        {
            Assert.ThrowsException<MethodAccessException>(() =>
            {
                this.MakeGenericMethodFromClass("InvalidMethodName", null);
            });
        }

        /// <summary>
        ///  Generic method that we'll implement to ensure the method can be created successfuly.
        /// </summary>
        /// <param name="testType">The type we're running this test on.</param>
        /// <returns>Type Name.</returns>
        public string ReturnValueForGenericTest<T>()
        {
            return typeof(T).Name;
        }

        #endregion
    }
}
