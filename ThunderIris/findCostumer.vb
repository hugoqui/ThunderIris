
Public Class findCostumer
    Private db As New TCESbaseEntities

    Public id As Integer

    Private Sub TextBox2_KeyDown(sender As Object, e As KeyEventArgs) Handles txtName.KeyDown
        If e.KeyCode = Keys.Enter Then FilterByName(txtName.Text)
    End Sub

    Sub FilterByName(ByVal name As String)
        Dim list = db.tcClients.AsQueryable().Where(Function(client) client.Name.Contains(name))
        gridClients.DataSource = list.ToList
    End Sub

    Sub FindById(ByVal id As Integer)
        Dim list = db.tcClients.AsQueryable().Where(Function(client) client.Id = id)
        gridClients.DataSource = list.ToList
    End Sub
    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles txtId.KeyDown
        If e.KeyCode = Keys.Enter Then
            FindById(txtId.Text)
            If gridClients.Rows.Count > 0 Then
                FormMenu.id = txtId.Text
                FormMenu.clientName = gridClients(1, 0).Value
                FormMenu.AddControl(New paymentControl)
            Else
                MsgBox("No se encontró el código")
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        If txtName.Text <> "" Then
            FilterByName(txtName.Text)
        ElseIf txtId.Text <> "" Then
            FindById(txtId.Text)
        End If
    End Sub

    Private Sub findCostumer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtId.Focus()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridClients.CellContentClick
        FormMenu.id = gridClients(0, gridClients.CurrentRow.Index).Value
        FormMenu.clientName = gridClients(1, gridClients.CurrentRow.Index).Value
        FormMenu.AddControl(New paymentControl)
    End Sub
End Class
