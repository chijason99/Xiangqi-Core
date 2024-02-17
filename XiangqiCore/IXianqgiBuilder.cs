namespace XiangqiCore;

public interface IXianqgiBuilder
{
    XiangqiBuilder UseDefaultConfiguration();
    XiangqiBuilder UseCustomFen(string customFen);
    XiangqiGame Build();
}