'------------------------------------
' File: UtilityTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

<TestClass()>
Public Class MandantDetailsTest

    <TestMethod()>
    Public Sub TestProperties_MandantIsNotValid_PropertiesAreEmptyStrings()
        ' Arrange
        Dim mandantDetails As MandantDetails = New MandantDetails()
        mandantDetails.Guid = Guid.NewGuid().ToString("D")
        mandantDetails.Name = "TestName"
        mandantDetails.HomePage = "www.homepage.com"
        mandantDetails.EmailAddress = "name@domain.com"
        mandantDetails.IsValidMandantGuid = False

        ' Act
        Dim name As String = mandantDetails.Name
        Dim homePage As String = mandantDetails.HomePage
        Dim emailAddress As String = mandantDetails.EmailAddress

        ' Assert
        Assert.IsTrue(String.IsNullOrEmpty(name), "String should be empty.")
        Assert.IsTrue(String.IsNullOrEmpty(homePage), "String should be empty.")
        Assert.IsTrue(String.IsNullOrEmpty(emailAddress), "String should be empty.")
    End Sub

    <TestMethod()>
    Public Sub TestProperties_MandantIsValid_PropertiesAreNotEmptyStrings()
        ' Arrange
        Dim mandantDetails As MandantDetails = New MandantDetails()
        mandantDetails.Guid = Guid.NewGuid().ToString("D")
        mandantDetails.Name = "TestName"
        mandantDetails.HomePage = "www.homepage.com"
        mandantDetails.EmailAddress = "name@domain.com"
        mandantDetails.IsValidMandantGuid = True

        ' Act
        Dim name As String = mandantDetails.Name
        Dim homePage As String = mandantDetails.HomePage
        Dim emailAddress As String = mandantDetails.EmailAddress

        ' Assert
        Assert.IsFalse(String.IsNullOrEmpty(name), "String should not be empty.")
        Assert.IsFalse(String.IsNullOrEmpty(homePage), "String should not be empty.")
        Assert.IsFalse(String.IsNullOrEmpty(emailAddress), "String should not be empty.")
    End Sub
End Class
