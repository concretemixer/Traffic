using System;

namespace Commons.SN
{
    public interface IPostData
    {
        string Link { get; }
        string Picture { get; }
        string LinkName { get; }
        string LinkCaption { get; }
        string LinkDescription { get; }
    }
}