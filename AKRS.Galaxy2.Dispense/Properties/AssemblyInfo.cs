using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过下列属性集
// 控制。更改这些属性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("AKRS.Galaxy2.Dispense")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("AKRS.Galaxy2.Dispense")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 属性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("49dbb2e6-e78b-4684-be46-cee8c4df22d1")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
// [assembly: AssemblyFileVersion("1.0.*")]

/*
 * 1.0.0.1
 * 2016-07-06
 * 最初版。
 * 
 * 1.0.1.*
 * 2016-07-07
 * （1）使用一种注册处理器的方式。
 * （2）完善了事件。
 * 
 * 
 * 1.0.2.*
 * 2016-07-12
 * （1）DispensingPatternControl类增加了一个int类型的字段_offset。
 * 
 * 
 * 
 * 1.0.3.*
 * 2016-07-13
 *	（1）支持控件缩放和动态设置网格尺寸。
 *	（2）坐标系使用标准笛卡尔坐标系，并以晶片中心作为坐标系原点。
 *	（3）增加了ChipSize属性。
 *	（4）增加了DispensingPatternForm。
 *	
 * 
 * 1.0.4.*
 * 2016-07-14
 *	（1）增加了DispensingPatternFormX，是DispensingPatternForm的DotNetBar变体。
 *	（2）修正了MovePointHandler.HandleMouseDownEvent方法会因为数组越界抛出异常的问题，
 *		以及由此引发一个逻辑问题：鼠标在线段附件点击会导致最近一次被移动的点跑到鼠标位置，
 *		并且成为“当前被移动的点”。
 *		
 * 
 * 
 * 1.0.5.*
 *	（1）调整了DispensingPatternFormX的listbox1内容的显示风格。
 *	（2）DispensingPatternControl增加了一个删除线的方法：RemoveLineByIndex；
 *		DispensingPatternFormX的listViewEx1增加了选中后右击弹出菜单的功能：菜单中有【删除】当
 *		前选中项的功能。
 *	（3）更正了DispensingPatternFormX的listViewEx1的一个显示错误：每次图形中有点增加了，总是
 *		把新增点的信息追加到最后一条记录中。
 *	（4）开放了【还原】已”隐藏线“的功能。
 *	
 * 
 * 
 * 1.0.6.*
 *	（1）DispensingPatternControl类增加了方法“SetData”，该方法的目的是在显示控件之前，先把控件
 *		的内容（线和点的信息）设置好。此方法由DispensingPatternFormX类的新增方法“SetData”调用。
 *	
 *		DispensingPatternFormX类增加了方法“SetData”，该方法的目的是在显示窗口之前，先把窗口的一些
 *		信息设置好，包括其所拥有的DispensingPatternControl控件的信息。
 *		
 *		DispensingPatternFormX类的增加新方法“GetData”，取代原“GetResult”方法。这个新方法返回一个
 *		DispensingPatternData类型的对象，其包含了丰富的信息。
 *		
 *		新类DispensingPatternData（在DispensingPatternData.cs文件中）主要用于DispensingPatternFormX的
 *		SetData方法和GetData方法的参数。
 *		
 *		把原来的LineData结构和LogicalPointData结构合并为PointEx类型，这个新类型也是用于存储逻辑点
 *		的信息的。
 *		
 *		对DispensingPatternFormX窗口中显示的子控件做了重新排列。增加了【显示密度】和【单位精度】
 *		的可选项。
 *		
 *		单位改为微米（um）。
 *		
 *		DispensingPatternControl控件的点显示得更大了。
 *		
 *	（2）在窗口标题中显示版本号。
 *	
 * 1.0.7.*
 * （1）
 * 
 * 
 * 1.1.0.*
 * (1)	增加了类：SelectPointHandler，SelectSegmentHandler，UserControl1（暂时这个类名）。
 *		增加了结构：PointAdditionalInfo。
 *		
 *		PointAdditionalInfo结构用于保存点的额外信息，目前有时间(Time)和速度(Velocity)。
 *		当m_bVelocity为true时，m_iVelocity的值才有意义；
 *		当m_bTime;为true时，m_iTime的值才有意义。
 * 
 *		PointEx类增加了一个PointAdditionalInfo类型的字段_addInfo，并为此增加了构造函数。
 *		
 *		UserControl1类是用户控件，用于向用户显示点的额外信息，同时用户通过这个控件来设置当前点或线段的额外信息。
 *		注意：其实线段是通过起点和终点来表示的，所以线段的额外信息是保存在点的额外信息中的。
 *		
 *		SelectPointHandler类 对“选中点”操作的处理。
 *		
 *		SelectSegmentHandler类 对“选中线段”操作的处理。
 *		
 *		DispensingPatternControl控件 定义了新的事件：PointSelected 和 SegmentSelected。
 *		
 * 1.1.1.*
 * (1)	增加了控件类UserControl2，与UserControl1是非常相似的；
 *		控件精简了UserControl1的功能，现在只用于“编辑线”，“编辑点”的部分由UserControl2来提供；
 *		PointAdditionalInfo类增加了“高度补偿”字段和属性。
 *
 * 1.1.2.*
 *  1. 调整版本的控制方式，新的版本表示中第四位代表Build号。
 *  2. “高度补偿”支持负值。即UserControl2的integerInputCompensation控件支持负值。（前一个版本（1.1.1.1）或许已经对此项支持了。）
 *  
 * 
 * 
 * 2017-05-10
 * 2.0.0.*
 * 
 * 
 * 
 * 
 * 2017-05-26
 * 2.1.0.*
 * 主要改动：增加了对多条曲线的支持。
 * 
 * 2017-06-01
 * 2.1.1.*
 * 增加了“出胶前延时”和“出胶后延时”。
 * 
 * 2017-06-02
 * 2.1.2.*
 * (1) 显示鼠标位置的坐标值，以及显示当前放大倍率。
 * (2) 通过颜色来区分出当前曲线。
 * (3) 用Dash Line表示未决的线段。
 * 
 * 
 * 2017-06-06
 * 2.1.3.*
 * (1) 曲线的起点已“空心圆”表示。
 * (2) “空过”的线段以虚线表示。
 * (3) 通过颜色来区分当前曲线及其他曲线。
 * (4) 当前线段加粗显示。
 * (5) 在线段列表选中的线段就是加粗显示的当前线段。
 * 
 * 
 * 2017-06-06
 * 2.2.0.*
 * (1) 项目更名为AKRS.Galaxy2.Dispense。
 * (2) 移除了多余和不必要的类。
 * 
 * 
 * 
 * 2017-06-07
 * 2.2.1.*
 * (1) 增加末端速率。
 * 
 * 
 * 2017-06-08
 * 2.2.2.*
 * (1) 点使用单精度浮点类型；高倍率下点的精度得以保持。
 * 
 * 
 * 2017-08-23
 * 2.2.3.*
 * EpoxyPatternControl类增加了MoveCurve和MoveAllCurves方法。
 * EpoxyPatternFormX类增加了移动曲线的按钮。
 * 
 * 
 * 2019-01-15
 * 2.2.4.*
 * 1. 使用了Point2D来表示点。
 * 2. 改进了EpoxyPatternFormX的界面，现在能显示选定线段的两个端点的信息，并且能够修改端点的高度。
 * 
 * 2019-01-25
 * 2.2.5.*
 * 1. 
 * 
 * 
 * 2019-04-18
 * 2.2.6.0
 * 给端点增加了一个属性，用于标识测高。
 * 
 * 2019-04-26
 * 2.2.7.0
 * 增加了移动线段的功能。
 */



[assembly: AssemblyVersion("2.2.7.0")]
