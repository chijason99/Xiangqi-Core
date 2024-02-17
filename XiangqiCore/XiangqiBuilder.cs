namespace XiangqiCore;

public class XiangqiBuilder : IXianqgiBuilder
{
    private const string _defaultStartingPositionFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w";

    public XiangqiBuilder()
    {
    }
    public string InitialFen { get; private set; }
    public Side SideToMove { get; private set; }

    public XiangqiBuilder UseDefaultConfiguration()
    {
        InitialFen = _defaultStartingPositionFen;
        SideToMove = Side.Red;

        return this;
    }

    public XiangqiBuilder UseCustomFen(string customFen)
    {
        InitialFen = customFen;

        return this;
    }

    public XiangqiGame Build()
    {
        return new XiangqiGame(initialFenString: InitialFen, sideToMove:SideToMove);
    }
}