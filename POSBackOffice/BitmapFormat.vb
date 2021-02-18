Imports System.Runtime.InteropServices
Imports System.IO

Namespace Sample
	Public Class BitmapFormat
		Public Structure BITMAPFILEHEADER
			Public bfType As UShort
			Public bfSize As Integer
			Public bfReserved1 As UShort
			Public bfReserved2 As UShort
			Public bfOffBits As Integer
		End Structure

		Public Structure MASK
			Public redmask As Byte
			Public greenmask As Byte
			Public bluemask As Byte
			Public rgbReserved As Byte
		End Structure

		Public Structure BITMAPINFOHEADER
			Public biSize As Integer
			Public biWidth As Integer
			Public biHeight As Integer
			Public biPlanes As UShort
			Public biBitCount As UShort
			Public biCompression As Integer
			Public biSizeImage As Integer
			Public biXPelsPerMeter As Integer
			Public biYPelsPerMeter As Integer
			Public biClrUsed As Integer
			Public biClrImportant As Integer
		End Structure

		'******************************************
'        * º¯ÊýÃû³Æ£ºRotatePic       
'        * º¯Êý¹¦ÄÜ£ºÐý×ªÍ¼Æ¬£¬Ä¿µÄÊÇ±£´æºÍÏÔÊ¾µÄÍ¼Æ¬Óë°´µÄÖ¸ÎÆ·½Ïò²»Í¬     
'        * º¯ÊýÈë²Î£ºBmpBuf---Ðý×ªÇ°µÄÖ¸ÎÆ×Ö·û´®
'        * º¯Êý³ö²Î£ºResBuf---Ðý×ªºóµÄÖ¸ÎÆ×Ö·û´®
'        * º¯Êý·µ»Ø£ºÎÞ
'        ********************************************

		Public Shared Sub RotatePic(BmpBuf As Byte(), width As Integer, height As Integer, ByRef ResBuf As Byte())
			Dim RowLoop As Integer = 0
			Dim ColLoop As Integer = 0
			Dim BmpBuflen As Integer = width * height

			Try
				RowLoop = 0
				While RowLoop < BmpBuflen
					For ColLoop = 0 To width - 1
						ResBuf(RowLoop + ColLoop) = BmpBuf(BmpBuflen - RowLoop - width + ColLoop)
					Next

					RowLoop = RowLoop + width
				End While
					'ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
					'logger.Append();
			Catch ex As Exception
			End Try
		End Sub

		'******************************************
'        * º¯ÊýÃû³Æ£ºStructToBytes       
'        * º¯Êý¹¦ÄÜ£º½«½á¹¹Ìå×ª»¯³ÉÎÞ·ûºÅ×Ö·û´®Êý×é     
'        * º¯ÊýÈë²Î£ºStructObj---±»×ª»¯µÄ½á¹¹Ìå
'        *           Size---±»×ª»¯µÄ½á¹¹ÌåµÄ´óÐ¡
'        * º¯Êý³ö²Î£ºÎÞ
'        * º¯Êý·µ»Ø£º½á¹¹Ìå×ª»¯ºóµÄÊý×é
'        ********************************************

		Public Shared Function StructToBytes(StructObj As Object, Size As Integer) As Byte()
			Dim StructSize As Integer = Marshal.SizeOf(StructObj)
			Dim GetBytes As Byte() = New Byte(StructSize - 1) {}

			Try
				Dim StructPtr As IntPtr = Marshal.AllocHGlobal(StructSize)
				Marshal.StructureToPtr(StructObj, StructPtr, False)
				Marshal.Copy(StructPtr, GetBytes, 0, StructSize)
				Marshal.FreeHGlobal(StructPtr)

				If Size = 14 Then
					Dim NewBytes As Byte() = New Byte(Size - 1) {}
					Dim Count As Integer = 0
					Dim [Loop] As Integer = 0

					For [Loop] = 0 To StructSize - 1
						If [Loop] <> 2 AndAlso [Loop] <> 3 Then
							NewBytes(Count) = GetBytes([Loop])
							Count += 1
						End If
					Next

					Return NewBytes
				Else
					Return GetBytes
				End If
			Catch ex As Exception
				'ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
				'logger.Append();

				Return GetBytes
			End Try
		End Function

		'******************************************
