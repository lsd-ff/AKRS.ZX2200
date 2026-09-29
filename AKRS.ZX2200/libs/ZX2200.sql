USE [master]
GO
/****** Object:  Database [ZX2200]    Script Date: 2025/7/23 14:00:06 ******/
CREATE DATABASE [ZX2200]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ZX2200', FILENAME = N'D:\Program Files\SqlData\ZX2200.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ZX2200_log', FILENAME = N'D:\Program Files\SqlData\ZX2200_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ZX2200] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ZX2200].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ZX2200] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ZX2200] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ZX2200] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ZX2200] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ZX2200] SET ARITHABORT OFF 
GO
ALTER DATABASE [ZX2200] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ZX2200] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ZX2200] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ZX2200] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ZX2200] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ZX2200] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ZX2200] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ZX2200] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ZX2200] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ZX2200] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ZX2200] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ZX2200] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ZX2200] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ZX2200] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ZX2200] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ZX2200] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ZX2200] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ZX2200] SET RECOVERY FULL 
GO
ALTER DATABASE [ZX2200] SET  MULTI_USER 
GO
ALTER DATABASE [ZX2200] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ZX2200] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ZX2200] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ZX2200] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ZX2200] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ZX2200] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ZX2200', N'ON'
GO
ALTER DATABASE [ZX2200] SET QUERY_STORE = ON
GO
ALTER DATABASE [ZX2200] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ZX2200]
GO
/****** Object:  Table [dbo].[AlarmLog]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Message] [varchar](255) NULL,
	[HandleTime] [datetime] NOT NULL,
	[HandleType] [varchar](255) NULL,
	[AlarmCode] [int] NOT NULL,
	[Category] [varchar](255) NULL,
 CONSTRAINT [PK_AlarmLog_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CapacityLog]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CapacityLog](
	[StartTime] [datetime] NOT NULL,
	[TotalNumber] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefectStatisticsEntity]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DefectStatisticsEntity](
	[Time] [datetime] NOT NULL,
	[DefectName] [varchar](255) NOT NULL,
	[TuIndex] [int] NOT NULL,
	[SubIndex] [int] NOT NULL,
	[ModuleIndex] [int] NOT NULL,
	[BondPositionName] [varchar](255) NOT NULL,
	[OffsetX] [float] NOT NULL,
	[OffsetY] [float] NOT NULL,
	[OffsetAngle] [float] NOT NULL,
	[Area] [float] NOT NULL,
	[TpOffsetX] [float] NULL,
	[TpOffsetY] [float] NULL,
	[UpLookOffsetX] [float] NULL,
	[UpLookOffsetY] [float] NULL,
	[ID] [varchar](255) NULL,
	[BondPositionX] [float] NULL,
	[BondPositionY] [float] NULL,
	[BpOffsetX] [float] NULL,
	[BpOffsetY] [float] NULL,
 CONSTRAINT [PK_DefectStatisticsEntity_Time_DefectName] PRIMARY KEY CLUSTERED 
(
	[Time] ASC,
	[DefectName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperateLogEntity]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperateLogEntity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Technician] [varchar](255) NULL,
	[Message] [varchar](255) NULL,
 CONSTRAINT [PK_OperateLogEntity_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParameterChangeLog]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParameterChangeLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Technician] [varchar](255) NULL,
	[Message] [varchar](255) NULL,
	[ParameterName] [varchar](255) NULL,
 CONSTRAINT [PK_ParameterChangeLog_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductTimeStatisticsEntity]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductTimeStatisticsEntity](
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[ProductTime] [float] NOT NULL,
 CONSTRAINT [PK_ProductTimeStatisticsEntity_StartTime_EndTime] PRIMARY KEY CLUSTERED 
(
	[StartTime] ASC,
	[EndTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemperatureCompensationEntity]    Script Date: 2025/7/23 14:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemperatureCompensationEntity](
	[Time] [datetime] NOT NULL,
	[ResultX] [float] NOT NULL,
	[ResultY] [float] NOT NULL,
	[ResultAngle] [float] NOT NULL,
	[AxisResultX] [float] NOT NULL,
	[AxisResultY] [float] NOT NULL,
 CONSTRAINT [PK_TemperatureCompensationEntity_Time] PRIMARY KEY CLUSTERED 
(
	[Time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [ZX2200] SET  READ_WRITE 
GO

GO
/****** Object:  Database [UserManager]    Script Date: 2025/7/23 13:59:41 ******/
CREATE DATABASE [UserManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Test20', FILENAME = N'D:\Program Files\SqlData\UserManager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Test20_log', FILENAME = N'D:\Program Files\SqlData\UserManager.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [UserManager] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UserManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UserManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UserManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UserManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UserManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UserManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [UserManager] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [UserManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UserManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UserManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UserManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UserManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UserManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UserManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UserManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UserManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [UserManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UserManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UserManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UserManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UserManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UserManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UserManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UserManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [UserManager] SET  MULTI_USER 
GO
ALTER DATABASE [UserManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UserManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UserManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UserManager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [UserManager] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [UserManager] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'UserManager', N'ON'
GO
ALTER DATABASE [UserManager] SET QUERY_STORE = ON
GO
ALTER DATABASE [UserManager] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [UserManager]
GO
/****** Object:  Table [dbo].[MenuItem]    Script Date: 2025/7/23 13:59:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ModuleName] [varchar](255) NOT NULL,
	[Code] [varchar](255) NOT NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_MenuItem_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2025/7/23 13:59:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Role_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenuPair]    Script Date: 2025/7/23 13:59:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenuPair](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MenuItemID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsEnable] [int] NOT NULL,
	[DefaultEnableValue] [int] NOT NULL,
 CONSTRAINT [PK_RoleMenuPair_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2025/7/23 13:59:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[Account] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[TelNo] [varchar](255) NOT NULL,
	[Disabled] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_User_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [UserManager]
GO
SET IDENTITY_INSERT [dbo].[MenuItem] ON 
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (1, -1, N'总模块', N'AllModule', N'总模块')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (2, 1, N'自动工作', N'AutoWork', N'自动工作')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (3, 2, N'自动工作', N'ContinueWork', N'当前Pad继续工作')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (4, 2, N'自动工作', N'ReWork', N'当前Pad从头开始做')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (5, 2, N'自动工作', N'RestartFrame', N'WorkFrame从头开始做')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (6, 2, N'自动工作', N'SingleStepRun', N'单步运行')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (7, 1, N'工作台框架', N'WorkbenchFrame', N'工作台框架')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (8, 7, N'工作台框架', N'BondworkProgramSet', N'工作台框架设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (9, 7, N'工作台框架', N'TrayProgram1Set', N'工作台框架设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (10, 7, N'工作台框架', N'TrayProgram2Set', N'Tray1框架设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (11, 1, N'程式', N'Program', N'程式')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (12, 11, N'程式', N'ProgramSetting', N'程式设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (13, 1, N'Bond模组', N'BondModule', N'Bond模组')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (14, 13, N'Bond模组', N'BondHeadSet', N'焊头')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (15, 13, N'Bond模组', N'DripHeadSet', N'点胶头')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (16, 1, N'流道设置', N'RunnerSettings', N'流道设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (17, 16, N'流道设置', N'WorkTableSet', N'流道工作台')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (18, 1, N'其他模组', N'OtherModules', N'其他模组')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (19, 18, N'其他模组', N'LoaderVisionPosSet', N'上料器设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (20, 18, N'其他模组', N'StackLoaderSet', N'堆栈上料')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (21, 1, N'Recipe', N'Recipe', N'Recipe')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (22, 21, N'Recipe', N'RecipeManage', N'Recpie管理')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (23, 21, N'Recipe', N'PRList', N'模板列表')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (24, 1, N'其他', N'Others', N'其他')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (25, 24, N'其他', N'MeasureHeight', N'测高')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (26, 24, N'其他', N'Consumable', N'耗材设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (27, 24, N'其他', N'MeasureCMK', N'CMK')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (28, 24, N'其他', N'SqlButton', N'报警查询')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (29, 24, N'其他', N'Barcode', N'二维码设置')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (30, 1, N'设备维护', N'EquipmentMaintenance', N'设备维护')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (31, 30, N'设备维护', N'LoadHardwareEditor', N'硬件调试')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (32, 30, N'设备维护', N'OneClickReturnToZero', N'一键回零')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (33, 30, N'设备维护', N'InitialMachine', N'轴IO初始化')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (34, 1, N'快捷菜单部分', N'ShortcutMenuSection', N'快捷菜单部分')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (35, 34, N'快捷菜单部分', N'xtraTabPage1', N'快捷菜单')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (36, 34, N'快捷菜单部分', N'xtraTabPage6', N'上料')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (37, 34, N'快捷菜单部分', N'xtraTabPage3', N'固晶模组')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (38, 34, N'快捷菜单部分', N'xtraTabPage4', N'流道')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (39, 34, N'快捷菜单部分', N'xtraTabPage2', N'更换酒精')
GO
INSERT [dbo].[MenuItem] ([ID], [ParentID], [ModuleName], [Code], [Description]) VALUES (40, 34, N'快捷菜单部分', N'xtraTabPage7', N'整机速度')
GO
SET IDENTITY_INSERT [dbo].[MenuItem] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([ID], [Name], [UpdateTime]) VALUES (1, N'管理员', CAST(N'2023-10-12T00:40:20.133' AS DateTime))
GO
INSERT [dbo].[Role] ([ID], [Name], [UpdateTime]) VALUES (2, N'工程师', CAST(N'2023-10-12T00:40:20.157' AS DateTime))
GO
INSERT [dbo].[Role] ([ID], [Name], [UpdateTime]) VALUES (3, N'技术员', CAST(N'2023-10-12T00:40:20.157' AS DateTime))
GO
INSERT [dbo].[Role] ([ID], [Name], [UpdateTime]) VALUES (4, N'操作员', CAST(N'2023-10-12T00:40:20.157' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleMenuPair] ON 
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (1, 1, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (2, 1, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (3, 1, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (4, 1, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (5, 2, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (6, 2, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (7, 2, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (8, 2, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (9, 3, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (10, 3, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (11, 3, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (12, 3, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (13, 4, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (14, 4, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (15, 4, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (16, 4, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (17, 5, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (18, 5, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (19, 5, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (20, 5, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (21, 6, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (22, 6, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (23, 6, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (24, 6, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (25, 7, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (26, 7, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (27, 7, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (28, 7, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (29, 8, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (30, 8, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (31, 8, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (32, 8, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (33, 9, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (34, 9, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (35, 9, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (36, 9, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (37, 10, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (38, 10, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (39, 10, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (40, 10, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (41, 11, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (42, 11, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (43, 11, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (44, 11, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (45, 12, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (46, 12, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (47, 12, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (48, 12, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (49, 13, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (50, 13, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (51, 13, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (52, 13, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (53, 14, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (54, 14, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (55, 14, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (56, 14, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (57, 15, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (58, 15, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (59, 15, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (60, 15, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (61, 16, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (62, 16, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (63, 16, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (64, 16, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (65, 17, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (66, 17, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (67, 17, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (68, 17, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (69, 18, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (70, 18, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (71, 18, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (72, 18, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (73, 19, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (74, 19, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (75, 19, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (76, 19, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (77, 20, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (78, 20, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (79, 20, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (80, 20, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (81, 21, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (82, 21, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (83, 21, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (84, 21, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (85, 22, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (86, 22, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (87, 22, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (88, 22, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (89, 23, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (90, 23, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (91, 23, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (92, 23, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (93, 24, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (94, 24, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (95, 24, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (96, 24, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (97, 25, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (98, 25, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (99, 25, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (100, 25, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (101, 26, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (102, 26, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (103, 26, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (104, 26, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (105, 27, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (106, 27, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (107, 27, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (108, 27, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (109, 28, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (110, 28, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (111, 28, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (112, 28, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (113, 29, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (114, 29, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (115, 29, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (116, 29, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (117, 30, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (118, 30, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (119, 30, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (120, 31, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (121, 31, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (122, 31, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (123, 31, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (124, 32, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (125, 32, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (126, 32, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (127, 32, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (128, 33, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (129, 33, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (130, 33, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (131, 33, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (132, 34, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (133, 34, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (134, 34, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (135, 34, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (136, 35, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (137, 35, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (138, 35, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (139, 35, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (140, 36, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (141, 36, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (142, 36, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (143, 36, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (144, 37, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (145, 37, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (146, 37, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (147, 37, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (148, 38, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (149, 38, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (150, 38, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (151, 38, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (152, 39, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (153, 39, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (154, 39, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (155, 39, 4, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (156, 40, 1, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (157, 40, 2, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (158, 40, 3, 0, 1)
GO
INSERT [dbo].[RoleMenuPair] ([ID], [MenuItemID], [RoleID], [IsEnable], [DefaultEnableValue]) VALUES (159, 40, 4, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[RoleMenuPair] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([ID], [RoleID], [Account], [Password], [Name], [TelNo], [Disabled], [UpdateTime]) VALUES (1, 1, N'Admin', N'123456', N'管理员', N'', 0, CAST(N'2023-10-12T00:40:20.157' AS DateTime))
GO
INSERT [dbo].[User] ([ID], [RoleID], [Account], [Password], [Name], [TelNo], [Disabled], [UpdateTime]) VALUES (2, 2, N'Engineer', N'123456', N'工程师', N'', 0, CAST(N'2023-10-12T00:40:20.167' AS DateTime))
GO
INSERT [dbo].[User] ([ID], [RoleID], [Account], [Password], [Name], [TelNo], [Disabled], [UpdateTime]) VALUES (3, 3, N'Technician', N'123456', N'技术员', N'', 0, CAST(N'2023-10-12T00:40:20.167' AS DateTime))
GO
INSERT [dbo].[User] ([ID], [RoleID], [Account], [Password], [Name], [TelNo], [Disabled], [UpdateTime]) VALUES (4, 4, N'Operator', N'123456', N'操作员', N'', 0, CAST(N'2023-10-12T00:40:20.170' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
USE [master]
GO
ALTER DATABASE [UserManager] SET  READ_WRITE 
GO
