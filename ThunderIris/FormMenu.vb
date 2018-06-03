
Public Class FormMenu
    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    Private settingsApp As aspWebSite

    Public id As Integer, clientName As String

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, titlePanel.MouseDown
        drag = True
        mousex = Windows.Forms.Cursor.Position.X - Me.Left
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub
    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove, titlePanel.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub
    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp, titlePanel.MouseUp
        drag = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnTransactions.Click
        If purchasePanel.Visible = True Then purchasePanel.Visible = False Else purchasePanel.Visible = True
        If salesPanel.Visible = True Then salesPanel.Visible = False Else salesPanel.Visible = True

        salesReportPanel.Visible = False : purchasesReportPanel.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        salesPanel.Visible = False : purchasePanel.Visible = False

        If salesReportPanel.Visible = True Then salesReportPanel.Visible = False Else salesReportPanel.Visible = True
        If purchasesReportPanel.Visible = True Then purchasesReportPanel.Visible = False Else purchasesReportPanel.Visible = True
    End Sub


    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        KillControls() ' remove any control in the contentPanel
        settingsApp = New aspWebSite
        settingsApp.Dock = DockStyle.Fill
        contentPanel.Controls.Add(settingsApp)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        End
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Me.WindowState = FormWindowState.Normal Then Me.WindowState = FormWindowState.Maximized Else Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Dim findCostumer As findCostumer
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnSell.Click
        KillControls() ' remove any control in the contentPanel
        findCostumer = New findCostumer
        findCostumer.Dock = DockStyle.Fill
        contentPanel.Controls.Add(findCostumer)
    End Sub

    Sub KillControls()
        For Each control As Control In contentPanel.Controls
            contentPanel.Controls.Remove(control)
        Next
    End Sub

    Public Sub AddControl(ByVal control)
        KillControls()
        contentPanel.Controls.Add(control)
    End Sub

    Private Sub btnPurchase_Click(sender As Object, e As EventArgs) Handles btnPurchase.Click

    End Sub
End Class