'        * º¯ÊýÃû³Æ£ºGetBitmap       
'        * º¯Êý¹¦ÄÜ£º½«´«½øÀ´µÄÊý¾Ý±£´æÎªÍ¼Æ¬     
'        * º¯ÊýÈë²Î£ºbuffer---Í¼Æ¬Êý¾Ý
'        *           nWidth---Í¼Æ¬µÄ¿í¶È
'        *           nHeight---Í¼Æ¬µÄ¸ß¶È
'        * º¯Êý³ö²Î£ºÎÞ
'        * º¯Êý·µ»Ø£ºÎÞ
'        ********************************************

		Public Shared Sub GetBitmap(buffer As Byte(), nWidth As Integer, nHeight As Integer, ByRef ms As MemoryStream)
			Dim ColorIndex As Integer = 0
			Dim m_nBitCount As UShort = 8
			Dim m_nColorTableEntries As Integer = 256
			Dim ResBuf As Byte() = New Byte(nWidth * nHeight * 2 - 1) {}

			Try
				Dim BmpHeader As New BITMAPFILEHEADER()
				Dim BmpInfoHeader As New BITMAPINFOHEADER()
				Dim ColorMask As MASK() = New MASK(m_nColorTableEntries - 1) {}

				Dim w As Integer = (((nWidth + 3) \ 4) * 4)

				'Í¼Æ¬Í·ÐÅÏ¢
				BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader)
				BmpInfoHeader.biWidth = nWidth
				BmpInfoHeader.biHeight = nHeight
				BmpInfoHeader.biPlanes = 1
				BmpInfoHeader.biBitCount = m_nBitCount
				BmpInfoHeader.biCompression = 0
				BmpInfoHeader.biSizeImage = 0
				BmpInfoHeader.biXPelsPerMeter = 0
				BmpInfoHeader.biYPelsPerMeter = 0
				BmpInfoHeader.biClrUsed = m_nColorTableEntries
				BmpInfoHeader.biClrImportant = m_nColorTableEntries

				'ÎÄ¼þÍ·ÐÅÏ¢
				BmpHeader.bfType = &H4d42
				BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4
				BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) \ 32) * 4) * BmpInfoHeader.biHeight)
				BmpHeader.bfReserved1 = 0
				BmpHeader.bfReserved2 = 0

				ms.Write(StructToBytes(BmpHeader, 14), 0, 14)
				ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader))

				'µ÷ÊÔ°åÐÅÏ¢
				For ColorIndex = 0 To m_nColorTableEntries - 1
					ColorMask(ColorIndex).redmask = CByte(ColorIndex)
					ColorMask(ColorIndex).greenmask = CByte(ColorIndex)
					ColorMask(ColorIndex).bluemask = CByte(ColorIndex)
					ColorMask(ColorIndex).rgbReserved = 0

					ms.Write(StructToBytes(ColorMask(ColorIndex), Marshal.SizeOf(ColorMask(ColorIndex))), 0, Marshal.SizeOf(ColorMask(ColorIndex)))
				Next

				'Í¼Æ¬Ðý×ª£¬½â¾öÖ¸ÎÆÍ¼Æ¬µ¹Á¢µÄÎÊÌâ
				RotatePic(buffer, nWidth, nHeight, ResBuf)

				Dim filter As Byte() = Nothing
				If w - nWidth > 0 Then
					filter = New Byte(w - nWidth - 1) {}
				End If
				For i As Integer = 0 To nHeight - 1
					ms.Write(ResBuf, i * nWidth, nWidth)
					If w - nWidth > 0 Then
						ms.Write(ResBuf, 0, w - nWidth)
					End If
				Next
					' ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
					' logger.Append();
			Catch ex As Exception
			End Try
		End Sub

		'******************************************
