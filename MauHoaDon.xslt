<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<head>
				<title>HÓA ĐƠN BÁN LẺ</title>
				<style>
					body { font-family: Arial; text-align: center; background: #f0f0f0; }
					.bill { width: 400px; margin: 20px auto; background: white; padding: 20px; border: 1px solid #ccc; box-shadow: 5px 5px 10px #888; }
					h1 { color: red; border-bottom: 2px dashed red; padding-bottom: 10px; }
					.info { text-align: left; margin: 10px 0; }
					.total { font-weight: bold; font-size: 20px; color: blue; margin-top: 20px; border-top: 2px solid #000; padding-top: 10px; }
				</style>
			</head>
			<body>
				<div class="bill">
					<h1>HÓA ĐƠN THANH TOÁN</h1>
					<div class="info">
						<p>
							Mã HĐ: <b>
								<xsl:value-of select="HoaDon/MaHD"/>
							</b>
						</p>
						<p>
							Ngày lập: <xsl:value-of select="HoaDon/NgayLap"/>
						</p>
						<p>
							Thu ngân: <xsl:value-of select="HoaDon/MaNV"/>
						</p>
						<hr/>
						<p>
							Khách hàng: <xsl:value-of select="HoaDon/TenKhach"/>
						</p>
						<p>
							Sản phẩm: <b>
								<xsl:value-of select="HoaDon/TenSP"/>
							</b>
						</p>
						<p>
							Số lượng: <xsl:value-of select="HoaDon/SoLuong"/>
						</p>
						<p>Đơn giá: ... VNĐ</p>
					</div>
					<div class="total">
						TỔNG TIỀN: <xsl:value-of select="HoaDon/ThanhTien"/> VNĐ
					</div>
					<p style="margin-top:20px; font-style:italic;">Cảm ơn quý khách!</p>
				</div>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>