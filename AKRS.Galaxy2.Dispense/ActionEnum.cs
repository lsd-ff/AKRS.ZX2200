namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 动作枚举
    /// </summary>
	public enum ActionEnum
	{
		None,

        PaintCurve,
        PaintPoint,
        SelectObject,

		HideSegment,
		EraseSegment,		
		MoveSegment,	
		SelectSegment,        

        AppendPoint,
        DeletePoint,
        InsertPoint,
        MovePoint,
        SelectPoint,

        TranslateCurve,
        ScaleCurve,
        RotateCurve,
        
        TranslateBackgroundImage,
        RotateBackgroundImage,
	}
}