'        * º¯ÊýÃû³Æ£ºWriteBitmap       
'        * º¯Êý¹¦ÄÜ£º½«´«½øÀ´µÄÊý¾Ý±£´æÎªÍ¼Æ¬     
'        * º¯ÊýÈë²Î£ºbuffer---Í¼Æ¬Êý¾Ý
'        *           nWidth---Í¼Æ¬µÄ¿í¶È
'        *           nHeight---Í¼Æ¬µÄ¸ß¶È
'        * º¯Êý³ö²Î£ºÎÞ
'        * º¯Êý·µ»Ø£ºÎÞ
'        ********************************************

		Public Shared Sub WriteBitmap(buffer As Byte(), nWidth As Integer, nHeight As Integer)
			Dim ColorIndex As Integer = 0
			Dim m_nBitCount As UShort = 8
			Dim m_nColorTableEntries As Integer = 256
			Dim ResBuf As Byte() = New Byte(nWidth * nHeight - 1) {}

			Try

				Dim BmpHeader As New BITMAPFILEHEADER()
				Dim BmpInfoHeader As New BITMAPINFOHEADER()
				Dim ColorMask As MASK() = New MASK(m_nColorTableEntries - 1) {}
				Dim w As Integer = (((nWidth + 3) \ 4) * 4)
				'Í¼Æ¬Í·ÐÅÏ¢
				BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader)
				BmpInfoHeader.biWidth = nWidth
				BmpInfoHeader.biHeight = nHeight
				BmpInfoHeader.biPlanes = 1
				BmpInfoHeader.biBitCount = m_nBitCount
				BmpInfoHeader.biCompression = 0
				BmpInfoHeader.biSizeImage = 0
				BmpInfoHeader.biXPelsPerMeter = 0
				BmpInfoHeader.biYPelsPerMeter = 0
				BmpInfoHeader.biClrUsed = m_nColorTableEntries
				BmpInfoHeader.biClrImportant = m_nColorTableEntries

				'ÎÄ¼þÍ·ÐÅÏ¢
				BmpHeader.bfType = &H4d42
				BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4
				BmpHeader.bfSize = BmpHeader.bfOffBits + ((((w * BmpInfoHeader.biBitCount + 31) \ 32) * 4) * BmpInfoHeader.biHeight)
				BmpHeader.bfReserved1 = 0
				BmpHeader.bfReserved2 = 0

				Dim FileStream As Stream = File.Open("finger.bmp", FileMode.Create, FileAccess.Write)
				Dim TmpBinaryWriter As New BinaryWriter(FileStream)

				TmpBinaryWriter.Write(StructToBytes(BmpHeader, 14))
				TmpBinaryWriter.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)))

				'µ÷ÊÔ°åÐÅÏ¢
				For ColorIndex = 0 To m_nColorTableEntries - 1
					ColorMask(ColorIndex).redmask = CByte(ColorIndex)
					ColorMask(ColorIndex).greenmask = CByte(ColorIndex)
					ColorMask(ColorIndex).bluemask = CByte(ColorIndex)
					ColorMask(ColorIndex).rgbReserved = 0

					TmpBinaryWriter.Write(StructToBytes(ColorMask(ColorIndex), Marshal.SizeOf(ColorMask(ColorIndex))))
				Next

				'Í¼Æ¬Ðý×ª£¬½â¾öÖ¸ÎÆÍ¼Æ¬µ¹Á¢µÄÎÊÌâ
				RotatePic(buffer, nWidth, nHeight, ResBuf)

				'Ð´Í¼Æ¬
				'TmpBinaryWriter.Write(ResBuf);
				Dim filter As Byte() = Nothing
				If w - nWidth > 0 Then
					filter = New Byte(w - nWidth - 1) {}
				End If
				For i As Integer = 0 To nHeight - 1
					TmpBinaryWriter.Write(ResBuf, i * nWidth, nWidth)
					If w - nWidth > 0 Then
						TmpBinaryWriter.Write(ResBuf, 0, w - nWidth)
					End If
				Next

				FileStream.Close()
				TmpBinaryWriter.Close()
					'ZKCE.SysException.ZKCELogger logger = new ZKCE.SysException.ZKCELogger(ex);
					'logger.Append();
			Catch ex As Exception
			End Try
		End Sub
	End Class
End Namespace
