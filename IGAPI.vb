Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Public Class VistaGram
    'VistaGram (INstagramAPI) Open Source
    ' Written By Alkaymary Coder
    'INstagram:@Ak.coder
    'Telegram:@Ko_Tools
    'Created On 2020/4/20 10:30:42 AM
#Region "VistaGram(InstagramAPI) API"
#Region "Main Variables"
    Private Shared MainCookies, LogCookies As New CookieContainer()
    Private Shared _Csrftoken As String = ""
#End Region
#Region "Account section"
    Public Shared Function SignIn(ByVal UserName As String, ByVal Password As String, ByVal Proxy As String) As String

        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/accounts/login/ajax/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = MainCookies
                .Headers.Add("X-CSRFToken", "missing")
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If Proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(Proxy)
                End If
            End With
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes("username=" & UserName & "&password=" & Password)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            MainCookies.Add(response.Cookies) : _Csrftoken = ""
            For Each cookie In response.Cookies
                If responseFromServer.Contains("""authenticated"": true") Then
                    LogCookies.Add(response.Cookies)
                End If
                If Not cookie.ToString = "csrftoken=""""" Then If cookie.ToString.Contains("csrftoken") Then _Csrftoken = cookie.ToString.Replace("csrftoken=", "")
            Next
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
    Public Shared Function SignUp(ByVal Full_Name As String, ByVal UserName As String, ByVal Email As String, ByVal Password As String, ByVal Proxy As String) As String
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/accounts/web_create_ajax/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = MainCookies
                .Headers.Add("X-CSRFToken", "missing")
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If Proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(Proxy)
                End If
            End With
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes("email=" + Email + "&password=" & Password & "&username=" & UserName & "&first_name=" & Full_Name & "&month=3&day=5&year=2000&seamless_login_enabled=1&tos_version=row")
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
    Public Shared Function Forget_Password(ByVal UserName_Or_Email_Or_Number As String, ByVal proxy As String) As String
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/accounts/account_recovery_send_ajax/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = MainCookies
                .Headers.Add("X-CSRFToken", "missing")
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(proxy)
                End If
            End With
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes("email_or_username=" & UserName_Or_Email_Or_Number & "&recaptcha_challenge_field=")
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
#End Region
#Region "Fetching information section"
    Private Shared Function GetUID(ByVal IMG As String) As String
        Return Regex.Match(New Net.WebClient().DownloadString("https://api.instagram.com/oembed/?callback=&url=" + IMG), """media_id"": ""(.*?)"",").Groups(1).Value
    End Function
    Private Shared Function GetID(ByVal UserName As String) As String
        Return Regex.Match(New Net.WebClient().DownloadString("https://www.instagram.com/" + UserName + "/?__a=1"), """profilePage_(.*?)"",").Groups(1).Value
    End Function
#End Region
#Region "Interaction section"
    Public Shared Function add_Like(ByVal PostUrl As String, ByVal Proxy As String) As String
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/web/likes/" + GetUID(PostUrl) + "/like/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = LogCookies
                .Headers.Add("X-CSRFToken", _Csrftoken)
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If Proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(Proxy)
                End If
            End With
            Dim dataStream As Stream = request.GetRequestStream()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone

    End Function
    Public Shared Function add_Follow(ByVal Username As String, ByVal proxy As String)
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/web/friendships/" + GetID(Username) + "/follow/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = LogCookies
                .Headers.Add("X-CSRFToken", _Csrftoken)
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(proxy)
                End If
            End With
            Dim dataStream As Stream = request.GetRequestStream()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
    Public Shared Function add_Comment(ByVal Postlink As String, ByVal Comment As String, ByVal proxy As String) As String
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/web/comments/" + GetUID(Postlink) + "/add/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = LogCookies
                .Headers.Add("X-CSRFToken", _Csrftoken)
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(proxy)
                End If
            End With
            Dim postData As String = "comment_text=" & Comment & "&replied_to_comment_id="
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
#End Region
#Region "Publications pull section"
    Public Shared Function PostViaHashtag(ByVal Hashtag As String)
        Dim PostList As String
        Dim Respone As String = ""
        Try
            Dim A_, _k, _S As New System.Net.WebClient
            Dim result = A_.DownloadString("https://www.instagram.com/explore/tags/" + Hashtag.Replace("#", "") + "/?__a=1")
            Dim MC1 As MatchCollection = Regex.Matches(result, "shortcode"":""(.*?)""")
            Dim EC1 As IEnumerator = MC1.GetEnumerator() '   
            While EC1.MoveNext
                PostList += "https://www.instagram.com/p/" + Regex.Match(CType(EC1.Current, Match).ToString(), "shortcode"":""(.*?)""").Groups(1).Value + vbCrLf
            End While
            Respone = PostList
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
    Public Shared Function PostViaUserName(ByVal UserName As String)
        Dim PostList As String
        Dim Respone As String = ""
        Try
            Dim A_, _k, _S As New System.Net.WebClient
            Dim result = A_.DownloadString("https://www.instagram.com/explore/tags/" + UserName.Replace("@", "") + "/?__a=1")
            Dim MC1 As MatchCollection = Regex.Matches(result, "shortcode"":""(.*?)""")
            Dim EC1 As IEnumerator = MC1.GetEnumerator() '   
            While EC1.MoveNext
                PostList += "https://www.instagram.com/p/" + Regex.Match(CType(EC1.Current, Match).ToString(), "shortcode"":""(.*?)""").Groups(1).Value + vbCrLf
            End While
            Respone = PostList
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
#End Region
#Region "Modification section within the account"
    Public Shared Function editProfile(ByVal Full_Name As String, ByVal UserName As String, ByVal Email As String, ByVal phone_number As String, ByVal external_url As String, ByVal biography As String, ByVal Proxy As String) As String
        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/accounts/edit/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = LogCookies
                .Headers.Add("X-CSRFToken", _Csrftoken)
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If Proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(Proxy)
                End If
            End With
            Dim postData As String = "first_name=" & Full_Name & "&email=" & Email & "&username=" & UserName & "&phone_number=" & phone_number & "&biography=" & biography & "&external_url=" & external_url & "&chaining_enabled=on"
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
    Public Shared Function ChangePassword(ByVal oldPassword As String, ByVal NewPassWord As String, ByVal Proxy As String) As String

        Dim Respone As String = ""
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://www.instagram.com/accounts/login/ajax/") : With request
                .Method = ("POST")
                .Accept = ("*/*")
                .CookieContainer = LogCookies
                .Headers.Add("X-CSRFToken", _Csrftoken)
                .Headers.Add("X-Requested-With", "XMLHttpRequest")
                .Headers.Add("Accept-Language", "ar,en-US;q=0.9,en;q=0.8")
                .ContentType = ("application/x-www-form-urlencoded; charset=UTF-8")
                .UserAgent = ("Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0")
                If Proxy = Nothing Then
                    .Proxy = Nothing
                Else
                    .Proxy = New WebProxy(Proxy)
                End If
            End With
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes("old_password=" + oldPassword + "&new_password1=" + NewPassWord + "&new_password2=" + NewPassWord)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            Respone = responseFromServer
        Catch ex As WebException
            Dim R2 As HttpWebResponse = ex.Response
            Dim D2 = R2.GetResponseStream()
            Dim reader As New StreamReader(D2)
            Dim Res2 As String = reader.ReadToEnd()
            Respone = Res2
        End Try
        Return Respone
    End Function
#End Region
#End Region
End Class
