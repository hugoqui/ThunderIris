Public Class FormSales
    Private db As TCESbaseEntities
    Private Sub FormSales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        Dim products = GetAllProducts()
        For Each product As tcProduct In products
            ComboBox1.Items.Add(product.Name)
        Next
    End Sub

    Function GetAllProducts() As IQueryable(Of tcProduct)
        Return db.tcProducts
    End Function
End Class