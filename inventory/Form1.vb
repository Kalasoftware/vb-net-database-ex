Imports System.Data.OleDb
Imports System.Reflection.Emit
Public Class Form1
    Dim connection As OleDbConnection
    Dim ds As DataSet

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim connection_string As New String("Data Source=localhost;" +
                                             "password=it4; User id=it4; " +
                                             "Provider=ORAOLEDB.Oracle")
        connection = New OleDbConnection(connection_string)

        fillDataGrid()
        fillcompanyc()
    End Sub

    Private Sub fillcompanyc()
        Dim dataAdapter As New OleDbDataAdapter("Select * from COMPANY_C", connection)
        dataAdapter.Fill(ds, "company_C")
        cmpbox.DataSource = ds.Tables("company_c")
        cmpbox.DisplayMember = "CNAME"
        cmpbox.ValueMember = "CID"


    End Sub

    Private Sub fillDataGrid()
        ds = New DataSet
        Dim dataAdapter As New OleDbDataAdapter _
            ("Select * from INVENTORY_C", connection)
        dataAdapter.Fill(ds)
        DataGridView1.DataSource = ds.Tables(0)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            connection.Open()
            Dim qry As String
            qry = "INSERT INTO INVENTORY_C " &
 "( ITEM_NAME, QUANTITY, TOTAL_PRICE, PAYMENT_DATE, DELIVERY_DATE,COMPANY_ID) 
VALUES ( ?, ?, ?, TO_DATE(?, 'YYYY-MM-DD'), TO_DATE(?, 'YYYY-MM-DD')),?"

            Dim command As New OleDbCommand(qry, connection)
            command.Parameters.AddWithValue("?", ComboBox1.SelectedItem.ToString)
            command.Parameters.AddWithValue("?", NumericUpDown1.Value)
            command.Parameters.AddWithValue("?", CInt(txt_price.Text))
            ' command.Parameters.AddWithValue("?", txt_company.Text)
            command.Parameters.AddWithValue("?", DateTimePicker1.Value.ToString("yyyy-MM-dd"))
            command.Parameters.AddWithValue("?", DateTimePicker2.Value.ToString("yyyy-MM-dd"))
            command.Parameters.AddWithValue("?", cmpbox.SelectedValue)


            Dim insrow = command.ExecuteNonQuery()
            connection.Close()
            If (insrow >= 1) Then
                MsgBox("Data Inserted")
                fillDataGrid()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        Try

            Label8.Text = DataGridView1.SelectedRows(0).Cells(0).Value
            ComboBox1.SelectedItem = DataGridView1.SelectedRows(0).Cells(1).Value
            NumericUpDown1.Value = DataGridView1.SelectedRows(0).Cells(2).Value
            txt_price.Text = DataGridView1.SelectedRows(0).Cells(3).Value
            txt_company.Text = DataGridView1.SelectedRows(0).Cells(4).Value
            DateTimePicker1.Value = DataGridView1.SelectedRows(0).Cells(5).Value
            DateTimePicker2.Value = DataGridView1.SelectedRows(0).Cells(6).Value
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try

            connection.Open()
            Dim updateQry = "Update INVENTORY_C SET ITEM_NAME=?, QUANTITY=?, TOTAL_PRICE =?, PAYMENT_DATE=?, DELIVERY_DATE =?, COMPANY_ID=? 
WHERE IID=?"
            Dim cmd As New OleDbCommand(updateQry, connection)

            cmd.Parameters.AddWithValue("?", ComboBox1.SelectedItem)
            cmd.Parameters.AddWithValue("?", NumericUpDown1.Value)
            cmd.Parameters.AddWithValue("?", CInt(txt_price.Text))

            cmd.Parameters.AddWithValue("?", DateTimePicker1.Value.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("?", DateTimePicker2.Value.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("?", cmpbox.SelectedValue)
            cmd.Parameters.AddWithValue("?", Label8.Text)

            Dim res = cmd.ExecuteNonQuery()
            connection.Close()

            If (res >= 1) Then
                MsgBox("Data updated")
                fillDataGrid()
            End If


        Catch ex As Exception

        End Try


    End Sub

    Private Sub cmpbox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmpbox.SelectedIndexChanged



    End Sub
End Class


