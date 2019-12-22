use [D:\MAINBACKGROUNDFORM (1)\MAINBACKGROUNDFORM\BIN\DEBUG\NHANVIEN.MDF]

/****** Object:  Table [dbo].[LoginData]    Script Date: 12/24/2019 2:35:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginData](
	[Username] [nvarchar](10) NOT NULL,
	[Password] [nvarchar](10) NULL,
 CONSTRAINT [PK_LoginData] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nhanvien]    Script Date: 12/24/2019 2:35:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nhanvien](
	[Họ tên] [nvarchar](50) NOT NULL,
	[Tên làm việc] [nvarchar](50) NULL,
	[Giới tính] [nvarchar](10) NULL,
	[Địa chỉ] [nvarchar](50) NULL,
	[SĐT] [nvarchar](10) NULL,
	[Ngày sinh] [datetime] NULL,
	[Giờ làm] [nvarchar](50) NULL,
	[Hệ lương] [nvarchar](50) NULL,
	[Lương] [nvarchar](50) NULL,
 CONSTRAINT [PK_Nhanvien] PRIMARY KEY CLUSTERED 
(
	[Họ tên] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into LoginData values('admin','123456')