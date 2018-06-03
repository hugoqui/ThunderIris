Public Class paymentControl
    Private db As New TCESbaseEntities
    Private billId As Integer
    Private userName As String
    Private Sub paymentControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtId.Focus()
        lblCostumer.Text = FormMenu.clientName
        txtComments.Text = db.tcClients.Find(FormMenu.id).Comments
        cmbProduct.Items.Clear()
        For Each item As String In GetProducts()
            cmbProduct.Items.Add(item)
        Next
    End Sub

    Sub ShowTotal()
        Dim total As New Single
        For i = 0 To gridBill.Rows.Count - 1
            total += Single.Parse(gridBill(3, i).Value)
        Next
        txtTotal.Text = Format(total, "C2")
    End Sub

    Function GetProducts() As List(Of String)
        Dim list As New List(Of String)
        For Each item In db.tcProducts
            list.Add(item.Name)
        Next
        For Each item In db.tcServices
            list.Add(item.Name)
        Next
        Return list
    End Function

    Private Sub txtId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtId.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim product = findProduct(Integer.Parse(txtId.Text))
            If product IsNot Nothing Then
                cmbProduct.Text = product.Name
                txtPrice.Text = product.Price
            Else
                cmbProduct.Text = "No encontrado"
                txtPrice.Text = 0
            End If
        End If
    End Sub

    Function findProduct(ByVal id As Integer) As tcProduct
        Dim product As tcProduct = db.tcProducts.Find(id)
        Dim service As tcService
        If product Is Nothing Then
            service = db.tcServices.Find(id)
            If service IsNot Nothing Then
                product = New tcProduct
                product.Id = service.Id
                product.Name = service.Name
                product.Price = service.Price
            Else
                Return Nothing
            End If
        End If
        Return product
    End Function

    Function findProductByName(ByVal name As String) As tcProduct
        Dim products = From p In db.tcProducts Where p.Name.Contains(name) Select p
        Dim list = products.ToList
        Dim product As New tcProduct
        If list.Count > 0 Then
            product = list(0)
        Else
            Dim services = From s In db.tcServices Where s.Name.Contains(name)
            Dim slist = services.ToList
            Dim service As New tcService
            If slist.Count > 0 Then
                service = slist(0)
                product.Id = service.Id
                product.Name = service.Name
                product.Price = service.Price
            Else
                product = Nothing
            End If
        End If
        Return product
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        AddProduct()
        Clear()
        ShowTotal()
    End Sub

    Sub Clear()
        txtPrice.Text = 0
        txtQuantity.Value = 1
        txtId.Text = ""
        cmbProduct.Text = ""
        txtId.Focus()
    End Sub

    Sub AddProduct()
        gridBill.Rows.Add()
        Dim y = gridBill.Rows.Count - 1
        gridBill(0, y).Value = txtId.Text
        gridBill(1, y).Value = cmbProduct.Text
        gridBill(2, y).Value = txtQuantity.Value
        gridBill(3, y).Value = txtPrice.Text
        Dim p As Single = txtPrice.Text
        Dim q As Integer = txtQuantity.Value
        gridBill(4, y).Value = txtQuantity.Value * txtPrice.Text
    End Sub

    Private Sub cmbProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProduct.SelectedIndexChanged
        Dim product = findProductByName(cmbProduct.Text)
        If product IsNot Nothing Then
            txtId.Text = product.Id
            txtPrice.Text = product.Price
        End If
    End Sub

    Private Sub enterPress(sender As Object, e As KeyEventArgs) Handles txtId.KeyDown, txtPrice.KeyDown, txtQuantity.KeyDown, txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If SaveData() Then PrintBill() Else MsgBox("No se pudo guardar el registro.")
    End Sub

    Sub PrintBill()
        PrintPreviewDialog1.ShowDialog()
        Me.Hide()
    End Sub

    Function SaveData() As Boolean
        Try
            Dim bill = New tcBill
            bill.ClientId = FormMenu.id
            bill.IsCredit = False
            bill.PaymentMethod = 1

            db.tcBills.Add(bill)
            db.SaveChanges()

            For i = 0 To gridBill.Rows.Count - 1
                Dim id = gridBill(0, i).Value
                If IsProduct(id) Then
                    Dim sale = New tcProductSell()
                    sale.BillId = bill.Id
                    billId = bill.Id
                    sale.ProductId = Integer.Parse(id)
                    sale.Quantity = Integer.Parse(gridBill(2, i).Value)
                    sale.Price = Single.Parse(gridBill(3, i).Value)
                    db.tcProductSells.Add(sale)
                ElseIf IsService(id) Then
                    Dim sale = New tcServiceSell()
                    sale.BillId = bill.Id
                    sale.ServiceId = Integer.Parse(id)
                    sale.Quantity = Integer.Parse(gridBill(2, i).Value)
                    sale.Price = Single.Parse(gridBill(3, i).Value)
                    db.tcServiceSells.Add(sale)
                End If
            Next
            db.SaveChanges()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Function IsProduct(ByVal id As Integer) As Boolean
        If db.tcProducts.Find(id) IsNot Nothing Then Return True Else Return False
    End Function

    Function IsService(ByVal id As Integer) As Boolean
        If db.tcServices.Find(id) IsNot Nothing Then Return True Else Return False
    End Function

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim ps = New Printing.PaperSize
        ps.Height = 1200
        ps.Width = 850

        PrintDocument1.DefaultPageSettings.PaperSize = ps
        Dim font As Font

        'Header
        font = New Font("Arial Black", 13)
        e.Graphics.DrawString(FormMenu.id & " - " & lblCostumer.Text, font, Brushes.Black, 30, 10)
        font = New Font("Arial ", 12)
        e.Graphics.DrawString("# " & billId & userName & " - " & Now.ToShortDateString & " - " & Now.ToShortTimeString, font, Brushes.Black, 30, 30)

        'Grid
        Dim y = 50
        For i = 0 To gridBill.Rows.Count - 1
            e.Graphics.DrawString(gridBill(0, i).Value, font, Brushes.Black, 30, y)
            e.Graphics.DrawString(gridBill(1, i).Value, font, Brushes.Black, 100, y)
            e.Graphics.DrawString(gridBill(2, i).Value, font, Brushes.Black, 250, y)
            Dim price = Format(Single.Parse(gridBill(3, i).Value), "C2")
            e.Graphics.DrawString(price, font, Brushes.Black, 300, y)
            Dim total = Format(gridBill(4, i).Value, "C2")
            e.Graphics.DrawString(total, font, Brushes.Black, 400, y)
            y += 20
        Next
    End Sub

    Sub CalculateStock(ByVal product As tcProduct)

    End Sub
End Class
