namespace Tests.model;

[TestFixture]
public class AbstractEmployeeTests
{
    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {

    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {

    }

    [Test]
    public void Name_SetNull_ShouldThrowException()
    {

    }

    [Test]
    public void Name_SetWhiteSpace_ShouldThrowException()
    {

    }

    [Test]
    public void Name_SetNonWhiteSpace_ShouldSet()
    {

    }

    [Test]
    public void Salary_SetNegativeValue_ShouldThrowException()
    {
        
    }

    [Test]
    public void Salary_SetPositiveValue_ShouldSet()
    {
        
    }

    [Test]
    public void HireDate_SetFutureDate_ShouldThrowException()
    {
        
    }

    [Test]
    public void HireDate_SetPastDate_ShouldSet()
    {
        
    }

    [Test]
    public void LayoffDate_SetBeforeHireDateAndHasValue_ShouldThrowException()
    {
        
    }

    [Test]
    public void LayoffDate_SetAfterHireDateAndHasValue_ShouldSet()
    {
        
    }

    [Test]
    public void LayoffDate_SetNull_ShouldSet()
    {
        
    }

    [Test]
    public void ShiftStart_SetAfterShiftEnd_ShouldThrowException()
    {
        
    }

    [Test]
    public void ShiftStart_SetBeforeShiftEnd_ShouldSet()
    {
        
    }

    [Test]
    public void ShiftEnd_SetBeforeShiftStart_ShouldThrowException()
    {
        
    }

    [Test]
    public void ShiftEnd_SetAfterShiftStart_ShouldSet()
    {
        
    }
}