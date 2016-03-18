Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO.Ports
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data

Public Class Form1
    Dim receiveByte() As Byte
    Dim dt As New DataTable
    Dim ss() As String

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  
        Try
            With dt.Columns
                .Add("id_device")
                .Add("type_device")
                .Add("data1")
                .Add("data2")
                .Add("data3")
                .Add("time")
            End With

            ss = {22, "ph", 33, 44, 55, Date.Now}

            dt.Rows.Add(ss)

            With DataGridView1
                .DataSource = dt.DefaultView()
                .DataSource = dt
                '.Columns("星期1").Width = 60
                .RowHeadersWidth = 80
                '.Rows(0).HeaderCell.Value = "上午1"

                '.Columns("id").Visible = False
            End With

        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

 
        Try
            Dim nunmberStr(11) As String
            Dim points() As String
            Dim s(43) As String

            s(42) = 255
            s(43) = 255


            '将文本框中的文字以write方法自串行端口送出
            '以ReadExisting方法自串行端口读取数据




            'For i = 0 To 43
            '    TextBox1.Text = TextBox1.Text & " " & s(i)
            'Next




            Dim sendBts() As Byte
            ' Dim receiveBts() As Byte
            For i = 0 To 43
                ReDim Preserve sendBts(i)
                sendBts(i) = Val(s(i))
            Next
            ToolStripStatusLabel1.Text = "正在发送数据！"
            RS232.Write(sendBts, 0, sendBts.Length)    '送出命令
            ToolStripStatusLabel1.Text = "数据发送成功！"
        Catch ex As Exception
            ToolStripStatusLabel1.Text = "出现错误，请重试！"
        End Try
       




    End Sub


    Private Function Ints(ByVal text As String)
        Dim a As String
        a = Int(Val(text) / 256)
        Return a
    End Function
    Private Function Mods(ByVal text As String)
        Dim a As String
        a = (Val(text) Mod 256)
        Return a
    End Function
    Private Function change(ByVal s As String) As String
        change = AscW(s)
        If change < 0 Then
            change = change + 65536
        End If
    End Function

    Private Function change2(ByVal s As String) As String
        change2 = Asc(s)
    End Function


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try

            If Button3.Text = "检测串口" Then

                If SerialPort.GetPortNames.Length = 0 Then
                    ToolStripStatusLabel1.Text = "通信端口未获取，请重试！"
                    Exit Sub
                End If

                For Each sp As String In SerialPort.GetPortNames   '使用GetPortNames取得计算机中可用的通信端口名称
                    ComboBox1.Items.Add(sp)        '将可用的通信端口名称输入combox1组件中
                Next
                ComboBox1.Sorted = True         '对可用的通信端口名称进行排序
                ComboBox1.SelectedIndex = 0     '第一个为默认选项
                Button3.Text = "打开串口"
                ToolStripStatusLabel1.Text = "通信端口已获取，请打开！"
            ElseIf Button3.Text = "关闭串口" Then
                If RS232 Is Nothing OrElse Not RS232.IsOpen Then        '还未打开
                    ToolStripStatusLabel1.Text = "通信端口还未打开！"

                Else
                    RS232.Close()      '关闭RS232
                    ToolStripStatusLabel1.Text = "通信端口已关闭！"
                    btnSend.Enabled = False
                    Button3.Text = "检测串口"     '关闭传送按钮
                End If
            ElseIf Button3.Text = "打开串口" Then
                Dim mportname As String
                'Dim buf As String
                mportname = ComboBox1.SelectedItem.ToString    '欲打开的通信端口
                RS232.PortName = mportname        '设置打开的通信端口
                RS232.Encoding = Encoding.ASCII   '设置编码方式为ASCII
                If Not RS232.IsOpen Then     '还未打开
                    RS232.Open()       '打开通信端口
                    btnSend.Enabled = True   '启用传送按钮
                    ToolStripStatusLabel1.Text = "已打开通信端口！"
                    Button3.Text = "关闭串口"
                Else

                    ToolStripStatusLabel1.Text = "通信端口打开错误！"
                    End

                End If



            End If
        Catch ex As Exception
            ToolStripStatusLabel1.Text = ex.Message
        End Try
      







    End Sub

    Private Sub TimeDelay(ByVal DT As Integer)
        Dim StartTick As Integer
        StartTick = Environment.TickCount()     '开始计数前的tick
        Do
            If Environment.TickCount() - StartTick >= DT Then Exit Do
            Application.DoEvents()     '处理队列里的信息
        Loop
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If Not RS232 Is Nothing Then         '判断是否已创建通信对象
            If RS232.IsOpen Then RS232.Close() '若已打开，就将其关闭
        End If
        End
    End Sub


    Private Sub numberBox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ToolStripStatusLabel1.Text = "可输入11位号码！如：12345678909"

    End Sub

 


 
    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim reciveStr As String
        Dim s As String()
        With ofD
            .Filter = "txt文件（*.txt）|*.txt"
            .FilterIndex = 1
            .Title = "打开文件"
            .FileName = Nothing
        End With
        If ofD.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim sr As StreamReader = New StreamReader(ofD.FileName, System.Text.Encoding.Default)

                reciveStr = Replace(Trim(sr.ReadToEnd), Chr(13) + Chr(10), "")
                s = reciveStr.Split(";")

                For i = 0 To s.Length - 1
                    input(s(i))
                Next
                ToolStripStatusLabel1.Text = "导入成功！"
            Catch ex As Exception
                ToolStripStatusLabel1.Text = ex.Message
            End Try
        End If
    End Sub
    Private Function input(ByVal a As String)
        Dim r As String()

        r = a.Split(":")
        Select Case r(0)
            'Case "手机号码"
            '    numberBox.Text = r(1)
            'Case "目标IP1"
            '    ipBox1.Text = r(1)
            'Case "目标IP2"
            '    ipBox2.Text = r(1)
            'Case "目标IP3"
            '    ipBox3.Text = r(1)
            'Case "目标IP4"
            '    ipBox4.Text = r(1)
            'Case "端口号"
            '    portBox.Text = r(1)
            'Case "组电压上限"
            '    vsUp.Text = r(1)
            'Case "组电压下限"
            '    vsDown.Text = r(1)
            'Case "组电流上限"
            '    isUp.Text = r(1)
            'Case "组电流下限"
            '    isDown.Text = r(1)
            'Case "电池电压上限"
            '    btyVUp.Text = r(1)

            'Case "电池电压下限"
            '    btyVDown.Text = r(1)
            'Case "电池温度上限"
            '    btyHUp.Text = r(1)
            'Case "电池温度下限"
            '    btyHDown.Text = r(1)
            'Case "电池内阻上限"
            '    btyRUp.Text = r(1)
            'Case "电池内阻下限"
            '    btyRDown.Text = r(1)

        End Select
    End Function

    Private Sub btnOutport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutport.Click
        Dim sumStr As String
        With SaveFileDialog1
            .Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
            .FilterIndex = 1
            .Title = "保存文件"
            .FileName = Nothing
        End With
        ' SaveFileDialog1.ShowDialog()
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                '            sumStr = "手机号码:" & numberBox.Text & ";" & vbCrLf & "目标IP1:" & _
                'ipBox1.Text & ";" & vbCrLf & "目标IP2:" & ipBox2.Text & ";" & vbCrLf & "目标IP3:" & ipBox3.Text & ";" & vbCrLf & _
                '"目标IP4:" & ipBox4.Text & ";" & vbCrLf & "端口号:" & portBox.Text & ";" & vbCrLf & "组电压上限:" & _
                'vsUp.Text & ";" & vbCrLf & "组电压下限:" & vsDown.Text & ";" & vbCrLf & "组电流上限:" & isUp.Text & ";" & vbCrLf & _
                '"组电流下限:" & isDown.Text & ";" & vbCrLf & "电池电压上限:" & btyVUp.Text & ";" & vbCrLf & "电池电压下限:" & _
                'btyVDown.Text & ";" & vbCrLf & "电池温度上限:" & btyHUp.Text & ";" & vbCrLf & "电池温度下限:" & btyHDown.Text & ";" & vbCrLf & _
                '"电池内阻上限:" & btyRUp.Text & ";" & vbCrLf & "电池内阻下限:" & btyRDown.Text & ";" & vbCrLf

                My.Computer.FileSystem.WriteAllText(SaveFileDialog1.FileName, sumStr, True)
                ToolStripStatusLabel1.Text = "导出成功"
            Catch ex As Exception
                ToolStripStatusLabel1.Text = ex.Message
            End Try
        End If
       

    End Sub



 

    Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If Not RS232 Is Nothing Then         '判断是否已创建通信对象
            If RS232.IsOpen Then RS232.Close() '若已打开，就将其关闭
        End If
    End Sub

    Public Sub RS232_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles RS232.DataReceived
        Me.Invoke(New EventHandler(AddressOf Sp_receiving))
    End Sub
    Private Sub Sp_receiving(ByVal sender As Object, ByVal e As EventArgs)
        Dim InString As String
        Try
            RevBytes.Text = Str(Val(RevBytes.Text) + RS232.BytesToRead)

            If RS232.BytesToRead > 0 Then
                Threading.Thread.Sleep(100)
                InString = RS232.ReadExisting.ToString
                RS232.DiscardInBuffer()
                RevBytes.Text = InString & vbCrLf
            End If

        Catch ex As Exception
            ToolStripStatusLabel1.Text = ex.Message
        End Try
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim testText As String
        Dim testTexts As String

        Dim pattern As String = "\?\w{12}\!"
        Dim s() As String
        testText = "?PH0133445566!"
        testTexts = "66!?PH0133445566!?PH0133345566!?PH0133435566!?PH0133443566!?PH0133445366!?PH0133445536!?12"
        ' s() = Regex.Matches(testTexts, pattern)
        Dim mc As MatchCollection
        mc = Regex.Matches(testTexts, pattern)

        For i = 0 To mc.Count - 1 Step 1
            ReDim Preserve s(i)
            s(i) = mc(i).Value
            RevBytes.Text += s(i)
        Next

        '对S（i）中的数据切割为ss，分别填充到图表中，上传数据库，上位机部分结束

        Call Button4_Click()

    End Sub

    Private Sub Button4_Click() Handles Button4.Click
        Dim dt As DataTable
        dt = CreateDataTable()

        Chart1.DataSource = dt


        Chart1.Series(0).YValueMembers = "Volume1"
        Chart1.Series(1).YValueMembers = "Volume2"
        Chart1.Series(0).XValueMember = "Date"


        Chart1.DataBind()
    End Sub
    Private Function CreateDataTable()
        Dim dt As New DataTable
        dt.Columns.Add("Date")
        dt.Columns.Add("Volume1")
        dt.Columns.Add("Volume2")
        Dim dr As DataRow
        dr = dt.NewRow()
        dr("Date") = "Jan"
        dr("Volume1") = 3731
        dr("Volume2") = 4101
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Date") = "Feb"
        dr("Volume1") = 6024
        dr("Volume2") = 4324
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Date") = "Mar"
        dr("Volume1") = 4935
        dr("Volume2") = 2935
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Date") = "Apr"
        dr("Volume1") = 4466
        dr("Volume2") = 5644
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Date") = "May"
        dr("Volume1") = 5117
        dr("Volume2") = 5671
        dt.Rows.Add(dr)

        dr = dt.NewRow()
        dr("Date") = "Jun"
        dr("Volume1") = 3546
        dr("Volume2") = 4646
        dt.Rows.Add(dr)

        Return dt
    End Function

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            Dim connStr As String
            Dim conn As MySqlConnection
            If Not conn Is Nothing Then conn.Close()
            connStr = "Server=qdm170159190.my3w.com;uid=qdm170159190;pwd=chl288842;database=qdm170159190_db"
            'connStr = "Server=localhost;uid=root;pwd=;database=cc"
            conn = New MySqlConnection(connStr)

            conn.Open()

            Dim query_add As String = "INSERT INTO c_data(id_device,type_device,data1,data2,data3,time) VALUES('" + ss(0) + "','" + ss(1) + "','" + ss(2) + "','" + ss(3) + "','" + ss(4) + "','" + ss(5) + "')"

            ToolStripStatusLabel1.Text = "信息添加成功"

            Dim oT As MySqlTransaction = conn.BeginTransaction()
            Try
                Dim oC As MySqlCommand
                '下面的commandtext是执行的更新语句，例如Insert into 语句。 
                oC = New MySqlCommand(query_add, conn, oT)
                oC.CommandType = CommandType.Text
                oC.ExecuteNonQuery()
                oT.Commit()
                oC = Nothing
                oT = Nothing

            Catch oe As Exception
                oT.Rollback()
            End Try

            conn.Close()        '关闭数据库
            conn = Nothing      '清空连接的数据库信息
        Catch ex As MySqlException
            ToolStripStatusLabel1.Text = ex.Message
        End Try
    End Sub


    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://www.vastskycc.com/web/demo.html")
    End Sub
End Class
