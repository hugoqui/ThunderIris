Imports System.Text
Imports System.Xml
Imports Microsoft.AspNet.SignalR.Client
Imports Newtonsoft.Json

Public Class Form1

    Dim hubConnection = New HubConnection("http://admin.truecolorsguatemala.com/")
    Dim myHubProxy As IHubProxy = hubConnection.CreateHubProxy("ZionHub")

    Dim header = "TRUE COLORS ENGLISH SCHOOL"
    Dim bill = 0
    Dim client = ""
    Dim clientId = ""
    Dim typeOfBill = 1 ' 1 = products; 2 = services; 3 = abonos
    Dim user = ""
    Dim paymentMethod = "Efectivo" '0 efectivo; 1 Tarjeta; 2 cheque; 3 deposito
    Dim list = New List(Of Product)
    Dim isCredit As Boolean = False
    Dim isPayment As Boolean = False

    Function JsonRead(text As String)
        Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(text)
    End Function

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim prFont As New Font("Courier New", 10, FontStyle.Bold)
        Dim xPos As Single = 10
        Dim yPos As Single = prFont.GetHeight(e.Graphics)
        e.Graphics.DrawString(header, prFont, Brushes.Black, xPos, yPos)

        yPos = yPos + prFont.GetHeight(e.Graphics)
        Dim text = "[ " & bill & " ] - " & Now.Date.ToShortDateString & " - " & Now.Hour & ":" & Now.Minute & ":" & Now.Second
        e.Graphics.DrawString(text, prFont, Brushes.Black, xPos, yPos)

        yPos = yPos + prFont.GetHeight(e.Graphics) + 10
        prFont = New Font("Arial Narrow", 8, FontStyle.Bold)
        e.Graphics.DrawString(clientId & " - " & client, prFont, Brushes.Black, xPos, yPos)

        prFont = New Font("Courier New", 8)
        yPos = yPos + prFont.GetHeight(e.Graphics) + 5
        xPos = 10 : e.Graphics.DrawString("Cod", prFont, Brushes.Black, xPos, yPos)
        xPos = 40 : e.Graphics.DrawString("Nombre", prFont, Brushes.Black, xPos, yPos)
        xPos = 190 : e.Graphics.DrawString("Cnt", prFont, Brushes.Black, xPos, yPos)
        xPos = 215 : e.Graphics.DrawString("Prc", prFont, Brushes.Black, xPos, yPos)
        xPos = 265 : e.Graphics.DrawString("Total", prFont, Brushes.Black, xPos, yPos)
        Dim total = 0
        For Each item As Product In list
            yPos = yPos + prFont.GetHeight(e.Graphics)
            xPos = 10 : e.Graphics.DrawString(item.Id, prFont, Brushes.Black, xPos, yPos)
            xPos = 40 : e.Graphics.DrawString(item.Name, prFont, Brushes.Black, xPos, yPos)
            xPos = 190 : e.Graphics.DrawString(item.Quantity, prFont, Brushes.Black, xPos, yPos)
            xPos = 215 : e.Graphics.DrawString(item.Price, prFont, Brushes.Black, xPos, yPos)
            xPos = 265 : e.Graphics.DrawString(item.Total, prFont, Brushes.Black, xPos, yPos)
            total = total + item.Total
        Next

        prFont = New Font("Courier New", 8, FontStyle.Bold)
        yPos = yPos + prFont.GetHeight(e.Graphics) + 5
        xPos = 215 : e.Graphics.DrawString("Total", prFont, Brushes.Black, xPos, yPos)
        xPos = 265 : e.Graphics.DrawString(total, prFont, Brushes.Black, xPos, yPos)

        yPos = yPos + 20
        If isCredit Then
            prFont = New Font("Arial Black", 18, FontStyle.Bold)
            xPos = 100 : e.Graphics.DrawString("CREDITO", prFont, Brushes.Black, xPos, yPos)
            isCredit = False
        End If

        If isPayment Then
            xPos = 215 : e.Graphics.DrawString("SALDO", prFont, Brushes.Black, xPos, yPos)
            Dim balance = GetClientBalance(clientId)
            xPos = 265 : e.Graphics.DrawString(balance, prFont, Brushes.Black, xPos, yPos)
            isPayment = False
        End If

        prFont = New Font("Courier New", 8, FontStyle.Bold)
        xPos = 10 : e.Graphics.DrawString(paymentMethod, prFont, Brushes.Black, xPos, 450)
        xPos = 200 : e.Graphics.DrawString(user, prFont, Brushes.Black, xPos, 450)

        e.HasMorePages = False
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        myHubProxy.On(Of Integer)("printBill", Function(billId) PrintBill(billId))
        hubConnection.Start().Wait()
    End Sub

    Function PrintBill(billId As Integer)
        bill = billId
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("http://admin.truecolorsguatemala.com/api/sales/getBillDetails/" & bill)
        Dim jsonBill = JsonRead(result)

        If jsonBill("tcServiceSales").count > 0 Then
            typeOfBill = 2
        ElseIf jsonBill("tcClientDetails").count > 0 Then
            typeOfBill = 3
        Else
            typeOfBill = 1
        End If

        clientId = jsonBill("ClientId")
        client = jsonBill("tcClient")("Name")
        user = jsonBill("User")
        isCredit = jsonBill("IsCredit")

        Select Case jsonBill("PaymentMethod")
            Case 1 : paymentMethod = "Tarjeta"
            Case 2 : paymentMethod = "Cheque"
            Case 3 : paymentMethod = "Deposito"
            Case Else : paymentMethod = "Efectivo"
        End Select

        list = New List(Of Product)
        Select Case typeOfBill
            Case 1 'products
                For Each item In jsonBill("tcProductSales")
                    Dim product = New Product
                    product.Id = item("ProductId")
                    product.Name = item("tcProduct")("Name")
                    product.Price = item("Price")
                    product.Quantity = item("Quantity")
                    product.Total = product.Price * product.Quantity
                    list.Add(product)
                Next
            Case 2 'services
                For Each item In jsonBill("tcServiceSales")
                    Dim service = New Product
                    service.Id = item("ServiceId")
                    service.Name = item("tcService")("Name")
                    service.Price = item("Price")
                    service.Quantity = item("Quantity")
                    service.Total = service.Price * service.Quantity
                    list.Add(service)
                Next
            Case Else 'payments
                If isCredit Then
                    For Each item In jsonBill("tcProductSales")
                        Dim product = New Product
                        product.Id = item("ProductId")
                        product.Name = item("tcProduct")("Name")
                        product.Price = item("Price")
                        product.Quantity = item("Quantity")
                        product.Total = product.Price * product.Quantity
                        list.Add(product)
                    Next
                Else
                    For Each item In jsonBill("tcClientDetails")
                        Dim product = New Product
                        product.Id = 0
                        product.Name = "Abono"
                        product.Price = item("Payment")
                        product.Quantity = 1
                        product.Total = product.Price * product.Quantity
                        list.Add(product)
                    Next
                    isPayment = True
                End If
        End Select

        Dim customSize As Printing.PaperSize
        customSize = New Printing.PaperSize("BillSize", 340, 500)
        PrintDocument1.DefaultPageSettings.PaperSize = customSize
        PrintDocument1.DefaultPageSettings.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
        'PrintPreviewDialog1.ShowDialog()
        PrintDocument1.Print()
        Return "Done"
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Val(TextBox1.Text) > 0 Then
            PrintBill(TextBox1.Text)
        Else
            MsgBox("Debe ser un número", MsgBoxStyle.Information, "Zion-Printer")
        End If
        TextBox1.Text = "" : TextBox1.Focus()
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            ZionNotify.Visible = True
            ZionNotify.BalloonTipIcon = ToolTipIcon.Info
            ZionNotify.BalloonTipTitle = "Impresion ZionOnline"
            ZionNotify.BalloonTipText = "La impresora está corriendo en segundo plano"
            ZionNotify.ShowBalloonTip(50000)
            'Me.Hide()
            ShowInTaskbar = False
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ZionNotify.DoubleClick
        'Me.Show()
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        ZionNotify.Visible = False
    End Sub

    Function GetClientBalance(ByVal id As String) As Integer
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("http://admin.truecolorsguatemala.com/api/sales/getClientBalance/" & id)
        Return result
    End Function

End Class


Public Class Product
    Private _id As Integer
    Public Property Id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _price As Decimal
    Public Property Price() As Decimal
        Get
            Return _price
        End Get
        Set(ByVal value As Decimal)
            _price = value
        End Set
    End Property
    Private _qnt As Integer
    Public Property Quantity() As Integer
        Get
            Return _qnt
        End Get
        Set(ByVal value As Integer)
            _qnt = value
        End Set
    End Property
    Private _total As Decimal
    Public Property Total() As Decimal
        Get
            Return _total
        End Get
        Set(ByVal value As Decimal)
            _total = value
        End Set
    End Property
End Class