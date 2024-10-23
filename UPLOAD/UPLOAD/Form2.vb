' Name    : Rahmandhani Prihartono
' Created : Wednesday, 23 Oct 2024

Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.Design
Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.Net.NetworkInformation

Public Class Form2
    Private Function CheckApplicationVersion(fileName As String) As Boolean
        Dim connectionString As String = "Data Source=MYKULWSPC000404.kul-dc.dhl.com,1525;Initial Catalog=LOREAL;Persist Security Info=True;User ID=dscidfin_db;Password=K3pit1ngR3bus23!"
        Dim isVersionValid As Boolean = False

        Dim query As String = "SELECT version FROM generic_configs WHERE filename = @filename"

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@filename", fileName)
                Try
                    connection.Open()
                    Dim version As String = command.ExecuteScalar()?.ToString

                    'Checking if the version matches the expected one
                    If version IsNot Nothing AndAlso version = "1.4" Then
                        isVersionValid = True
                    Else
                        MessageBox.Show($"application tools upload version is not valid. Version 1.4 is expected, but version {version} was found. Please contact IT for assitance")
                    End If
                Catch ex As Exception
                    MessageBox.Show("Terjadi kesalahan saat mengecek versi aplikasi: " & ex.Message)
                End Try
            End Using
        End Using
        Return isVersionValid
    End Function
    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim openFileDialog As New OpenFileDialog
        openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx"
        openFileDialog.Title = "Pilih file CSV atau Excel"

        If openFileDialog.ShowDialog = DialogResult.OK Then
            txtFilePath.Text = openFileDialog.FileName
        End If
    End Sub
    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim filePath = txtFilePath.Text
        Dim fileName = Path.GetFileName(filePath)

        If Not CheckApplicationVersion("Upload To DB L'oreal - Admin") Then
            Return 'If version not match, process stop'
        End If

        If String.IsNullOrEmpty(filePath) Then
            MessageBox.Show("Select a file before uploading")
            Return
        End If

        ' Regular expression untuk memvalidasi nama file yang valid
        Dim patternStock = "^stock_on_hands.*"
        Dim patternItems = "^items.*"
        Dim patternLocations = "^locations.*"
        Dim patternUser = "^user_area_assignments.*"

        ' Tambahkan validasi untuk user_area_assignments
        If (Regex.IsMatch(fileName, patternStock) OrElse Regex.IsMatch(fileName, patternItems) OrElse Regex.IsMatch(fileName, patternLocations) OrElse Regex.IsMatch(fileName, patternUser)) AndAlso filePath.EndsWith(".csv") Then
            ' Lakukan truncate table sebelum upload
            TruncateTable(fileName)

            ' Panggil fungsi untuk upload file CSV ke SQL Server
            UploadCsvToSqlServer(filePath)
        Else
            MessageBox.Show("Nama file tidak valid. Pastikan file adalah stock_on_hands, items, locations, atau user_area_assignments.")
        End If
    End Sub

    Private Sub TruncateTable(fileName As String)
        Dim connectionString As String = "Data Source=MYKULWSPC000404.kul-dc.dhl.com,1525;Initial Catalog=LOREAL;Persist Security Info=True;User ID=dscidfin_db;Password=K3pit1ngR3bus23!"
        Dim tableName As String = ""

        ' Tentukan tabel berdasarkan nama file
        If fileName.StartsWith("stock_on_hands") Then
            tableName = "stock_on_hands"
        ElseIf fileName.StartsWith("items") Then
            tableName = "items"
        ElseIf fileName.StartsWith("locations") Then
            tableName = "locations"
        ElseIf fileName.StartsWith("user_area_assignments") Then
            tableName = "user_area_assignments"
        End If

        If String.IsNullOrEmpty(tableName) Then Return

        ' Truncate tabel yang sesuai
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim truncateQuery As String = $"TRUNCATE TABLE {tableName}"
                Using command As New SqlCommand(truncateQuery, connection)
                    command.ExecuteNonQuery()
                End Using
                MessageBox.Show($"Data di tabel {tableName} berhasil dihapus")
            Catch ex As Exception
                MessageBox.Show("Terjadi kesalahan saat menghapus data tabel: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub UploadCsvToSqlServer(csvFilePath As String)
        Dim connectionString As String = "Data Source=MYKULWSPC000404.kul-dc.dhl.com,1525;Initial Catalog=LOREAL;Persist Security Info=True;User ID=dscidfin_db;Password=K3pit1ngR3bus23!"
        Try
            ' Cek apakah file CSV ada
            If Not File.Exists(csvFilePath) Then
                MessageBox.Show("File CSV tidak ditemukan: " & csvFilePath)
                Return
            End If

            ' Dapatkan nama file tanpa ekstensi
            Dim fileName As String = Path.GetFileNameWithoutExtension(csvFilePath).ToUpper()
            Dim tableName As String = ""

            ' Tentukan tabel berdasarkan nama file
            If fileName.StartsWith("STOCK_ON_HANDS") Then
                tableName = "stock_on_hands"
            ElseIf fileName.StartsWith("ITEMS") Then
                tableName = "items"
            ElseIf fileName.StartsWith("LOCATIONS") Then
                tableName = "locations"
            ElseIf fileName.StartsWith("USER_AREA_ASSIGNMENTS") Then
                tableName = "user_area_assignments"
            Else
                MessageBox.Show("Nama file tidak dikenali untuk tabel SQL: " & fileName)
                Return
            End If

            ' Baca data dari file CSV ke dalam DataTable
            Dim dataTable As New DataTable()
            Using csvReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(csvFilePath)
                csvReader.TextFieldType = FileIO.FieldType.Delimited
                csvReader.SetDelimiters(",")

                ' Baca header dari file CSV dan tambahkan kolom ke DataTable
                Dim headerFields As String() = csvReader.ReadFields()
                For Each headerField As String In headerFields
                    dataTable.Columns.Add(New DataColumn(headerField))
                Next

                Dim currentLine As Integer = 0
                Dim totalLines As Integer = File.ReadAllLines(csvFilePath).Length - 1 'Mengurangi 1 untuk header
                ProgressBar1.Maximum = totalLines

                ' Baca data dari file CSV dan masukkan ke DataTable
                While Not csvReader.EndOfData
                    Dim dataFields As String() = csvReader.ReadFields()
                    dataTable.Rows.Add(dataFields)

                    For i As Integer = 0 To dataFields.Length - 1
                        If String.IsNullOrWhiteSpace(dataFields(i)) Then
                            dataFields(i) = DBNull.Value.ToString()
                        End If
                    Next

                    dataTable.Rows.Add(dataFields)

                    'Update ProgressBar dan label persentasi
                    currentLine += 1
                    ProgressBar1.Value = currentLine
                    lblProgressPercentage.Text = Math.Round((currentLine / totalLines) * 100, 2).ToString() & "%"
                    Application.DoEvents() 'Refresh UI agar progressBar diperbarui
                End While
            End Using

            ' Upload data ke SQL Server jika ada data
            If dataTable.Rows.Count > 0 Then
                UploadDataTableToSqlServer(dataTable, tableName, connectionString)
            End If

            'Update progress setelah upload selesai
            ProgressBar1.Value = ProgressBar1.Maximum

            ' Menampilkan jumlah baris yang diunggah
            Dim rowCount As Integer = dataTable.Rows.Count
            MessageBox.Show("Data CSV berhasil diupload ke tabel " & tableName & " di SQL Server." & vbCrLf & "Jumlah baris yang diunggah: " & rowCount.ToString())
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan: " & ex.Message)
        End Try
    End Sub
    Private Sub UploadDataTableToSqlServer(dataTable As DataTable, tableName As String, connectionString As String)
        Try
            Using sqlConnection As New SqlConnection(connectionString)
                sqlConnection.Open()

                Using bulkCopy As New SqlBulkCopy(sqlConnection)
                    bulkCopy.DestinationTableName = tableName
                    bulkCopy.BatchSize = 10000
                    bulkCopy.BulkCopyTimeout = 0

                    ' Map kolom dari DataTable ke tabel SQL Server
                    For Each column As DataColumn In dataTable.Columns
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName)
                    Next

                    ' Upload data ke SQL Server
                    bulkCopy.WriteToServer(dataTable)
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error uploading data to table " & tableName & ": " & ex.Message)
        End Try
    End Sub
    Private Sub UploadSheetToSqlServer(sheetName As String, destinationTableName As String, excelConnection As OleDbConnection, sqlConnectionString As String)
        Dim query As String = "SELECT * FROM [" & sheetName & "]"
        Dim cmd As New OleDbCommand(query, excelConnection)
        Dim adapter As New OleDbDataAdapter(cmd)

        Dim dataTable As New DataTable()
        adapter.Fill(dataTable)

        Dim batchSize As Integer = 10000 ' Tentukan Ukuran Batch
        Dim totalRows As Integer = dataTable.Rows.Count
        Dim currentRow As Integer = 0
        Dim totalRowsUploaded As Integer = 0 'variabel untuk menyimpan jumlah total baris yang diupload

        ' Set ProgressBar untuk sheet ini
        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = totalRows

        Using sqlConnection As New SqlConnection(sqlConnectionString)
            sqlConnection.Open()

            While currentRow < totalRows
                ' Buat DataTable baru untuk batch
                Dim batchTable As DataTable = dataTable.Clone()

                ' Tambahkan baris ke batchTable
                For i As Integer = 0 To batchSize - 1
                    If currentRow < totalRows Then
                        batchTable.ImportRow(dataTable.Rows(currentRow))
                        currentRow += 1
                    Else
                        Exit For
                    End If
                Next

                ' Menggunakan sqlBulkCopy untuk upload batch
                Using bulkCopy As New SqlBulkCopy(sqlConnection)
                    bulkCopy.DestinationTableName = destinationTableName
                    bulkCopy.BatchSize = batchSize ' Set Ukuran batch
                    bulkCopy.WriteToServer(batchTable)
                    totalRowsUploaded += batchTable.Rows.Count
                End Using

                ' Update ProgressBar dan Label setelah setiap batch
                ProgressBar1.Value = currentRow
                UpdateProgressPercentage(currentRow, totalRows)

                ' Optional: tampilkan status upload batch
                Console.WriteLine($"Batch uploaded to {destinationTableName}. Rows uploaded: {currentRow}/{totalRows}")
            End While

        End Using
        ' Tampilkan pesan jumlah total baris yang berhasil diupload setelah selesai
        MessageBox.Show($"Jumlah total baris yang berhasil diupload ke {destinationTableName}: {totalRowsUploaded}")
    End Sub
    Private Sub UpdateProgressPercentage(currentRow As Integer, totalRows As Integer)
        Dim progressPercentage As Integer = CInt((currentRow / totalRows) * 100)
        lblProgressPercentage.Text = $"{progressPercentage}%"
        Application.DoEvents() ' This line allows UI to update
    End Sub
    Private Sub txtFilePath_TextChanged(sender As Object, e As EventArgs) Handles txtFilePath.TextChanged
        txtFilePath.Enabled = False
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'Truncate Data : task_headers,task_details & stock_counts
        If Not CheckApplicationVersion("Upload To DB L'oreal - Admin") Then
            Return
        End If

        Dim inputPassword As String = InputBox("Please enter admin password:", "Password Required")

        If inputPassword <> "IndonesiaMerdeka2024" Then
            MessageBox.Show("Invalid password. Operation canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Stop process if password is incorrect
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure to clean data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim connectionString As String = "Data Source=MYKULWSPC000404.kul-dc.dhl.com,1525;Initial Catalog=LOREAL;Persist Security Info=True;User ID=dscidfin_db;Password=K3pit1ngR3bus23!"

            Using conn As New SqlConnection(connectionString)
                Dim transaction As SqlTransaction = Nothing

                Try
                    conn.Open()

                    ' Mulai transaksi
                    transaction = conn.BeginTransaction()

                    ' Truncate task_headers
                    Dim truncateTaskHeaders As New SqlCommand("TRUNCATE TABLE task_headers;", conn, transaction)
                    truncateTaskHeaders.CommandTimeout = 1200
                    truncateTaskHeaders.ExecuteNonQuery()

                    ' Truncate task_details
                    Dim truncateTaskDetails As New SqlCommand("TRUNCATE TABLE task_details;", conn, transaction)
                    truncateTaskDetails.CommandTimeout = 1200
                    truncateTaskDetails.ExecuteNonQuery()

                    ' Truncate stock_count
                    Dim truncateStockCount As New SqlCommand("TRUNCATE TABLE stock_counts;", conn, transaction)
                    truncateStockCount.CommandTimeout = 1200
                    truncateStockCount.ExecuteNonQuery()

                    ' Commit transaksi
                    transaction.Commit()

                    MessageBox.Show("Clean Data Has Been Succesfully.")
                Catch ex As SqlException
                    ' Rollback jika ada error
                    If transaction IsNot Nothing Then
                        transaction.Rollback()
                    End If
                    MessageBox.Show("Error: " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End Using
        Else
            MessageBox.Show("Clean data operation has been cancelled")
        End If
    End Sub
End Class